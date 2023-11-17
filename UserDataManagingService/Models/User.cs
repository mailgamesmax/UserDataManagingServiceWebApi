using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;

namespace UserDataManagingService.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string NickName { get; set; }
        [MaxLength(150)]
        public string Name { get; set; } //= string.Empty;        
        [MaxLength(150)]
        public string LastName { get; set; } //= string.Empty;        
        [MaxLength(150)]
        public string PersonalCode { get; set; } = string.Empty;        

        public string PhoneNr { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(350)]


        //+ for JWT
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public Role Role { get; set; }

        [ForeignKey("UserID")]
        public Guid LivingPlaceId { get; set; }
        public LivingPlace LivingPlace{ get; set; }
        
        //[InverseProperty("User")]
        //[JsonIgnore]
        //public ICollection<LivingPlace> UserLivingPlaces { get; set; } = new List<NoteCategory>();

    }

}
