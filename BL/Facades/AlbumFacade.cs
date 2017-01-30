using System.Collections.Generic;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.Services.Albums;

namespace BL.Facades
{
    public class AlbumFacade
    {
        private readonly IAlbumService _albumService;

        public AlbumFacade(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        public void CreateAlbumWithInterpret(AlbumDTO album, int interpretId)
        {
            _albumService.CreateAlbum(album, interpretId);
        }

        public void EditAlbum(AlbumDTO album, int interpretId, int[] songIds, int[] albumReviewIds)
        {
            _albumService.EditAlbum(album, interpretId, songIds, albumReviewIds);
        }

        public void DeleteAlbum(int albumId)
        {
            _albumService.DeleteAlbum(albumId);
        }

        public AlbumDTO GetAlbum(int albumId)
        {
            return _albumService.GetAlbum(albumId);
        }

        public IEnumerable<AlbumDTO> ListAlbums(AlbumFilter filter)
        {
            return _albumService.ListAlbums(filter);
        }
    }
}