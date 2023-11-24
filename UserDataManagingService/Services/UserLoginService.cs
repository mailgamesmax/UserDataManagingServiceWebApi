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
        public UserLoginService(AppDbContext appDbContext, IUserRepository userRepository)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
        }
    }
}
