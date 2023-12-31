﻿using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests
{
    public class PasswordChangingRequest
    {
        [NotNullOrWhiteSpace]
        public string OldPassword { get; set; }
        [NotNullOrWhiteSpace]
        public string NewPassword { get; set; }
    }
}
