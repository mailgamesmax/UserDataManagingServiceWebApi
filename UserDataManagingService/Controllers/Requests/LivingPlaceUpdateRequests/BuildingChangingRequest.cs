using UserDataManagingService.Models.Atrributes;

namespace UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests
{
    public class BuildingChangingRequest
    {
        [NotNullOrWhiteSpace]
        public string BuildingNr { get; set; }
    }
}
