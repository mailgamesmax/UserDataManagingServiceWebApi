namespace UserDataManagingService.Models.Repositories
{
    public interface ILivingPlaceRepository
    {
        LivingPlace CreateLivingPlaceData(Guid userId, string city, string street, string buildingNr, string apartmentNr);
        Task<LivingPlace> ChangeLivingPlaceData(Guid targetPlaceId, string city, string street, string buildingNr, string apartmentNr);
        Task<LivingPlace> GetLivingPlaceDataByUserID(Guid userId);
    }
}
