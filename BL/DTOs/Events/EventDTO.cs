using System;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs.Events
{
    public class EventDTO
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Name { get; set; }

        [Display(Name = "Interpret")]
        public int InterpretId { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Location")]
        public string Place { get; set; }

        [MaxLength(1024)]
        [Display(Name = "Link")]
        public string EventLink { get; set; }
    }
}