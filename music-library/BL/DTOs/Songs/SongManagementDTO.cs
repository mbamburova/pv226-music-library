using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace BL.DTOs.Songs
{
    public class SongManagementDTO
    {
        public int ID { get; set; }

        public string Album { get; set; }

        public string Interpret { get; set; }

        public string Name { get; set; }

        public Genre Genre { get; set; }

        [Display(Name = "Youtube link")]
        public string YTLink { get; set; }

        public int OriginalSong { get; set; }

        public string SongPath { get; set; }
    }
}