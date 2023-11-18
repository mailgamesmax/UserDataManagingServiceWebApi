using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public interface ILivingPlaceEditService
    {
        Task<LivingPlace> EditLivingPlaceDataByNickName(string nickName, string city, string street, string buildingNr, string apartmentNr);
        //Task<LivingPlace> GetLivingPlaceDataByUserID(Guid userId);

        Task<LivingPlace> EditLivingPlaceDataByUserId(Guid selectUserId, string city, string street, string buildingNr, string apartmentNr);
        void AutoCycleFixer_UserLivingPlace(LivingPlace targetObject);        

    }
}
