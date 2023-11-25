using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using UserDataManagingService.Models;
using UserDataManagingService.Models.Repositories;

namespace UserDataManagingService.Services
{
    // this part is mostly created for app represent
    public class AdminService : IAdminService
    {

        public async Task<User> AutentificateAdminUser()
        {
            var adminData = await ExistedAdmin();
            if (adminData != null)
            {
                return adminData;
            }

            adminData = await CreateNewAdmin();
            _appDbContext.Users.Add(adminData);
            await _appDbContext.SaveChangesAsync();
            return adminData;
        }

        private async Task<User> ExistedAdmin()
        {
            var currentlyAdminData = await _appDbContext.Users.FirstOrDefaultAsync(u => u.NickName == "admin");
            if (currentlyAdminData == null) 
            {
                return null;
            }
            return currentlyAdminData;
        }

        private async Task<User> CreateNewAdmin()
        {

                CreateAdminPasswordHashAndSalt("admin", out byte[] passwordHash, out byte[] passwordSalt);
                var createdAcc = new User
                {
                    Name = "adminitrator",
                    LastName = "adminitrator",
                    NickName = "admin",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    PersonalCode = "admin code",
                    PhoneNr = "admin phone",
                    Email = "admin@email.com",

                    Role = Role.Admin,
                };

                return createdAcc;

        }

        private (byte[], byte[]) CreateAdminPasswordHashAndSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return (passwordHash, passwordSalt);
        }
    //
        private readonly AppDbContext _appDbContext;
        private readonly IUserRepository _userRepository;
        public AdminService(AppDbContext appDbContext, IUserRepository userRepository)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
        }
    }
}
