using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests
{
    public class LoginRequest
    {
        [NotNullOrWhiteSpace]
        public string NickName { get; set; }
        [NotNullOrWhiteSpace]
        public string Password { get; set; }
    }
}
