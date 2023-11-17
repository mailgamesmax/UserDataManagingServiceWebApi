using UserDataManagingService.Models.DTOs;
using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public interface IUserLoginAndCreateService
    {
        UserStatusDTO UserLogin(string newNickName, string password); //1
        Task<(bool, User)> SignupNewUser(string newName, string newLastName, string newNickName, string password, string personalCode, string phoneNr, string email); //2
//        User SignupNewUser(string userName, string newNickName, string password); //2

    }


}
