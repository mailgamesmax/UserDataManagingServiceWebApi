using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests
{
    public class UserToRemoveRequest
    {
        [NotNullOrWhiteSpace]
        public string UserId { get; set; }
    }
}
