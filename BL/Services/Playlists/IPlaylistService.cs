using System.Collections.Generic;
using BL.DTOs.Filters;
using BL.DTOs.Playlists;

namespace BL.Services.Playlists
{
    public interface IPlaylistService
    {
        void CreateInitPlaylist(int userId);

        void CreatePlaylist(PlaylistDTO playlistDto, int userId);

        void EditPlaylist(PlaylistDTO playlistDto, int userId);

        void DeletePlaylist(int playlistId);

        PlaylistDTO GetPlaylist(int playlistId);

        IEnumerable<PlaylistDTO> ListPlaylists(PlaylistFilter filter);
    }
}