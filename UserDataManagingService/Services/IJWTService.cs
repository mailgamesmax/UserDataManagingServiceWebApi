using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public interface IJWTService
    {
        string GetJwtToken(string userName, Role role);
    }
}
