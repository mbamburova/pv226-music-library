using System.Collections.Generic;
using BL.DTOs.Events;

namespace PL.Models
{
    public class EventsViewModel
    {
        public IList<EventDisplayDTO> Events { get; set; }
    }
}