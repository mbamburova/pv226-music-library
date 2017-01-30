using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Reviews;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class SongReviewListQuery : AppQuery<SongReviewDTO>
    {
        public SongReviewListQuery(IUnitOfWorkProvider provider) : base(provider) {}

        public SongReviewFilter Filter { get; set; }

        protected override IQueryable<SongReviewDTO> GetQueryable()
        {
            IQueryable<SongReview> query = Context.SongReviews;

            if (Filter?.SongId >= 0)
            {
                query = query.Where(song => song.Song.ID == Filter.SongId);
            }
            return query.ProjectTo<SongReviewDTO>();
        }
    }
}