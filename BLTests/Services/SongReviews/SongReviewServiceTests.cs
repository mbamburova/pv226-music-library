using System;
using System.Collections.Generic;
using System.Linq;
using BL.Bootstrap;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.DTOs.Reviews;
using BL.DTOs.Songs;
using BL.Services.Albums;
using BL.Services.Interprets;
using BL.Services.SongReviews;
using BL.Services.Songs;
using Castle.Windsor;
using DAL;
using DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTests.Services.SongReviews
{
    [TestClass]
    public class SongReviewServiceTests
    {
        private static ISongReviewService _songReviewService;
        private static IAlbumService _albumService;
        private static ISongService _songService;
        private static IInterpretService _interpretService;
        private static readonly MusicLibraryDbContext Context = new MusicLibraryDbContext();

        public static readonly IWindsorContainer Container = new WindsorContainer();

        private static int _song1Id;
        private static int _song2Id;
        private static int _song3Id;

        private static int _album1Id;
        private static int _album2Id;

        private static int _interpret1Id;
        private static int _interpret2Id;

        private static int _songReview1Id;
        private static int _songReview2Id;

        private AlbumDTO _album1;
        private AlbumDTO _album2;

        private InterpretDTO _interpret1;
        private InterpretDTO _interpret2;

        private SongDTO _song1;
        private SongDTO _song2;
        private SongDTO _song3;

        private SongReviewDTO _songReview1;
        private SongReviewDTO _songReview2;

        [ClassInitialize]
        public static void ClassInit(TestContext context1)
        {
            Container.Install(new BussinessLayerInstaller());
            MappingInit.ConfigureMapping();
            _songReviewService = Container.Resolve<ISongReviewService>();
            _songService = Container.Resolve<ISongService>();
            _albumService = Container.Resolve<IAlbumService>();
            _interpretService = Container.Resolve<IInterpretService>();

            DeleteTables();
        }

        [TestInitialize]
        public void Init()
        {
            _interpret1Id = 1;
            _interpret1 = new InterpretDTO
            {
                Name = "System of a down",
                Language = Language.English,
                ID = _interpret1Id
            };

            _interpret2Id = 2;
            _interpret2 = new InterpretDTO
            {
                Name = "Linkin Park",
                Language = Language.English,
                ID = _interpret2Id
            };

            _interpretService.CreateInterpret(_interpret1);
            _interpretService.CreateInterpret(_interpret2);

            _album1Id = 1;
            _album1 = new AlbumDTO
            {
                ID = _album1Id,
                InterpretId = 1,
                Name = "Toxicity",
                Year = 2001
            };

            _album2Id = 2;
            _album2 = new AlbumDTO
            {
                ID = _album2Id,
                InterpretId = 2,
                Name = "Meteora",
                Year = 2003
            };
            _albumService.CreateAlbum(_album1, _interpret1.ID);
            _albumService.CreateAlbum(_album2, _interpret2.ID);

            _song1Id = 1;
            _song1 = new SongDTO
            {
                ID = _song1Id,
                Name = "Prison Song",
                Added = new DateTime(2016, 11, 6),
                AlbumId = _album1Id,
                Genre = Genre.Rock
            };

            _song2Id = 2;
            _song2 = new SongDTO
            {
                ID = _song2Id,
                Name = "Deer Dance",
                Added = new DateTime(2016, 11, 6),
                AlbumId = _album1Id,
                Genre = Genre.Rock
            };

            _song3Id = 3;
            _song3 = new SongDTO
            {
                ID = _song3Id,
                Name = "Numb",
                Added = new DateTime(2016, 11, 6),
                AlbumId = _album2Id,
                Genre = Genre.Rock
            };

            _songService.CreateSong(_song1, _album1Id);
            _songService.CreateSong(_song2, _album1Id);
            _songService.CreateSong(_song3, _album2Id);

            _songReview1Id = 1;
            _songReview1 = new SongReviewDTO
            {
                ID = _songReview1Id,
                SongId = _song1.ID,
                Note = "Perfect album",
                Rating = 9
            };

            _songReview2Id = 2;
            _songReview2 = new SongReviewDTO
            {
                SongId = _song2.ID,
                Note = "Not bad",
                Rating = 8,
                ID = _songReview2Id
            };

            _songReviewService.CreateSongReview(_songReview1, _song1Id);
            _songReviewService.CreateSongReview(_songReview2, _song2Id);
        }

        [TestCleanup]
        public void AfterTestMethod()
        {
            DeleteTables();
        }

        private static void DeleteTables()
        {
            var listOfTables = new List<string>
            {
                "AlbumReviews",
                "Albums",
                "Events",
                "Interprets",
                "Playlists",
                "SongLists",
                "SongReviews",
                "Songs",
                "Users",
                "UserAccounts"
            };

            foreach (var tableName in listOfTables)
            {
                Context.Database.ExecuteSqlCommand("DELETE FROM [" + tableName + "]" + "DBCC CHECKIDENT (" + tableName +
                                                   ",RESEED, 0)");
            }
            Context.SaveChanges();
        }

        [TestMethod]
        public void CreateSongReviewTest()
        {
            Assert.AreEqual(2, _songReviewService.ListSongReviews(null).Count());

            var songReview3 = new SongReviewDTO
            {
                SongId = _song3Id,
                ID = 3,
                Note = "Great!",
                Rating = 10
            };
            _songReviewService.CreateSongReview(songReview3, _song3Id);

            Assert.AreEqual(3, _songReviewService.ListSongReviews(null).Count());
        }

        [TestMethod]
        public void EditSongReviewTest()
        {
            _songReviewService.EditSongReview(
                new SongReviewDTO
                {
                    ID = _songReview1Id,
                    SongId = _song1Id,
                    Rating = 10,
                    Note = "EDITED - OK"
                }, _song1Id);

            var editerSongReview = _songReviewService.GetSongReview(_songReview1Id);

            Assert.AreEqual("EDITED - OK", editerSongReview.Note);
            Assert.AreEqual(10.0, editerSongReview.Rating);
        }

        [TestMethod]
        public void DeleteSongReviewTest()
        {
            _songReviewService.DeleteSongReview(_songReview1Id);
            Assert.AreEqual(1, _songReviewService.ListSongReviews(null).Count());

            _songReviewService.DeleteSongReview(_songReview2Id);
            Assert.AreEqual(0, _songReviewService.ListSongReviews(null).Count());
        }

        [TestMethod]
        public void ListSongReviewsBySongTest()
        {
            Assert.AreEqual(1, _songReviewService.ListSongReviews(new SongReviewFilter {SongId = _song1Id}).Count());
            Assert.AreEqual(0, _songReviewService.ListSongReviews(new SongReviewFilter {SongId = _song3Id}).Count());
        }

        [TestMethod]
        public void GetSongReview()
        {
            Assert.AreEqual(_songReview1.Note, _songReviewService.GetSongReview(_songReview1Id).Note);
            Assert.AreEqual(_songReview1.Rating, _songReviewService.GetSongReview(_songReview1Id).Rating);

            Assert.AreEqual(_songReview2.Note, _songReviewService.GetSongReview(_songReview2Id).Note);
            Assert.AreEqual(_songReview2.SongId, _songReviewService.GetSongReview(_songReview2Id).SongId);
        }
    }
}