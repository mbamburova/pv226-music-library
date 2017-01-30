using System.Collections.Generic;
using BL.DTOs.Events;
using BL.DTOs.Filters;
using BL.Services.Events;

namespace BL.Facades
{
    public class EventFacade
    {
        private readonly IEventService _eventService;

        public EventFacade(IEventService eventService)
        {
            _eventService = eventService;
        }

        public void CreateEvent(EventDTO eventDto, int interpretId)
        {
            _eventService.CreateEvent(eventDto, interpretId);
        }

        public void EditEvent(EventDTO eventDto, int interpretId)
        {
            _eventService.EditEvent(eventDto, interpretId);
        }

        public void DeleteEvent(int eventId)
        {
            _eventService.DeleteEvent(eventId);
        }

        public EventDTO GetEvent(int eventId)
        {
            return _eventService.GetEvent(eventId);
        }

        public IEnumerable<EventDTO> GetEvents(EventFilter filter)
        {
            return _eventService.ListEvents(filter);
        }
    }
}