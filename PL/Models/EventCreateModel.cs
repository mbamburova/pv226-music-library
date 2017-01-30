using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTOs.Events;

namespace PL.Models
{
    public class EventCreateModel
    {
        public EventDTO EventDto { get; set; }

        [Display(Name = "Interpret")]
        public string InterpretId { get; set; }

        public IEnumerable<SelectListItem> Interprets { get; set; }
    }
}