using System.Collections.Generic;
using BL.DTOs.Filters;
using BL.DTOs.Reviews;

namespace BL.Services.AlbumReviews
{
    public interface IAlbumReviewService
    {
        void CreateAlbumReview(AlbumReviewDTO albumReviewDto, int albumId);

        void EditAlbumReview(AlbumReviewDTO albumReviewDto, int albumId);

        void DeleteAlbumReview(int albumReviewId);

        AlbumReviewDTO GetAlbumReview(int albumReviewId);

        IEnumerable<AlbumReviewDTO> ListAlbumReviews(AlbumReviewFilter filter);
    }
}