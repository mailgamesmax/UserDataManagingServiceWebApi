using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests
{
    public class ApartmentChangingRequest
    {
        [NotNullOrWhiteSpace]
        public string ApartmentNr { get; set; }
    }
}
