using System.Security.Cryptography;
using UserDataManagingService.Models.DTOs;
using UserDataManagingService.Models.Repositories;
using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public class UserLoginAndCreateService : IUserLoginAndCreateService
    {
        public UserStatusDTO UserLogin(string inputedNickName, string password) //1
        {
            var clientAcc = _appDbContext.Users.SingleOrDefault(a => a.NickName == inputedNickName);
            if (clientAcc == null)
            {
                return new UserStatusDTO(false);
            }

            var isUserExist = VerifyPasswordHash(password, clientAcc.PasswordHash, clientAcc.PasswordSalt);
            return new UserStatusDTO(isUserExist, clientAcc.Role);
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computeHash.SequenceEqual(computeHash);
        }

        public async Task<(bool, User)> SignupNewUser(string userName, string newNickName, string password) //2
        {

            var nickNameExistAlready = await _userRepository.GetFullUserByNickname(userName);
            if (nickNameExistAlready != null)
            {
                return (false, null);
            }

            var acc = _userRepository.CreateUser(userName, newNickName, password);
            _appDbContext.Users.Add(acc);
            await _appDbContext.SaveChangesAsync();
            return (true, acc);

        }

        //
        private readonly AppDbContext _appDbContext;
        private readonly IUserRepository _userRepository;

        public UserLoginAndCreateService(AppDbContext appDbContext, IUserRepository userRepository)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
        }
    }
}
