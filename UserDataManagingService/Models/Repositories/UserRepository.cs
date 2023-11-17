using System.Security.Cryptography;
using System;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Guid> GetUserIdByNickname(string nickName)
        {
            var userId = await _appDbContext.Users
                .Where(a => a.NickName == nickName)
                .Select(a => a.UserId)
                .SingleOrDefaultAsync();

            return userId;
        }

        public async Task<User> GetFullUserByNickname(string nickName)
        {
            User user = await _appDbContext.Users.FirstOrDefaultAsync(n => n.NickName == nickName);
            return user;
        }



        //
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

    }
}

