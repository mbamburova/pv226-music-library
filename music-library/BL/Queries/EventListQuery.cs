using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Events;
using BL.DTOs.Filters;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class EventListQuery : AppQuery<EventDTO>
    {
        public EventListQuery(IUnitOfWorkProvider provider) : base(provider) {}

        public EventFilter Filter { get; set; }

        protected override IQueryable<EventDTO> GetQueryable()
        {
            IQueryable<Event> query = Context.Events;

            if (!string.IsNullOrEmpty(Filter?.Name))
            {
                query = query.Where(e => e.Name.ToLower().Contains(Filter.Name.ToLower()));
            }

            if (Filter?.InterpretId >= 0)
            {
                query = query.Where(e => e.Interpret.ID == Filter.InterpretId);
            }
            if (!string.IsNullOrEmpty(Filter?.Place))
            {
                query = query.Where(e => e.Place.ToLower().Contains(Filter.Place.ToLower()));
            }
            if (Filter?.SortAscending == true)
            {
                query = query.OrderBy(e => e.Time);
            }
            return query.ProjectTo<EventDTO>();
        }
    }
}