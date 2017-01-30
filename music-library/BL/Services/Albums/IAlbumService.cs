using System.Collections.Generic;
using BL.DTOs.Albums;
using BL.DTOs.Filters;

namespace BL.Services.Albums
{
    public interface IAlbumService
    {
        void CreateAlbum(AlbumDTO albumDto, int interpretId);

        void EditAlbum(AlbumDTO albumDto, int interpretId, int[] songIds, int[] albumReviewIds);

        void DeleteAlbum(int albumId);

        AlbumDTO GetAlbum(int albumId);

        IEnumerable<AlbumDTO> ListAlbums(AlbumFilter filter);

        void MakeAlbumPublic(AlbumDTO albumDto, int interpretId);
    }
}