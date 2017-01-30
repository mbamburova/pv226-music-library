using System.Web.Mvc;
using BL.Facades;
using BL.Utils.AccountPolicy;
using PL.Models;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.Admin)]
    public class SongManagementController : Controller
    {
        #region Facades

        public SongFacade SongFacade { get; set; }

        #endregion

        #region AdminSongActionMethods

        public ActionResult Index()
        {
            var model = new SongManagementModel
            {
                SongsForPublish = SongFacade.ListSongsForPublish(),
                SongsEditedForConfirmation = SongFacade.ListSongEditedUnconfirmed(),
                SongsBeforeEdit = SongFacade.ListSongBeforeUpdate()
            };

            return View("AdminSongListView", model);
        }


        public ActionResult Publish(int id)
        {
            SongFacade.PublishSong(id);

            return RedirectToAction("Index");
        }

        public ActionResult Confirm(int id)
        {
            SongFacade.ConfirmEditedSong(id);

            return RedirectToAction("Index");
        }

        public ActionResult Cancel(int id)
        {
            SongFacade.CancelRequestForPublish(id);

            return RedirectToAction("Index");
        }

        public ActionResult CancelEditSong(int id)
        {
            SongFacade.DeleteSong(id);

            return RedirectToAction("Index");
        }
    }

    #endregion
}