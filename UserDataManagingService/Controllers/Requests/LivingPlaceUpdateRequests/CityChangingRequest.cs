using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests
{
    public class CityChangingRequest
    {
        [NotNullOrWhiteSpace]
        public string City { get; set; }
    }
}
