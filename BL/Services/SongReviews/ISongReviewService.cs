using System.Collections.Generic;
using BL.DTOs.Filters;
using BL.DTOs.Reviews;

namespace BL.Services.SongReviews
{
    public interface ISongReviewService
    {
        void CreateSongReview(SongReviewDTO somgReviewDto, int songId);

        void EditSongReview(SongReviewDTO songReviewDto, int songId);

        void DeleteSongReview(int songReviewId);

        SongReviewDTO GetSongReview(int songReviewId);

        IEnumerable<SongReviewDTO> ListSongReviews(SongReviewFilter filter);
    }
}