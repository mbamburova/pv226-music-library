using System.Collections.Generic;
using BL.DTOs.Filters;
using BL.DTOs.Playlists;
using BL.Services.Playlists;

namespace BL.Facades
{
    public class PlaylistFacade
    {

        #region Dependencies

        private readonly IPlaylistService _playlistService;

        public PlaylistFacade(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        #endregion
        public void CreateInitPlaylist(int userId)
        {
            _playlistService.CreateInitPlaylist(userId);
        }

        public void CreatePlaylist(PlaylistDTO playlistDto, int userId)
        {
            _playlistService.CreatePlaylist(playlistDto, userId);
        }

        public void EditPlaylist(PlaylistDTO playlistDto, int userId)
        {
            _playlistService.EditPlaylist(playlistDto, userId);
        }

        public void DeletePlaylist(int playlistId)
        {
            _playlistService.DeletePlaylist(playlistId);
        }

        public PlaylistDTO GetPlaylist(int playlistId)
        {
            return _playlistService.GetPlaylist(playlistId);
        }

        public List<PlaylistDTO> ListPlaylists(PlaylistFilter filter)
        {
            return (List<PlaylistDTO>) _playlistService.ListPlaylists(filter);
        }

       
    }
}