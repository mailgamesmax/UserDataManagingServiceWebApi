using System.Security.Cryptography;
using UserDataManagingService.Models.DTOs;
using UserDataManagingService.Models.Repositories;
using UserDataManagingService.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace UserDataManagingService.Services
{
    public class UserLoginService : IUserLoginService
    {
        public async Task<UserStatusDTO> UserLogin(string inputedNickName, string password) //1
        {
            var existedUser = await _userRepository.GetFullUserByNickname(inputedNickName);
            if (existedUser == null)
            {
                return new UserStatusDTO(false);
            }            
            if(existedUser.UserIsActive == false)
            {
                return new UserStatusDTO(false);
            }

            var isUserExist = VerifyPasswordHash(password, existedUser.PasswordHash, existedUser.PasswordSalt);
            return new UserStatusDTO(isUserExist, existedUser.Role);
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computeHash.SequenceEqual(computeHash);
        }

/*        public Token GenerateToken(string nick, Role role)
        {
            var token = _jwtService.GetJwtToken(request.NickName, (Role) response.Role)
                return token;
        }
*/

        public async Task<(bool, User)> SignupNewUser(string newName, string newLastName, string newNickName, string password, string personalCode, string phoneNr, string email) //2
        {

            var nickNameExistAlready = await _userRepository.GetFullUserByNickname(newNickName);
            if (nickNameExistAlready != null)
            {
                return (true, nickNameExistAlready);
            }

            var acc = _userRepository.CreateUser(newName, newLastName, newNickName, password, personalCode, phoneNr, email);          
            _appDbContext.Users.Add(acc);
            await _appDbContext.SaveChangesAsync();
            return (false, acc);
        }

        public async Task<(bool, string)> CompleteUserCreating(Guid userId)
        {
            var targetUser = await _userRepository.GetFullUserById(userId);
            if (targetUser == null) 
            {
                return (false, "nerastas user pagal nurodyta id");
            }
            if(await _userRepository.UserDataAreNotNullOrWihteSpaceAndMapped(targetUser))
            {
                targetUser.UserIsActive = true;
                _appDbContext.SaveChangesAsync();
                return (true, "patikrinimas ok, vartotojas sukurtas ir issaugotas");
            }
            await _userRepository.DeleteUserAsync(userId);
            return (false, "Klaida registruojant user registracijos duomenis. " +
                "\nIšsaugoti duomenys ištrinti, vartotojas - nesukurtas");

        }

        public async Task<UserDTO> CreateUserDTO(User user)
        {
            var userDTO = CreateUserDTOPersonalInfo(user);

            var userLivingPlace = await _livingPlaceRepository.GetLivingPlaceDataByUserID(user.UserId);
            if (userLivingPlace == null)
            {
                userDTO.LivingPlace = null;
            }
            else
            {
               var userDTOLivingPlace = CreateUserDTOLivingPlace(userLivingPlace);
               userDTO.LivingPlace = userDTOLivingPlace;
            }

            var userAvatar = await _avatarRepository.GetAvatarByUserID(user.UserId);
            if (userAvatar == null)
            {
                userDTO.Avatar = null;
            }
            else
            {
                var avatarDTO = CreateUserDTOAvatar(userAvatar);
                userDTO.Avatar = avatarDTO;
            }

            return userDTO;
        }
        public UserDTO CreateUserDTOPersonalInfo(User user)//, string nickName)//, string name, string lastName, string personalCode, string phoneNr, string email) 
        {
            var userDTO = new UserDTO
            {
                UserId = user.UserId,
                NickName = user.NickName,
                Name = user.Name,
                LastName = user.LastName,
                PersonalCode = user.PersonalCode,
                PhoneNr = user.PhoneNr,
                Email = user.Email,
                UserIsActive = user.UserIsActive,
                Role = user.Role
            };

            return userDTO;
        }

        public LivingPlace CreateUserDTOLivingPlace(LivingPlace livingPlace)
        {
            var placeDTO = new LivingPlace
            {
                City = livingPlace.City,
                Street = livingPlace.Street,
                BuildingNr = livingPlace.BuildingNr,
                ApartmentNr = livingPlace.ApartmentNr,
                LivingPlace_Id = livingPlace.LivingPlace_Id,
                UserId = livingPlace.UserId
            };            

            return placeDTO;
        }

        public Avatar CreateUserDTOAvatar(Avatar avatar)
        {
            var avatarDTO = new Avatar
            {
                Avatar_Id = avatar.Avatar_Id,
                AvatarBytes = avatar.AvatarBytes,
                UserId = avatar.UserId,
                Title = avatar.Title,
            };

            return avatarDTO;
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
        private readonly IUserRepository _userRepository;
        private readonly ILivingPlaceRepository _livingPlaceRepository;
        private readonly IAvatarRepository _avatarRepository;
        public UserLoginService(AppDbContext appDbContext, IUserRepository userRepository, ILivingPlaceRepository livingPlaceRepository, IAvatarRepository avatarRepository)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
            _livingPlaceRepository = livingPlaceRepository;
            _avatarRepository = avatarRepository;
        }
    }
}
