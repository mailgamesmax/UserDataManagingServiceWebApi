using System.Text.Json.Serialization;
using System.Text.Json;
using UserDataManagingService.Models;
using UserDataManagingService.Models.Repositories;

namespace UserDataManagingService.Services
{
    public class AvatarCRUDService : IAvatarCRUDService
    {
        public async Task<Avatar> CreateOrUpdateAvatar(Guid userId, IFormFile image)        {       
            var avatarToUpdate = await _avatarRepository.GetAvatarByUserID(userId);
            if(avatarToUpdate == null)
            {
                var newAvatar = await _avatarRepository.ImageToBytesAndCreateAvatar(image.FileName, image);
                _appDbContext.Avatars.Add(newAvatar);                
                
                var userToUpdate = await _userRepository.AvatarIdMappingToUserId(userId, newAvatar);
                await _appDbContext.SaveChangesAsync();
                return newAvatar;
            }
            else
            {
                var newAvatar = await _avatarRepository.ImageToBytesAndCreateAvatar(image.FileName, image);
                avatarToUpdate = _avatarRepository.UpdateCurrentAvatar(avatarToUpdate, newAvatar);
                var userToUpdate = await _userRepository.AvatarIdMappingToUserId(userId, avatarToUpdate);
                await _appDbContext.SaveChangesAsync();
                return avatarToUpdate;
            }

        }

        public void AutoCycleFixer_UserAvatar(Avatar targetObject)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            string jsonString = JsonSerializer.Serialize(targetObject, options);
        }
        //
        private readonly AppDbContext _appDbContext;
        private readonly IUserRepository _userRepository;
        private readonly ILivingPlaceRepository _placeRepository;
        private readonly IAvatarRepository _avatarRepository;
        //private readonly IUserLoginAndCreateService _userLoginAndCreateService;


        //public LivingPlaceEditService(AppDbContext appDbContext, IUserRepository userRepository, ILivingPlaceRepository livingPlaceRepository, IUserLoginAndCreateService userLoginAndCreateService)
        public AvatarCRUDService(AppDbContext appDbContext, IUserRepository userRepository, ILivingPlaceRepository livingPlaceRepository, IAvatarRepository avatarRepository)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
            _placeRepository = livingPlaceRepository;
            _avatarRepository = avatarRepository;
            //  _userLoginAndCreateService = userLoginAndCreateService;
        }
    }
}
