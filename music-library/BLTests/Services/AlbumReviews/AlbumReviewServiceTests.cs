using System.Collections.Generic;
using System.Linq;
using BL.Bootstrap;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.DTOs.Reviews;
using BL.DTOs.Users;
using BL.Services.AlbumReviews;
using BL.Services.Albums;
using BL.Services.Interprets;
using Castle.Windsor;
using DAL;
using DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTests.Services.AlbumReviews
{
    [TestClass]
    public class AlbumReviewServiceTests
    {
        private static IAlbumReviewService _albumReviewService;
        private static IAlbumService _albumService;
        private static IInterpretService _interpretService;
        private static readonly MusicLibraryDbContext Context = new MusicLibraryDbContext();


        public static readonly IWindsorContainer Container = new WindsorContainer();

        private static int _album1Id;
        private static int _album2Id;

        private static int _interpret1Id;
        private static int _interpret2Id;

        private static int _albumReview1Id;
        private static int _albumReview2Id;

        private AlbumDTO _album1;
        private AlbumDTO _album2;

        private AlbumReviewDTO _albumReview1;
        private AlbumReviewDTO _albumReview2;

        private InterpretDTO _interpret1;
        private InterpretDTO _interpret2;

        public UserAccountDTO Use { get; set; }

        [ClassInitialize]
        public static void ClassInit(TestContext context1)
        {
            Container.Install(new BussinessLayerInstaller());
            MappingInit.ConfigureMapping();
            _albumReviewService = Container.Resolve<IAlbumReviewService>();
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
                InterpretId = _interpret1Id,
                Name = "Toxicity",
                Year = 2003
            };

            _album2Id = 2;
            _album2 = new AlbumDTO
            {
                ID = _album2Id,
                InterpretId = _interpret2Id,
                Name = "Meteora",
                Year = 2008
            };

            _albumService.CreateAlbum(_album1, _interpret1Id);
            _albumService.CreateAlbum(_album2, _interpret2Id);

            _albumReview1Id = 1;
            _albumReview1 = new AlbumReviewDTO
            {
                ID = _albumReview1Id,
                AlbumId = _album1.ID,
                Note = "Perfect album",
                Rating = 9
            };

            _albumReview2Id = 2;
            _albumReview2 = new AlbumReviewDTO
            {
                AlbumId = _album2.ID,
                Note = "Not bad",
                Rating = 8,
                ID = _albumReview2Id
            };

            _albumReviewService.CreateAlbumReview(_albumReview1, _album1Id);
            _albumReviewService.CreateAlbumReview(_albumReview2, _album2Id);
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
        public void CreateAlbumReviewTest()
        {
            Assert.AreEqual(2,
                _albumReviewService.ListAlbumReviews(new AlbumReviewFilter {SortAscending = true}).Count());

            var albumId = 3;
            var album3 = new AlbumDTO
            {
                ID = albumId,
                InterpretId = _interpret2Id,
                Name = "Hybrid Theory",
                Year = 2000
            };

            _albumService.CreateAlbum(album3, _interpret2Id);

            var albumReview = new AlbumReviewDTO
            {
                AlbumId = albumId,
                Note = "Not bad",
                Rating = 9,
                ID = _albumReview2Id
            };

            _albumReviewService.CreateAlbumReview(albumReview, albumId);

            Assert.AreEqual(3,
                _albumReviewService.ListAlbumReviews(new AlbumReviewFilter {SortAscending = true}).Count());
        }

        [TestMethod]
        public void SortTest()
        {
            Assert.AreEqual(_albumReview2.Rating,
                _albumReviewService.ListAlbumReviews(new AlbumReviewFilter {SortAscending = true}).First().Rating);

            var albumId = 3;
            var album3 = new AlbumDTO
            {
                ID = albumId,
                InterpretId = _interpret2Id,
                Name = "Hybrid Theory",
                Year = 2000
            };

            _albumService.CreateAlbum(album3, _interpret2Id);

            var albumReview = new AlbumReviewDTO
            {
                AlbumId = albumId,
                Note = "Not bad",
                Rating = 9,
                ID = _albumReview2Id
            };

            _albumReviewService.CreateAlbumReview(albumReview, albumId);

            Assert.AreEqual(_albumReview1.Rating,
                _albumReviewService.ListAlbumReviews(new AlbumReviewFilter {SortAscending = false}).First().Rating);
        }

        [TestMethod]
        public void EditAlbumReviewTest()
        {
            _albumReviewService.EditAlbumReview(
                new AlbumReviewDTO
                {
                    ID = _albumReview1Id,
                    AlbumId = _album2Id,
                    Note = "EDITED NOTE",
                    Rating = 0
                }, _album2Id);

            var editedAlbumReview = _albumReviewService.GetAlbumReview(_albumReview1Id);

            Assert.AreEqual(_album2Id, editedAlbumReview.AlbumId);
            Assert.AreEqual("EDITED NOTE", editedAlbumReview.Note);
            Assert.AreEqual(0.0, editedAlbumReview.Rating);
        }

        [TestMethod]
        public void DeleteAlbumReviewTest()
        {
            _albumReviewService.DeleteAlbumReview(_albumReview1Id);
            Assert.AreEqual(1,
                _albumReviewService.ListAlbumReviews(new AlbumReviewFilter {SortAscending = true}).Count());

            _albumReviewService.DeleteAlbumReview(_albumReview2Id);
            Assert.AreEqual(0,
                _albumReviewService.ListAlbumReviews(new AlbumReviewFilter {SortAscending = true}).Count());
        }


        [TestMethod]
        public void GetAlbumReviewTest()
        {
            var albumReview = _albumReviewService.GetAlbumReview(_albumReview1Id);

            Assert.AreEqual(_albumReview1.Note, albumReview.Note);
            Assert.AreEqual(_albumReview1.Rating, albumReview.Rating);
            Assert.AreEqual(_albumReview1.AlbumId, albumReview.AlbumId);
        }

        [TestMethod]
        public void ListAlbumReviewsByAlbumTest()
        {
            Assert.AreEqual(1, _albumReviewService.ListAlbumReviews(new AlbumReviewFilter {AlbumId = _album1Id}).Count());
            Assert.AreEqual(0,
                _albumReviewService.ListAlbumReviews(new AlbumReviewFilter {AlbumId = int.MaxValue}).Count());
        }

        [TestMethod]
        public void ListAlbumReviewsTest()
        {
            Assert.AreEqual(2,
                _albumReviewService.ListAlbumReviews(new AlbumReviewFilter {SortAscending = true}).Count());
        }
    }
}