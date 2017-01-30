using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace BL.DTOs.Interprets
{
    public class InterpretDTO
    {
        public int ID { get; set; }

        [Display(Name = "Interpret's name")]
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string InterpretImgUri { get; set; }

        [Required]
        public Language Language { get; set; }

        public bool IsPublic { get; set; }
    }
}