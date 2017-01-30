using System;
using System.Collections.Generic;
using AutoMapper;
using BL.DTOs.Events;
using BL.DTOs.Filters;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services.Events
{
    public class EventService : MusicLibraryService, IEventService
    {

        #region Dependencies

        private readonly EventRepository _eventRepository;

        private readonly EventListQuery _eventListQuery;

        private readonly InterpretRepository _interpretRepository;

        public EventService(EventRepository eventRepository, EventListQuery eventListQuery,
            InterpretRepository interpretRepository)
        {
            _eventRepository = eventRepository;
            _eventListQuery = eventListQuery;
            _interpretRepository = interpretRepository;
        }

        #endregion

        public void CreateEvent(EventDTO eventDto, int interpretId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var e = Mapper.Map<Event>(eventDto);
                e.Interpret = GetEventInterpret(interpretId);
                _eventRepository.Insert(e);
                uow.Commit();
            }
        }

        public void EditEvent(EventDTO eventDto, int interpretId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var e = _eventRepository.GetById(eventDto.ID);
                Mapper.Map(eventDto, e);
                e.Interpret = GetEventInterpret(interpretId);
                _eventRepository.Update(e);
                uow.Commit();
            }
        }

        public void DeleteEvent(int eventId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _eventRepository.Delete(eventId);
                uow.Commit();
            }
        }

        public EventDTO GetEvent(int eventId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var e = _eventRepository.GetById(eventId);
                return e != null ? Mapper.Map<EventDTO>(e) : null;
            }
        }

        public IEnumerable<EventDTO> ListEvents(EventFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = GetQuery(filter);
                _eventListQuery.Filter = filter;

                var sortEvent = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                query.AddSortCriteria("Time", sortEvent);
                return query.Execute() ?? new List<EventDTO>();
            }
        }

        private Interpret GetEventInterpret(int interpretId)
        {
            var interpret = _interpretRepository.GetById(interpretId);
            if (interpret == null)
            {
                throw new NullReferenceException("Event service - GetEventInterpret(...) interpret cant be null");
            }
            return interpret;
        }

        private IQuery<EventDTO> GetQuery(EventFilter filter)
        {
            var query = _eventListQuery;
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }


    }
}