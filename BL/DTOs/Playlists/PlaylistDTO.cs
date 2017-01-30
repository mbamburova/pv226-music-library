using System;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs.Playlists
{
    public class PlaylistDTO
    {
        public int ID { get; set; }

        [RegularExpression("[^All my songs]", ErrorMessage = "Cannot create playlist with name 'All my songs'")]
        [Required(ErrorMessage = "Playlist name can't be empty.")]
        public string Name { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public int UserId { get; set; }
    }
}