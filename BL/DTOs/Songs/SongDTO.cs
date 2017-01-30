using System;
using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace BL.DTOs.Songs
{
    public class SongDTO
    {
        public int ID { get; set; }

        public int AlbumId { get; set; }

        public int CreatedBy { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public DateTime Added { get; set; }

        [MaxLength(1024)]
        [Display(Name = "Youtube link")]
        public string YTLink { get; set; }

        public bool IsPublic { get; set; }

        public bool Publish { get; set; }

        public int OriginalSongId { get; set; }

        [Display(Name = "Song path")]
        public string SongPath { get; set; }
    }
}