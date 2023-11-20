using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserDataManagingService.Models
{
    public class LivingPlace
    {
        [Key]
        public Guid LivingPlace_Id { get; set; } = Guid.NewGuid();

        [Column("Miestas")]
        [MaxLength(250)]
        public string City { get; set; }
        [Required]

        [Column("Gatve")]
        [MaxLength(250)]
        public string Street { get; set; } = string.Empty;

        [Column("Namo nr")]
        public string BuildingNr { get; set; } = string.Empty;
        [MaxLength(15)]

        [Column("Buto nr")]
        public string ApartmentNr { get; set; } = string.Empty;
        [MaxLength(15)]

        [ForeignKey("UserID")]
        public Guid UserId { get; set; }
        public User User { get; set; }        
    }
}

