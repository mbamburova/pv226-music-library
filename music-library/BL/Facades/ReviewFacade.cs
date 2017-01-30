using System.Collections.Generic;
using BL.DTOs.Filters;
using BL.DTOs.Reviews;
using BL.Services.AlbumReviews;
using BL.Services.SongReviews;

namespace BL.Facades
{
    public class ReviewFacade
    {
        private readonly IAlbumReviewService _albumReviewService;
        private readonly ISongReviewService _songReviewService;

        public ReviewFacade(ISongReviewService songReviewService, IAlbumReviewService albumReviewService)
        {
            _songReviewService = songReviewService;
            _albumReviewService = albumReviewService;
        }

        public void CreateAlbumReview(AlbumReviewDTO albumReviewDto, int albumId)
        {
            _albumReviewService.CreateAlbumReview(albumReviewDto, albumId);
        }

        public void EditAlbumReview(AlbumReviewDTO albumReviewDto, int albumId)
        {
            _albumReviewService.EditAlbumReview(albumReviewDto, albumId);
        }

        public void DeleteAlbumReview(int albumReviewId)
        {
            _albumReviewService.DeleteAlbumReview(albumReviewId);
        }

        public AlbumReviewDTO GetAlbumReview(int albumReviewId)
        {
            return _albumReviewService.GetAlbumReview(albumReviewId);
        }

        public IEnumerable<AlbumReviewDTO> ListAlbumReviews(AlbumReviewFilter filter)
        {
            return _albumReviewService.ListAlbumReviews(filter);
        }

        public void CreateSongReview(SongReviewDTO songReviewDto, int songId)
        {
            _songReviewService.CreateSongReview(songReviewDto, songId);
        }

        public void EditSongReview(SongReviewDTO songReviewDto, int songId)
        {
            _songReviewService.EditSongReview(songReviewDto, songId);
        }

        public void DeleteSongReview(int songReviewId)
        {
            _songReviewService.DeleteSongReview(songReviewId);
        }

        public SongReviewDTO GetSongReview(int songReview)
        {
            return _songReviewService.GetSongReview(songReview);
        }

        public IEnumerable<SongReviewDTO> ListSongReviews(SongReviewFilter filter)
        {
            return _songReviewService.ListSongReviews(filter);
        }
    }
}