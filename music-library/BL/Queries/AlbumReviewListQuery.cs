using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Reviews;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class AlbumReviewListQuery : AppQuery<AlbumReviewDTO>
    {
        public AlbumReviewListQuery(IUnitOfWorkProvider provider) : base(provider) {}

        public AlbumReviewFilter Filter { get; set; }

        protected override IQueryable<AlbumReviewDTO> GetQueryable()
        {
            IQueryable<AlbumReview> query = Context.AlbumReviews;

            if (Filter?.AlbumId >= 0)
            {
                query = query.Where(albumReview => albumReview.Album.ID == Filter.AlbumId);
            }

            return query.ProjectTo<AlbumReviewDTO>();
        }
    }
}