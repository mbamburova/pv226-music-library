using System.Collections.Generic;
using BL.DTOs.Albums;
using BL.DTOs.Events;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;

namespace PL.Models
{
    public class InterpretListViewModel
    {
        public InterpretDTO Interpret { get; set; }

        public InterpretFilter InterpretFilter { get; set; }

        public IList<InterpretDTO> Interprets { get; set; }

        public IList<EventDTO> Events { get; set; }

        public IList<AlbumDTO> Albums { get; set; }
    }
}