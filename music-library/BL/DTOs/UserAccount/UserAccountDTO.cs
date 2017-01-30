using System;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs.Users
{
    public class UserAccountDTO
    {
        public Guid ID { get; set; }

        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(64)]
        public string LastName { get; set; }


        [Required]
        [EmailAddress]
        [MaxLength(128)]
        public string Email { get; set; }
    }
}