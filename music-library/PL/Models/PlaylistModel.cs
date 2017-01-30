using System.Collections.Generic;
using BL.DTOs.Albums;
using BL.DTOs.Playlists;
using BL.DTOs.Songs;

namespace PL.Models
{
    public class PlaylistModel
    {
        public PlaylistDTO Playlist { get; set; }

        public IList<SongDTO> Songs { get; set; }

        public List<AlbumDTO> Albums { get; set; }
    }
}