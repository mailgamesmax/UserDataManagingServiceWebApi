using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public interface IAvatarCRUDService
    {
        Task <Avatar> CreateOrUpdateAvatar(Guid userId, IFormFile image);
        Task<byte[]> ResizeAndConvertImageToBytes(IFormFile primaryImage);
        Guid ConvertStringToGuid(string anyString);
        void AutoCycleFixer_UserAvatar(Avatar targetObject);
    }
}
