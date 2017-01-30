using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTOs.Songs;

namespace PL.Models
{
    public class SongCreateModel
    {
        [Display(Name = "Interpret's name")]
        public string InterpretId { get; set; }

        public IEnumerable<SelectListItem> Interprets { get; set; }

        public SongDTO Song { get; set; }

        public string AlbumId { get; set; }

        public IEnumerable<SelectListItem> Albums { get; set; }
    }
}