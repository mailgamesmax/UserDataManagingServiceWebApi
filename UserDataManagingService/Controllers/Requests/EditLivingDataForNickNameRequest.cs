using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests
{
    public class EditLivingDataForNickNameRequest
    {
        [NotNullOrWhiteSpace]
        public string NickName { get; set; }
        [NotNullOrWhiteSpace]
        public string City { get; set; }
        [NotNullOrWhiteSpace]
        public string Street { get; set; }
        [NotNullOrWhiteSpace]
        public string BuildingNr { get; set; }
        public string ApartmentNr { get; set; }
    }
}
