using System.Collections.Generic;
using BL.DTOs.Events;
using BL.DTOs.Filters;

namespace BL.Services.Events
{
    public interface IEventService
    {
        void CreateEvent(EventDTO eventDto, int interpretId);

        void EditEvent(EventDTO eventDto, int interpretId);

        void DeleteEvent(int eventId);

        EventDTO GetEvent(int eventId);

        IEnumerable<EventDTO> ListEvents(EventFilter filter);
    }
}