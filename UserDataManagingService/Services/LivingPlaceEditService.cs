using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using UserDataManagingService.Models;
using UserDataManagingService.Models.Repositories;

namespace UserDataManagingService.Services
{
    public class LivingPlaceEditService : ILivingPlaceEditService
    {
        public async Task<LivingPlace> EditLivingPlaceDataByNickName(string nickName, string city, string street, string buildingNr, string apartmentNr)
        {
         //   var selectUserId = await _userRepository.GetUserIdByNickname(nickName);
            var selectUserId = await _userRepository.GetUserIdByNickname(nickName);
            if (selectUserId == Guid.Empty)
            {
                return null;
            }
            
            var livingPlaceToEdit = await EditLivingPlaceDataByUserId(selectUserId, city, street, buildingNr, apartmentNr);
                return livingPlaceToEdit;
        }

        public async Task<LivingPlace> EditLivingPlaceDataByUserId(Guid selectUserId, string city, string street, string buildingNr, string apartmentNr)
        {
            var selectedUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserId == selectUserId);
            var livingPlaceToEdit = await _placeRepository.GetLivingPlaceDataByUserID(selectUserId);
            if (livingPlaceToEdit != null)
            {
                livingPlaceToEdit = await _placeRepository.ChangeLivingPlaceData(livingPlaceToEdit.LivingPlace_Id, city, street, buildingNr, apartmentNr);
                selectedUser.LivingPlace = livingPlaceToEdit;
            }
            else
            {                
                livingPlaceToEdit = _placeRepository.CreateLivingPlaceData(city, street, buildingNr, apartmentNr);
                _appDbContext.LivingPlaces.Add(livingPlaceToEdit);
                
                selectedUser.LivingPlace = livingPlaceToEdit;
                selectedUser.LivingPlaceId = livingPlaceToEdit.LivingPlace_Id;
            }
            await _appDbContext.SaveChangesAsync();
            return livingPlaceToEdit;
        }

        public void AutoCycleFixer_UserLivingPlace(LivingPlace targetObject)
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
        //private readonly IUserLoginAndCreateService _userLoginAndCreateService;


        //public LivingPlaceEditService(AppDbContext appDbContext, IUserRepository userRepository, ILivingPlaceRepository livingPlaceRepository, IUserLoginAndCreateService userLoginAndCreateService)
        public LivingPlaceEditService(AppDbContext appDbContext, IUserRepository userRepository, ILivingPlaceRepository livingPlaceRepository)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
            _placeRepository = livingPlaceRepository;
          //  _userLoginAndCreateService = userLoginAndCreateService;
        }
    }
}
