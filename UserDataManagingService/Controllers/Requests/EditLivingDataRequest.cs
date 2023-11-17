namespace UserDataManagingService.Controllers.Requests
{
    public class EditLivingDataRequest
    {
        public string NickName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNr { get; set; }
        public string ApartmentNr { get; set; }
    }
}
