using System.ComponentModel.DataAnnotations;

namespace BL.DTOs.Reviews
{
    public abstract class ReviewDTO
    {
        [Display(Name = "Your name")]
        [MaxLength(65536)]
        public string Name { get; set; }

        public int ID { get; set; }

        [MaxLength(65536)]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [Required]
        [Range(0.0, 10.0)]
        public int Rating { get; set; }
    }
}