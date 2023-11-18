using Microsoft.EntityFrameworkCore;

namespace UserDataManagingService.Models.Repositories
{
    public class LivingPlaceRepository : ILivingPlaceRepository
    {
        //public LivingPlace CreateLivingPlaceData(Guid userId, string city, string street, string buildingNr, string apartmentNr)
        public LivingPlace CreateLivingPlaceData(string city, string street, string buildingNr, string apartmentNr)
        {
            var createdLivingPlace = new LivingPlace
            {
                City = city,
                Street = street,
                BuildingNr = buildingNr,
                ApartmentNr = apartmentNr,
            };
            return createdLivingPlace;
        }

        public async Task<LivingPlace> ChangeLivingPlaceData(Guid targetPlaceId, string city, string street, string buildingNr, string apartmentNr)
        {
            var targetLivingPlace = await _appDbContext.LivingPlaces.FirstOrDefaultAsync(p => p.LivingPlace_Id == targetPlaceId);
            if (!string.IsNullOrEmpty(city))
            {
                targetLivingPlace.City = city;
            }

            if (!string.IsNullOrEmpty(street))
            {
                targetLivingPlace.Street = street;
            }

            if (!string.IsNullOrEmpty(buildingNr))
            {
                targetLivingPlace.BuildingNr = buildingNr;
            }

            if (!string.IsNullOrEmpty(apartmentNr))
            {
                targetLivingPlace.ApartmentNr = apartmentNr;
            }

            return targetLivingPlace;
        }

        public async Task<LivingPlace> GetLivingPlaceDataByUserID(Guid userId)
        {
            var userLivingPlace = await _appDbContext.LivingPlaces.FirstOrDefaultAsync(x => x.UserId == userId);
            return userLivingPlace;
        }

        //
        private readonly AppDbContext _appDbContext;
        public LivingPlaceRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

    }
}
