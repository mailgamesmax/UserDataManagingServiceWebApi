using System.Security.Cryptography;
using System;
using Microsoft.EntityFrameworkCore;
using UserDataManagingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace UserDataManagingService.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User CreateUser(string newName, string newLastName, string newNickName, string password, string personalCode, string phoneNr, string email)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var createdAcc = new User
            {
                Name = newName,
                LastName = newLastName,
                NickName = newNickName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PersonalCode = personalCode,
                PhoneNr = phoneNr,
                Email = email,

                Role = Role.DefaultUser,
            };
            return createdAcc;
        }
        public (byte[], byte[]) CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return (passwordHash, passwordSalt);
        }

        public async Task<User> GetFullUserByNickname(string nickName)
        {
            var tryGetExistingUser = await UserNickNameExistAlready(nickName);
            
            if (tryGetExistingUser.Item1 == true)
            {
                return tryGetExistingUser.Item2;
            }
            else
            {
                return null;
            }
        }

        public async Task<User> GetFullUserById(Guid userId)
        {
            var targetUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (targetUser != null)
            {
                return targetUser;
            }
            return null;

        }

        public async Task<bool> GetUserActiveStatusByUserId(Guid userId)
        {
            var targetUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            return targetUser.UserIsActive;
        }

        public async Task<Guid> GetUserIdByNickname(string nickName)
        {
            var tryGetExistingUser = await UserNickNameExistAlready(nickName);
            if (tryGetExistingUser.Item1 == true)
            {
                return tryGetExistingUser.Item2.UserId;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public async Task<string> GetUserRoleById(Guid userId)
        {
            var targetUser = await GetFullUserById(userId);
            if (targetUser == null)
            {
                return (string.Empty);
            }            
            return targetUser.Role.ToString();
        }

        public async Task<User> AvatarIdMappingToUserId(Guid userId, Avatar avatar)
        {
            var targetUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            targetUser.Avatar = avatar;
            targetUser.AvatarId = avatar.Avatar_Id;
            //await _appDbContext.SaveChangesAsync();
            return targetUser;
        }

        public async Task<(bool, User user)> UserNickNameExistAlready(string inputedNickName)
        {
            var anyUser = await _appDbContext.Users.SingleOrDefaultAsync(a => a.NickName == inputedNickName);

            if (anyUser != null)
            {
                return (true, anyUser);
            }
            else
            {
                return (false, null);
            }
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var targetUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (targetUser != null)
            {
                _appDbContext.Users.Remove(targetUser);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UserDataAreNotNullOrWihteSpaceAndMapped(User user)
        {
            var livingPlace = await _livingPlaceRepository.GetLivingPlaceDataByUserID(user.UserId);
            var avatar = await   _avatarRepository.GetAvatarByUserID(user.UserId);
            return
                !string.IsNullOrWhiteSpace(user.Name) &&
                !string.IsNullOrWhiteSpace(user.NickName) &&
                !string.IsNullOrWhiteSpace(user.LastName) &&
                !string.IsNullOrWhiteSpace(user.PersonalCode) &&
                !string.IsNullOrWhiteSpace(user.Email) &&
                user.PasswordHash != null &&
                user.PasswordSalt != null &&
                !string.IsNullOrWhiteSpace(user.Role.ToString()) &&
                user.LivingPlaceId == livingPlace.LivingPlace_Id &&
                user.AvatarId == avatar.Avatar_Id;
                
                //!string.IsNullOrWhiteSpace(user.LivingPlaceId.ToString()) &&
                //!string.IsNullOrWhiteSpace(user.AvatarId.ToString());
        }

        public Guid ConvertStringToGuid(string anyString)
        {
            Guid guidFromString;
            if (Guid.TryParse(anyString, out guidFromString))
            {
                return guidFromString;
            }
            return Guid.Empty;
        }

        //
        private readonly AppDbContext _appDbContext;
        private readonly ILivingPlaceRepository _livingPlaceRepository;
        private readonly IAvatarRepository _avatarRepository;
        //private readonly IUserLoginAndCreateService _userLoginAndCreateService;
        public UserRepository(AppDbContext dbContext, ILivingPlaceRepository livingPlaceRepository, IAvatarRepository avatarRepository)
        {
            _appDbContext = dbContext;
            _livingPlaceRepository = livingPlaceRepository;
            _avatarRepository = avatarRepository;
        }

    }
}

