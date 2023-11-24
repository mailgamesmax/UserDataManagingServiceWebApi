using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests
{
    public class LastNameChangingRequest
    {
        [NotNullOrWhiteSpace]
        public string LastName { get; set; }
    }
}
