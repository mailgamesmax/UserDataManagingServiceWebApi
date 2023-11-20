namespace UserDataManagingService.Models.Repositories
{
    public interface IAvatarRepository
    {
        Task<Avatar> ImageToBytesAndCreateAvatar(string title, IFormFile image);
        //Task<Avatar> UploadAndUpdateAvatar(string title, byte[] avatar);

        Avatar UpdateCurrentAvatar(Avatar oldAvatar, Avatar newAvatar);
        Task<Avatar> GetAvatarByUserID(Guid userId);
    }
}
