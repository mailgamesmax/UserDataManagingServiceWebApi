using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests
{
    public class PhoneNrChangingRequest
    {
        [NotNullOrWhiteSpace]
        public string PhoneNr { get; set; }
    }
}
