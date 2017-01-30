using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.DTOs.Reviews;
using BL.DTOs.Songs;

namespace PL.Models
{
    public class SongListViewModel
    {
        public IList<SongDTO> Songs { get; set; }

        public IList<AlbumDTO> Albums { get; set; }

        public IList<InterpretDTO> Interprets { get; set; }

        [Display(Name = "Artist")]
        public InterpretDTO Interpret { get; set; }

        [Display(Name = "Album")]
        public AlbumDTO Album { get; set; }

        [Display(Name = "Song")]
        public SongDTO Song { get; set; }

        public IEnumerable<SelectListItem> AlbumItems { get; set; }

        public IList<SongReviewDTO> SongReviews { get; set; }

        public IList<AlbumReviewDTO> AlbumReviews { get; set; }

        [Display(Name = "Review of song")]
        public SongReviewDTO SongReviewDto { get; set; }

        [Display(Name = "Review of album")]
        public AlbumReviewDTO AlbumReviewDto { get; set; }

        public List<SongDTO> UserSongs { get; set; }

        public SongFilter SongFilter { get; set; }
    }
}