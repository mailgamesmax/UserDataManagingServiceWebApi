using UserDataManagingService.Models.DTOs;
using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public interface IPersonalInfoUpdateService
    {
        Task<bool> NameChange(Guid userId, string newName);
        Task<bool> EmailChange(Guid userId, string newEmail);
        Task<bool> LastNameChange(Guid userId, string newLastName);
        Task<bool> PasswordChange(Guid userId, string oldPassword, string newPassword);
        Task<bool> PersonalCodeChange(Guid userId, string newEmail);
        Task<bool> PhoneNrChange(Guid userId, string newEmail);

    }
}
