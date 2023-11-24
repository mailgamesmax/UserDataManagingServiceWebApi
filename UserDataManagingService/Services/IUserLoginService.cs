using UserDataManagingService.Models.DTOs;
using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public interface IUserLoginService
    {
        Task<UserStatusDTO> UserLogin(string inputedNickName, string password);
        Task<(bool, User)> SignupNewUser(string newName, string newLastName, string newNickName, string password, string personalCode, string phoneNr, string email); //2
        Task<(bool, string)> CompleteUserCreating(Guid userId);
        Guid ConvertStringToGuid(string anyString);
    }


}
