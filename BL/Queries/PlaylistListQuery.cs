using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Playlists;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class PlaylistListQuery : AppQuery<PlaylistDTO>
    {
        public PlaylistListQuery(IUnitOfWorkProvider provider) : base(provider) {}

        public PlaylistFilter Filter { get; set; }

        protected override IQueryable<PlaylistDTO> GetQueryable()
        {
            IQueryable<Playlist> query = Context.Playlists;

            if (!string.IsNullOrEmpty(Filter?.Name))
            {
                query = query.Where(song => song.Name.ToLower().Contains(Filter.Name.ToLower()));
            }

            if (Filter?.UserId >= 0)
            {
                query = query.Where(song => song.User.ID == Filter.UserId);
            }
            return query.ProjectTo<PlaylistDTO>();
        }
    }
}