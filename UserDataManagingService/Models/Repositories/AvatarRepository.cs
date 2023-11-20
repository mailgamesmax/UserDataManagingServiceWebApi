using Microsoft.EntityFrameworkCore;

namespace UserDataManagingService.Models.Repositories
{
    public class AvatarRepository : IAvatarRepository
    {
        public async Task<Avatar> GetAvatarByUserID(Guid userId)
        {
            var targetAvatar = await _appDbContext.Avatars.FirstOrDefaultAsync(a => a.UserId == userId);
            return targetAvatar;
        }

        public async Task<Avatar> ImageToBytesAndCreateAvatar(string title, IFormFile image) //? task
        {
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);

            var imageInBytes = memoryStream.ToArray();

            var createdAvatar = new Avatar(title, imageInBytes);
            return createdAvatar;        
        }

        public Avatar UpdateCurrentAvatar(Avatar oldAvatar, Avatar newAvatar) 
        {
            oldAvatar.Title = newAvatar.Title;
            oldAvatar.AvatarBytes = newAvatar.AvatarBytes;
            return oldAvatar;
        }


        //
        private readonly AppDbContext _appDbContext;
        public AvatarRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
