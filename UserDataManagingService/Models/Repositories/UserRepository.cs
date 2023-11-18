using System.Security.Cryptography;
using System;
using Microsoft.EntityFrameworkCore;
using UserDataManagingService.Services;

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
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public async Task<User> GetFullUserById(Guid userId)
        {
            var userById = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            return userById;
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


        //
        private readonly AppDbContext _appDbContext;
        //private readonly IUserLoginAndCreateService _userLoginAndCreateService;
        public UserRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

    }
}

