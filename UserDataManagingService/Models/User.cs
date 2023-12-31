﻿using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Name { get; set; } //= string.Empty;        
        [MaxLength(150)]
        [Required]
        public string LastName { get; set; } //= string.Empty;        
        [MaxLength(150)]
        [Required]
        public string PersonalCode { get; set; } = string.Empty;
        [MaxLength(12)]
        [Required]

        public string PhoneNr { get; set; } = string.Empty;
        [MaxLength(100)]
        [Required]
        public string Email { get; set; } = string.Empty;
        [MaxLength(350)]
        [Required]

        public bool UserIsActive {  get; set; }

        //+ for JWT
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public Role Role { get; set; }

        [ForeignKey("UserID")]
        public Guid LivingPlaceId { get; set; }
        
        [JsonIgnore]
        public LivingPlace LivingPlace{ get; set; }

        public Guid AvatarId { get; set; }

        [JsonIgnore]
        public Avatar Avatar { get; set; }

    }

}
