using System;
using System.ComponentModel.DataAnnotations;
using BL.DTOs.Interprets;

namespace BL.DTOs.Events
{
    public class EventDisplayDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public InterpretDTO Interpret { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Time { get; set; }

        [Display(Name = "Location")]
        public string Place { get; set; }

        [Display(Name = "Link")]
        public string EventLink { get; set; }
    }
}