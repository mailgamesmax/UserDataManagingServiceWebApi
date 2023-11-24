using System.Text.Json.Serialization;
using System.Text.Json;
using UserDataManagingService.Models;
using UserDataManagingService.Models.Repositories;
/*using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats;*/
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
//using Size = SixLabors.ImageSharp.Size;

namespace UserDataManagingService.Services
{
    public class AvatarCRUDService : IAvatarCRUDService
    {
        public async Task<Avatar> CreateOrUpdateAvatar(Guid userId, IFormFile image)
        {       
            var imageInBytes = await ResizeAndConvertImageToBytes(image);
            var avatarToUpdate = await _avatarRepository.GetAvatarByUserID(userId);
            if(avatarToUpdate == null)
            {
                var newAvatar = await _avatarRepository.CreateAvatar(image.FileName, imageInBytes);
                _appDbContext.Avatars.Add(newAvatar);                
                
                var userToUpdate = await _userRepository.AvatarIdMappingToUserId(userId, newAvatar);
                await _appDbContext.SaveChangesAsync();
                return newAvatar;
            }
            else
            {
                var newAvatar = await _avatarRepository.CreateAvatar(image.FileName, imageInBytes);
                avatarToUpdate = _avatarRepository.UpdateCurrentAvatar(avatarToUpdate, newAvatar);
                var userToUpdate = await _userRepository.AvatarIdMappingToUserId(userId, avatarToUpdate);
                await _appDbContext.SaveChangesAsync();
                return avatarToUpdate;
            }
        }

        public async Task<byte[]> ResizeAndConvertImageToBytes(IFormFile primaryImage)
        {
            {
                int targetSideSize = 200;

                using (var memoryStream = new MemoryStream())
                {
                    primaryImage.CopyTo(memoryStream);
                    using (var originalImage = Image.FromStream(memoryStream))
                    {                       

                        int newWidth, newHeight;
                        if (originalImage.Width > originalImage.Height)
                        {
                            newWidth = targetSideSize;
                            newHeight = (int)((float)originalImage.Height / originalImage.Width * targetSideSize);
                        }
                        else
                        {
                            newWidth = (int)((float)originalImage.Width / originalImage.Height * targetSideSize);
                            newHeight = targetSideSize;
                        }

                        using (var resizedImage = new Bitmap(newWidth, newHeight))
                        using (var g = Graphics.FromImage(resizedImage))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.DrawImage(originalImage, 0, 0, newWidth, newHeight);

                            using (var resultStream = new MemoryStream())
                            {
                                resizedImage.Save(resultStream, originalImage.RawFormat);
                                return resultStream.ToArray();
                            } 
                        } 
                    }
                }
            }
        }

        public Guid ConvertStringToGuid(string anyString)
        {
            Guid guidFromString;
            if (Guid.TryParse(anyString, out guidFromString))
            {
                return guidFromString;
            }
            return Guid.Empty;
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
