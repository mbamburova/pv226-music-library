using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Songs;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class SongListQuery : AppQuery<SongDTO>
    {
        public SongListQuery(IUnitOfWorkProvider provider) : base(provider) {}

        public SongFilter Filter { get; set; }

        protected override IQueryable<SongDTO> GetQueryable()
        {
            IQueryable<Song> query = Context.Songs;

            if (!string.IsNullOrEmpty(Filter?.Name))
            {
                query = query.Where(song => song.Name.ToLower().Contains(Filter.Name.ToLower()));
            }

            if (Filter?.AlbumId >= 0)
            {
                query = query.Where(song => song.Album.ID == Filter.AlbumId);
            }

            if (Filter?.Genre >= 0)
            {
                query = query.Where(song => (int) song.Genre == Filter.Genre);
            }

            if (Filter?.CreatedBy >= 0)
            {
                query = query.Where(song => song.CreatedBy == Filter.CreatedBy.Value);
            }

            if (Filter?.IsPublic != null)
            {
                query = query.Where(song => song.IsPublic == Filter.IsPublic.Value);
            }
            if (Filter?.Publish != null)
            {
                query = query.Where(song => song.Publish == Filter.Publish.Value);
            }

            if (Filter?.OriginalSongId != null)
            {
                query = query.Where(song => song.OriginalSongId != int.MinValue);
            }

            return query.ProjectTo<SongDTO>();
        }
    }
}