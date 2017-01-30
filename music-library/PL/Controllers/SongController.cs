using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.DTOs.Reviews;
using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using BL.Facades;
using BL.Utils.AccountPolicy;
using PL.Models;

namespace PL.Controllers
{
    public class SongController : Controller
    {
        #region Facades

        public SongFacade SongFacade { get; set; }
        public AlbumFacade AlbumFacade { get; set; }
        public InterpretFacade InterpretFacade { get; set; }
        public ReviewFacade ReviewFacade { get; set; }

        public UserFacade UserFacade { get; set; }
        public PlaylistFacade PlaylistFacade { get; set; }

        #endregion

        #region SongActionMethods

        public ActionResult Index()
        {
            var model = new SongListViewModel
            {
                Songs = SongFacade.ListSongs(new SongFilter {IsPublic = true}) as IList<SongDTO>,
                Albums = AlbumFacade.ListAlbums(null) as IList<AlbumDTO>,
                Interprets = InterpretFacade.ListInterprets(null) as IList<InterpretDTO>
            };

            if (!User.IsInRole(Claims.User))
            {
                return View("SongListView", model);
            }
            var user = UserFacade.GetUserAccordingToEmail(User.Identity.Name);
            var playlist =
                PlaylistFacade.ListPlaylists(new PlaylistFilter {UserId = user.ID, Name = "All my songs"}).First();
            model.UserSongs = (List<SongDTO>) SongFacade.ListSongsByPlaylist(playlist.ID);

            return View("SongListView", model);
        }

        [HttpPost]
        public ActionResult Index(SongListViewModel filter)
        {
            var model = new SongListViewModel
            {
                Songs =
                    SongFacade.ListSongs(new SongFilter {IsPublic = true, Name = filter.SongFilter.Name}) as
                        IList<SongDTO>,
                Albums = AlbumFacade.ListAlbums(null) as IList<AlbumDTO>,
                Interprets = InterpretFacade.ListInterprets(null) as IList<InterpretDTO>
            };

            if (!User.IsInRole(Claims.User))
            {
                return View("SongListView", model);
            }
            var user = UserFacade.GetUserAccordingToEmail(User.Identity.Name);
            var playlist =
                PlaylistFacade.ListPlaylists(new PlaylistFilter {UserId = user.ID, Name = "All my songs"}).First();
            model.UserSongs = (List<SongDTO>) SongFacade.ListSongsByPlaylist(playlist.ID);

            return View("SongListView", model);
        }

        public ActionResult ListSongsByAlbum(int id)
        {
            var album = AlbumFacade.GetAlbum(id);

            var model = new SongListViewModel
            {
                Songs = SongFacade.ListSongs(new SongFilter {AlbumId = id, IsPublic = true}) as IList<SongDTO>,
                Interpret = InterpretFacade.GetInterpret(album.InterpretId),
                Album = album,
                AlbumReviews =
                    (IList<AlbumReviewDTO>) ReviewFacade.ListAlbumReviews(new AlbumReviewFilter {AlbumId = id})
            };
            return View("ListSongsByFilter", model);
        }

        public ActionResult Create()
        {
            var model = new SongCreateModel
            {
                Interprets = InterpretFacade.ListInterprets(null).Select(c => new SelectListItem
                {
                    Value = c.ID.ToString(),
                    Text = c.Name
                }).AsEnumerable()
            };

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SongCreateModel model, SongDTO song)
        {
            if (ModelState.IsValid)
            {
                song.Publish = false;
                song.Added = DateTime.Now;
                song.OriginalSongId = int.MinValue;
                song.AlbumId =
                    AlbumFacade.ListAlbums(new AlbumFilter
                    {
                        InterpretId = Convert.ToInt32(model.InterpretId),
                        Name = "Unknown"
                    }).First().ID;

                if (User.IsInRole(Claims.Admin))
                {
                    song.CreatedBy = int.MinValue;
                    song.IsPublic = true;
                    var songId = SongFacade.CreateSong(song, song.AlbumId);
                }
                else
                {
                    var user = UserFacade.GetUserAccordingToEmail(User.Identity.Name);
                    song.CreatedBy = UserFacade.GetUserAccordingToEmail(User.Identity.Name).ID;
                    song.IsPublic = false;
                    var playlist =
                        PlaylistFacade.ListPlaylists(new PlaylistFilter {UserId = user.ID, Name = "All my songs"})
                            .First();
                    var songId = SongFacade.CreateSong(song, song.AlbumId);
                    SongFacade.CreateSonglist(new SongListDTO(), songId, playlist.ID);
                }

                return RedirectToAction("Index", User.IsInRole(Claims.User) ? "SongCollection" : "SongManagement");
            }
            model.Interprets = InterpretFacade.ListInterprets(null).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Name
            }).AsEnumerable();

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var song = SongFacade.GetSong(id);

            var model = new SongListViewModel
            {
                Song = song,
                Album = AlbumFacade.GetAlbum(song.AlbumId),
                SongReviews = (IList<SongReviewDTO>) ReviewFacade.ListSongReviews(new SongReviewFilter {SongId = id})
            };
            return View("Details", model);
        }

        public ActionResult Edit(int id)
        {
            var song = SongFacade.GetSong(id);

            return View(song);
        }

        public ActionResult Delete(int id)
        {
            SongFacade.DeleteSong(id);
            if (User.IsInRole(Claims.User))
            {
                return RedirectToAction("Index", "SongCollection");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SongDTO song)
        {
            if (!ModelState.IsValid)
            {
                return View(song);
            }
            var reviews = ReviewFacade.ListSongReviews(new SongReviewFilter {SongId = song.ID});
            var reviewIds = reviews.Select(a => a.ID).ToArray();

            if (User.IsInRole(Claims.Admin) || !song.IsPublic)
            {
                song.Publish = true;
                SongFacade.EditSong(song, song.AlbumId, reviewIds);
                return RedirectToAction("Index", "Song");
            }
            song.OriginalSongId = song.ID;
            if (song.IsPublic)
            {
                SongFacade.EditSongUnconfirmed(song, song.AlbumId, reviewIds);
            }

            return RedirectToAction("Index", "SongCollection");
        }

        public ActionResult CreateSongReview(int id)
        {
            return View("CreateSongReview", new SongReviewDTO {SongId = id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSongReview(SongReviewDTO songReview)
        {
            if (ModelState.IsValid)
            {
                ReviewFacade.CreateSongReview(songReview, songReview.SongId);
            }
            return RedirectToAction("Index");
        }

        public ActionResult AskForPublication(int id)
        {
            SongFacade.AskForPublication(id);

            return RedirectToAction("Index", "SongCollection");
        }

        public ActionResult CreateAlbumReview(int id)
        {
            var model = new SongListViewModel
            {
                AlbumReviewDto = new AlbumReviewDTO
                {
                    AlbumId = id
                },
                Album = AlbumFacade.GetAlbum(id)
            };
            return View("CreateAlbumReview", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAlbumReview(AlbumReviewDTO albumReviewDto)
        {
            if (ModelState.IsValid)
            {
                ReviewFacade.CreateAlbumReview(albumReviewDto, albumReviewDto.AlbumId);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}