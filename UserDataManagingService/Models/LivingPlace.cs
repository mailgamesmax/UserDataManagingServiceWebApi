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
        [Required]
        public string City { get; set; }
        [MaxLength(250)]
        [Required]

        [Column("Gatve")]
        public string Street { get; set; } = string.Empty;
        [MaxLength(250)]
        [Required]

        [Column("Namo nr")]
        public string BuildingNr { get; set; } = string.Empty;
        [MaxLength(15)]
        [Required]

        [Column("Buto nr")]
        public string ApartmentNr { get; set; } = string.Empty;
        [MaxLength(15)]

        [ForeignKey("UserID")]
        public Guid UserId { get; set; }
        public User User { get; set; }        
    }
}

