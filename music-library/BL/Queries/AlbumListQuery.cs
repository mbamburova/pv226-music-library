using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class AlbumListQuery : AppQuery<AlbumDTO>
    {
        public AlbumListQuery(IUnitOfWorkProvider provider) : base(provider) {}

        public AlbumFilter Filter { get; set; }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            IQueryable<Album> query = Context.Albums;

            if (!string.IsNullOrEmpty(Filter?.Name))
            {
                query = query.Where(song => song.Name.ToLower().Contains(Filter.Name.ToLower()));
            }

            if (Filter?.InterpretId >= 0)
            {
                query = query.Where(song => song.Interpret.ID == Filter.InterpretId);
            }
            if (Filter?.Year >= 0)
            {
                query = query.Where(song => song.Year == Filter.Year);
            }
            return query.ProjectTo<AlbumDTO>();
        }
    }
}