using System.Collections.Generic;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.DTOs.Playlists;
using BL.DTOs.Songs;
using BL.DTOs.User;

namespace PL.Models
{
    public class CollectionModel
    {
        public PlaylistDTO InitPlaylist { get; set; }

        public List<PlaylistDTO> Playlists { get; set; }

        public UserDTO User { get; set; }

        public List<SongDTO> Songs { get; set; }

        public List<AlbumDTO> Albums { get; set; }

        public List<InterpretDTO> Interprets { get; set; }

        public SongDTO Song { get; set; }

        public PlaylistDTO Playlist { get; set; }

        public SongFilter Filter { get; set; }

        public bool IsAdded { get; set; }
    }
}