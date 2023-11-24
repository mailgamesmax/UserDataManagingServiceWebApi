using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public interface ILivingPlaceEditService
    {
        Task<LivingPlace> CreateLivingPlaceDataByNickName(string nickName, string city, string street, string buildingNr, string apartmentNr);
        //Task<LivingPlace> GetLivingPlaceDataByUserID(Guid userId);

        Task<LivingPlace> CreateLivingPlaceDataByUserId(Guid selectUserId, string city, string street, string buildingNr, string apartmentNr);
        Task<bool> LivingDataChange(Guid userId, string propertyTitl, string newValute);
        Guid ConvertStringToGuid(string anyString);
        void AutoCycleFixer_UserLivingPlace(LivingPlace targetObject);        

    }
}
