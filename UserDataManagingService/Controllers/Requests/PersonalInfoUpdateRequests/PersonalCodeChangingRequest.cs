using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests
{
    public class PersonalCodeChangingRequest
    {
        [NotNullOrWhiteSpace]
        public string PersonalCode { get; set; }
    }
}
