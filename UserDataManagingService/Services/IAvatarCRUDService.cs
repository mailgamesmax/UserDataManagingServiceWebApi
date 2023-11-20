using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public interface IAvatarCRUDService
    {
        //Task <Avatar> GetAvatarAsync (Avatar avatar);
        Task <Avatar> CreateOrUpdateAvatar(Guid userId, IFormFile image);
        void AutoCycleFixer_UserAvatar(Avatar targetObject);
    }
}
