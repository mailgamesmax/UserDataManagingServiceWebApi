using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests
{
    public class StreetChangingRequest
    {
        [NotNullOrWhiteSpace]
        public string Street { get; set; }
    }
}
