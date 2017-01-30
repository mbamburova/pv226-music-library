using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class InterpretListQuery : AppQuery<InterpretDTO>
    {
        public InterpretListQuery(IUnitOfWorkProvider provider) : base(provider) {}

        public InterpretFilter Filter { get; set; }

        protected override IQueryable<InterpretDTO> GetQueryable()
        {
            IQueryable<Interpret> query = Context.Interprets;

            if (!string.IsNullOrEmpty(Filter?.Name))
            {
                query = query.Where(interpret => interpret.Name.ToLower().Contains(Filter.Name.ToLower()));
            }

            if (Filter?.Language >= 0)
            {
                query = query.Where(song => (int) song.Language == Filter.Language);
            }
            return query.ProjectTo<InterpretDTO>();
        }
    }
}