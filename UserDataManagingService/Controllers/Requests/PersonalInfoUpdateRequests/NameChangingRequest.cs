using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests
{
    public class NameChangingRequest
    {
        [NotNullOrWhiteSpace]
        public string Name { get; set; }
    }
}
