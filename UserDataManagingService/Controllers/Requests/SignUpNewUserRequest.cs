namespace UserDataManagingService.Controllers.Requests
{
    public class SignUpNewUserRequest
    {
        public string Username { get; set; }
        public string UserLastName { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string PersonalCode { get; set; }
        public string PhoneNr { get; set; }
        public string Email { get; set; }
    }
}
