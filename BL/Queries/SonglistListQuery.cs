using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Songlists;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class SonglistListQuery : AppQuery<SongListDTO>
    {
        public SonglistListQuery(IUnitOfWorkProvider provider) : base(provider) {}

        public SongListFilter Filter { get; set; }

        protected override IQueryable<SongListDTO> GetQueryable()
        {
            IQueryable<SongList> query = Context.SongLists;

            if (Filter?.PlaylistId >= 0)
            {
                query = query.Where(songlist => songlist.Playlist.ID == Filter.PlaylistId);
            }
            return query.ProjectTo<SongListDTO>();
        }
    }
}