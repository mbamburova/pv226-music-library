using System.Collections.Generic;
using BL.DTOs.Albums;
using BL.DTOs.Interprets;

namespace PL.Models
{
    public class AlbumListViewModel
    {
        public IList<AlbumDTO> Albums { get; set; }

        public IList<InterpretDTO> Interprets { get; set; }
    }
}