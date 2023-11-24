using Azure.Core;
using UserDataManagingService.Models.Repositories;

namespace UserDataManagingService.Services
{
    public class PersonalInfoUpdateService : IPersonalInfoUpdateService
    {
        public async Task<bool> NameChange(Guid userId, string newName)
        {
            var targetUser = await _userRepository.GetFullUserById(userId);
            if(targetUser == null)
            {
                return false;
            }            
            targetUser.Name = newName;
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailChange(Guid userId, string newEmail)
        {
            var targetUser = await _userRepository.GetFullUserById(userId);
            if (targetUser == null)
            {
                return false;
            }
            targetUser.Email = newEmail;
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LastNameChange(Guid userId, string newLastName)
        {
            var targetUser = await _userRepository.GetFullUserById(userId);
            if (targetUser == null)
            {
                return false;
            }
            targetUser.LastName = newLastName;
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PasswordChange(Guid userId, string oldPassword, string newPassword)
        {

            var targetUser = await _userRepository.GetFullUserById(userId);
            if (targetUser == null)
            {
                return false;
            }

            var oldPasswordVerified = await _userLoginService.UserLogin(targetUser.NickName, oldPassword);
            if (oldPasswordVerified.IsUserExist)
            {
                var newPasswordHashAndSalt = _userRepository.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
                targetUser.PasswordHash = newPasswordHashAndSalt.Item1;
                targetUser.PasswordSalt = newPasswordHashAndSalt.Item2;
            }
            
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PersonalCodeChange(Guid userId, string newCode)
        {
            var targetUser = await _userRepository.GetFullUserById(userId);
            if (targetUser == null)
            {
                return false;
            }
            targetUser.PersonalCode = newCode;
            await _appDbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> PhoneNrChange(Guid userId, string newPhoneNr)
        {
            var targetUser = await _userRepository.GetFullUserById(userId);
            if (targetUser == null)
            {
                return false;
            }
            targetUser.PhoneNr = newPhoneNr;
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        //
        private readonly AppDbContext _appDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginService _userLoginService;

        public PersonalInfoUpdateService(AppDbContext appDbContext, IUserRepository userRepository, IUserLoginService userLoginService)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
            _userLoginService = userLoginService;
        }
    }
}

