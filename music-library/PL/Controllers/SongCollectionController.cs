using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.DTOs.Playlists;
using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using BL.Facades;
using PL.Models;

namespace PL.Controllers
{
    public class SongCollectionController : Controller
    {
        #region Facades

        public SongFacade SongFacade { get; set; }
        public PlaylistFacade PlaylistFacade { get; set; }

        public UserFacade UserFacade { get; set; }

        public AlbumFacade AlbumFacade { get; set; }
        public InterpretFacade InterpretFacade { get; set; }

        #endregion

        #region SongCollectionActionMethods

        public ActionResult Index()
        {
            return View("Index", CreateCollectionModel());
        }

        public ActionResult Create()
        {
            var playlist = new PlaylistDTO();

            return View("Create", playlist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlaylistDTO playlist)
        {
            if (playlist.Name == null || playlist.Name.Equals("All my songs"))
            {
                playlist = new PlaylistDTO();
                return View(playlist);
            }

            var user = UserFacade.GetUserAccordingToEmail(User.Identity.Name);
            playlist.UserId = user.ID;
            playlist.Created = DateTime.Now;

            PlaylistFacade.CreatePlaylist(playlist, user.ID);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            PlaylistFacade.DeletePlaylist(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(PlaylistModel model, int id)
        {
            if (model == null)
            {
                model = new PlaylistModel();
            }

            model.Songs = (List<SongDTO>) SongFacade.ListSongsByPlaylist(id);
            model.Albums = (List<AlbumDTO>) AlbumFacade.ListAlbums(null);
            model.Playlist = PlaylistFacade.GetPlaylist(id);

            return View("Details", model);
        }


        public ActionResult AddToCollection(int id)
        {
            var user = UserFacade.GetUserAccordingToEmail(User.Identity.Name);
            var playlist =
                PlaylistFacade.ListPlaylists(new PlaylistFilter {UserId = user.ID, Name = "All my songs"}).First();

            SongFacade.CreateSonglist(new SongListDTO(), id, playlist.ID);

            return RedirectToAction("Index", "Song");
        }

        public ActionResult RemoveFromCollection(int id)
        {
            var user = UserFacade.GetUserAccordingToEmail(User.Identity.Name);
            var playlist =
                PlaylistFacade.ListPlaylists(new PlaylistFilter {UserId = user.ID, Name = "All my songs"}).First();
            var songlist =
                SongFacade.ListSonglists(new SongListFilter {PlaylistId = playlist.ID})
                    .First(list => list.SongId.Equals(id));

            SongFacade.DeleteSonglist(songlist.ID);

            return RedirectToAction("Index", "SongCollection");
        }

        public ActionResult AddToPlaylist(int playlistId, int songId)
        {
            var model = CreateCollectionModel();

            model.IsAdded = true;
            model.Song = SongFacade.GetSong(songId);
            model.Playlist = PlaylistFacade.GetPlaylist(playlistId);

            SongFacade.CreateSonglist(new SongListDTO(), songId, playlistId);

            return View("Index", model);
        }

        public ActionResult Shuffle(int id)
        {
            var model = new PlaylistModel
            {
                Songs = SongFacade.ListSongsByPlaListShuffle(id),
                Albums = AlbumFacade.ListAlbums(null) as List<AlbumDTO>,
                Playlist = PlaylistFacade.GetPlaylist(id)
            };

            return View("Details", model);
        }

        public ActionResult RemoveFromPlaylist(PlaylistModel model, int playListId, int songId)
        {
            var songlist = SongFacade.ListSonglists(new SongListFilter {PlaylistId = playListId})
                .First(song => song.SongId.Equals(songId));
            SongFacade.DeleteSonglist(songlist.ID);

            model.Songs = (List<SongDTO>) SongFacade.ListSongsByPlaylist(playListId);
            model.Albums = (List<AlbumDTO>) AlbumFacade.ListAlbums(null);
            model.Playlist = PlaylistFacade.GetPlaylist(playListId);

            return View("Details", model);
        }

        public CollectionModel CreateCollectionModel()
        {
            var user = UserFacade.GetUserAccordingToEmail(User.Identity.Name);
            var initPlaylist =
                PlaylistFacade.ListPlaylists(new PlaylistFilter {Name = "All my songs", UserId = user.ID}).First();
            return new CollectionModel
            {
                Playlists =
                    PlaylistFacade.ListPlaylists(new PlaylistFilter {UserId = user.ID}),
                Songs = (List<SongDTO>) SongFacade.ListSongsByPlaylist(initPlaylist.ID),
                Albums = (List<AlbumDTO>) AlbumFacade.ListAlbums(null),
                Interprets = (List<InterpretDTO>) InterpretFacade.ListInterprets(null)
            };
        }

        #endregion
    }
}