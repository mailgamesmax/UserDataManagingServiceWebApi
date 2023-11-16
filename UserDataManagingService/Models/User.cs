using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace UserDataManagingService.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string NickName { get; set; }
        public string Name { get; set; } //= string.Empty;        
        public string LastName { get; set; } //= string.Empty;        
        public string PersonalCode { get; set; } = string.Empty;        
        public string PhoneNr { get; set; } = string.Empty;        
        public string Email { get; set; } = string.Empty;                            


        //+ for JWT
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public Role Role { get; set; }

        //[InverseProperty("User")]
       // [JsonIgnore]
        //public ICollection<NoteCategory> UserNoteCategories { get; set; } = new List<NoteCategory>();

    }

}
