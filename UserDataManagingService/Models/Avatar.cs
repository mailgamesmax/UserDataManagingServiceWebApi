using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserDataManagingService.Models
{
    public class Avatar
    {
        [Key]
        public Guid Avatar_Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public byte[] AvatarBytes { get; set; }

        public Avatar(string title, byte[] bytes)
        {
            Title = title;
            AvatarBytes = bytes;
        }

        public Avatar() { }

        [ForeignKey("UserID")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
