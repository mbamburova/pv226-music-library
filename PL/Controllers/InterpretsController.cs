using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BL.DTOs.Albums;
using BL.DTOs.Events;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.Facades;
using PL.Models;

namespace PL.Controllers
{
    public class InterpretsController : Controller
    {
        public ActionResult CreateAlbum(int id)
        {
            var model = new AlbumDTO
            {
                InterpretId = id
            };

            return View(model);
        }

        public ActionResult EditAlbum(int id)
        {
            var album = AlbumFacade.GetAlbum(id);

            return View(album);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAlbum(AlbumDTO album)
        {
            if (ModelState.IsValid)
            {
                var songs = SongFacade.ListSongs(new SongFilter {AlbumId = album.ID});
                var songsIds = songs.Select(song => song.ID).ToArray();

                var reviews = ReviewFacade.ListAlbumReviews(new AlbumReviewFilter {AlbumId = album.ID});
                var reviewIds = reviews.Select(review => review.ID).ToArray();

                AlbumFacade.EditAlbum(album, album.InterpretId, songsIds, reviewIds);

                return RedirectToAction("Index");
            }

            return View(album);
        }


        public ActionResult DeleteAlbum(int id)
        {
            AlbumFacade.DeleteAlbum(id);

            return RedirectToAction("Index");
        }

        #region Facades

        public InterpretFacade InterpretFacade { get; set; }
        public EventFacade EventFacade { get; set; }
        public AlbumFacade AlbumFacade { get; set; }
        public SongFacade SongFacade { get; set; }
        public ReviewFacade ReviewFacade { get; set; }

        #endregion

        #region InterpretActionMethods

        public ActionResult Index()
        {
            var model = new InterpretListViewModel
            {
                Interprets = InterpretFacade.ListInterprets(null) as IList<InterpretDTO>
            };

            return View("InterpretListView", model);
        }

        [HttpPost]
        public ActionResult Index(InterpretListViewModel filter)
        {
            var model = new InterpretListViewModel
            {
                Interprets = InterpretFacade.ListInterprets(filter.InterpretFilter) as IList<InterpretDTO>
            };

            return View("InterpretListView", model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InterpretDTO interpret)
        {
            var interpretId = InterpretFacade.CreateInterpret(interpret);
            AlbumFacade.CreateAlbumWithInterpret(new AlbumDTO {Name = "Unknown", InterpretId = interpretId}, interpretId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAlbum(AlbumDTO album)
        {
            AlbumFacade.CreateAlbumWithInterpret(album, album.InterpretId);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var interpret = InterpretFacade.GetInterpret(id);

            return View(interpret);
        }

        public ActionResult Delete(int id)
        {
            InterpretFacade.DeleteInterpret(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InterpretDTO interpret)
        {
            if (ModelState.IsValid)
            {
                var originalInterpreter = InterpretFacade.GetInterpret(id);
                originalInterpreter.Name = interpret.Name;
                originalInterpreter.Language = interpret.Language;

                InterpretFacade.EditInterpret(originalInterpreter, null, null);

                return RedirectToAction("Index");
            }

            return View(interpret);
        }

        public ActionResult Details(int id)
        {
            var model = new InterpretListViewModel
            {
                Interpret = InterpretFacade.GetInterpret(id),
                Events = EventFacade.GetEvents(new EventFilter {InterpretId = id}) as IList<EventDTO>,
                Albums =
                    (IList<AlbumDTO>)
                        AlbumFacade.ListAlbums(new AlbumFilter {InterpretId = id, Name = null, Year = null})
            };

            return View("Details", model);
        }

        #endregion
    }
}