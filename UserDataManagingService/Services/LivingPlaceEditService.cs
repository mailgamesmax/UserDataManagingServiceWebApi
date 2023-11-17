using UserDataManagingService.Models;
using UserDataManagingService.Models.Repositories;

namespace UserDataManagingService.Services
{
    public class LivingPlaceEditService : ILivingPlaceEditService
    {
        public async Task<LivingPlace> EditLivingPlaceData(string nickName, string city, string street, string buildingNr, string apartmentNr)
        {
            var selectUserId = await _userRepository.GetUserIdByNickname(nickName);
            var selectLivingPlace = await _placeRepository.GetLivingPlaceDataByUserID(selectUserId);
            if(selectLivingPlace != null) 
            {
                selectLivingPlace = await _placeRepository.ChangeLivingPlaceData(selectLivingPlace.LivingPlace_Id, city, street, buildingNr, apartmentNr);                
            }
            else
            {
                selectLivingPlace = _placeRepository.CreateLivingPlaceData(selectUserId, city, street, buildingNr, apartmentNr);
                _appDbContext.LivingPlaces.Add(selectLivingPlace);
            }
                await _appDbContext.SaveChangesAsync();
                return selectLivingPlace;
        }

        //
        private readonly AppDbContext _appDbContext;
        private readonly IUserRepository _userRepository;
        private readonly ILivingPlaceRepository _placeRepository;

        public LivingPlaceEditService(AppDbContext appDbContext, IUserRepository userRepository, ILivingPlaceRepository livingPlaceRepository)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
            _placeRepository = livingPlaceRepository;
        }
    }
}
