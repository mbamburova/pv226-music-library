using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BL.DTOs.Events;
using BL.DTOs.Filters;
using BL.Facades;
using PL.Models;

namespace PL.Controllers
{
    public class EventsController : Controller
    {
        #region Facades 

        public EventFacade EventFacade { get; set; }

        public InterpretFacade InterpretFacade { get; set; }

        #endregion

        #region EventsActionMethod

        public ActionResult Index()
        {
            var events = EventFacade.GetEvents(new EventFilter {SortAscending = true});
            var eventDisplayDtos = new List<EventDisplayDTO>();

            foreach (var e in events)
            {
                eventDisplayDtos.Add(new EventDisplayDTO
                {
                    ID = e.ID,
                    Name = e.Name,
                    Place = e.Place,
                    EventLink = e.EventLink,
                    Time = e.Time,
                    Interpret = InterpretFacade.GetInterpret(e.InterpretId)
                });
            }
            var model = new EventsViewModel {Events = eventDisplayDtos};

            return View("EventsView", model);
        }

        public ActionResult Delete(int id)
        {
            EventFacade.DeleteEvent(id);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var e = EventFacade.GetEvent(id);
            var model = new EventCreateModel
            {
                EventDto = e,
                Interprets = GetInterpretSelectList(),
                InterpretId = e.InterpretId.ToString()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventCreateModel model)
        {
            if (ModelState.IsValid)
            {
                EventFacade.EditEvent(model.EventDto, Convert.ToInt32(model.InterpretId));

                return RedirectToAction("Index");
            }
            model.Interprets = GetInterpretSelectList();
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new EventCreateModel
            {
                EventDto = new EventDTO(),
                Interprets = GetInterpretSelectList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventCreateModel model)
        {
            if (ModelState.IsValid)
            {
                EventFacade.CreateEvent(model.EventDto, Convert.ToInt32(model.InterpretId));
                return RedirectToAction("Index");
            }
            model.Interprets = GetInterpretSelectList();
            return View(model);
        }

        private IEnumerable<SelectListItem> GetInterpretSelectList()
        {
            return InterpretFacade.ListInterprets(null).Select(i => new SelectListItem
            {
                Value = i.ID.ToString(),
                Text = i.Name
            }).AsEnumerable();
        }

        #endregion
    }
}