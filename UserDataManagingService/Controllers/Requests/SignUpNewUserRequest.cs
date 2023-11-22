using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests
{
    public class SignUpNewUserRequest
    {
        [NotNullOrWhiteSpace]
        public string Username { get; set; }
        [NotNullOrWhiteSpace]
        public string UserLastName { get; set; }
        [NotNullOrWhiteSpace]
        public string NickName { get; set; }
        [NotNullOrWhiteSpace]
        public string Password { get; set; }
        [NotNullOrWhiteSpace]
        public string PersonalCode { get; set; }
        [NotNullOrWhiteSpace]
        public string PhoneNr { get; set; }
        [NotNullOrWhiteSpace]
        public string Email { get; set; }
    }
}
