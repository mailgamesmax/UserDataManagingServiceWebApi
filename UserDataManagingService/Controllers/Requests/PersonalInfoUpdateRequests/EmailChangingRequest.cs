using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests
{
    public class EmailChangingRequest
    {
        [NotNullOrWhiteSpace]
        public string Email { get; set; }
    }
}
