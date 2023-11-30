using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserDataManagingService.Models.DTOs
{
    public class UserDTO
    {
        //personal Info
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string NickName { get; set; }
        public string Name { get; set; } 
        public string LastName { get; set; }
        public string PersonalCode { get; set; }

        public string PhoneNr { get; set; }
        public string Email { get; set; }

        public bool UserIsActive { get; set; }

        public Role Role { get; set; }

        public LivingPlace? LivingPlace { get; set; }

        public Avatar? Avatar { get; set; }

        public UserDTO()
        {

        }
    }
}
