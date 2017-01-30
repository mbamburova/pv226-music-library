using System.ComponentModel.DataAnnotations;

namespace BL.DTOs.Users
{
    public class UserRegistrationDTO
    {
        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(64)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, {Email}";
        }
    }
}