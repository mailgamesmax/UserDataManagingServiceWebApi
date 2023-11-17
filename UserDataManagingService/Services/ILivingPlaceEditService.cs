using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public interface ILivingPlaceEditService
    {
        Task<LivingPlace> EditLivingPlaceData(string nickName, string city, string street, string buildingNr, string apartmentNr);
        //Task<LivingPlace> GetLivingPlaceDataByUserID(Guid userId);
    }
}
