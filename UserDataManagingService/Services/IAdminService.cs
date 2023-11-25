using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public interface IAdminService
    {
        Task<User> AutentificateAdminUser();
    }
}
