using System.Collections.Generic;
using System.Linq;
using BL.Bootstrap;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.Services.Albums;
using BL.Services.Interprets;
using Castle.Windsor;
using DAL;
using DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTests.Services.Albums
{
    [TestClass]
    public class AlbumServiceTests
    {
        private static IAlbumService _albumService;
        private static IInterpretService _interpretService;

        private static readonly MusicLibraryDbContext Context = new MusicLibraryDbContext();

        public static readonly IWindsorContainer Container = new WindsorContainer();

        private static int _interpret1Id;
        private static int _interpret2Id;

        private static int _album1Id;
        private static int _album2Id;

        private AlbumDTO _album1;
        private AlbumDTO _album2;

        private InterpretDTO _interpret1;
        private InterpretDTO _interpret2;


        [ClassInitialize]
        public static void ClassInit(TestContext context1)
        {
            Container.Install(new BussinessLayerInstaller());
            MappingInit.ConfigureMapping();
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
                ID = _interpret1Id,
                Name = "System of a down",
                Language = Language.English,
                IsPublic = false
            };

            _interpret2Id = 2;
            _interpret2 = new InterpretDTO
            {
                ID = _interpret2Id,
                Name = "Linkin Park",
                Language = Language.English,
                IsPublic = false
            };
            _interpretService.CreateInterpret(_interpret1);
            _interpretService.CreateInterpret(_interpret2);


            _album1Id = 1;
            _album1 = new AlbumDTO
            {
                ID = _album1Id,
                InterpretId = _interpret1Id,
                Name = "Toxicity",
                Year = 2001
            };

            _album2Id = 2;
            _album2 = new AlbumDTO
            {
                ID = _album2Id,
                InterpretId = _interpret2Id,
                Name = "Meteora",
                Year = 2003
            };

            _albumService.CreateAlbum(_album1, _interpret1Id);
            _albumService.CreateAlbum(_album2, _interpret2Id);
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
        public void CreateAlbumTest()
        {
            Assert.AreEqual(2, _albumService.ListAlbums(null).Count());

            var interpret3 = new InterpretDTO
            {
                ID = 3,
                Name = "Kabat",
                Language = Language.Czech,
                IsPublic = false
            };
            _interpretService.CreateInterpret(interpret3);

            var album3 = new AlbumDTO
            {
                ID = 3,
                InterpretId = _interpret1Id,
                Name = "Dole v dole",
                Year = 2003
            };
            _albumService.CreateAlbum(album3, interpret3.ID);

            Assert.AreEqual(3, _albumService.ListAlbums(null).Count());
        }

        [TestMethod]
        public void EditAlbumTest()
        {
            _albumService.EditAlbum(
                new AlbumDTO {ID = _album1Id, InterpretId = _interpret1Id, Name = "ToxicityEDITED"}, _interpret2Id,
                new[] {3, 4}, new[] {5, 6});
            Assert.AreEqual("ToxicityEDITED", _albumService.GetAlbum(_album1Id).Name);
            Assert.AreEqual(_interpret2Id, _albumService.GetAlbum(_album1Id).InterpretId);
        }

        [TestMethod]
        public void DeleteAlbumTest()
        {
            _albumService.DeleteAlbum(_album1Id);
            _albumService.DeleteAlbum(_album2Id);

            Assert.IsNull(_albumService.GetAlbum(_album1Id));
            Assert.IsNull(_albumService.GetAlbum(_album2Id));
        }

        [TestMethod]
        public void GetAlbumTest()
        {
            Assert.AreEqual(_album2.ID, _albumService.GetAlbum(_album2.ID).ID);
            Assert.AreEqual(_album2.Name, _albumService.GetAlbum(_album2.ID).Name);

            Assert.AreEqual(_album1.ID, _albumService.GetAlbum(_album1.ID).ID);
            Assert.AreEqual(_album1.Name, _albumService.GetAlbum(_album1.ID).Name);
        }

        [TestMethod]
        public void ListAlbumsTest()
        {
            Assert.AreEqual(2, _albumService.ListAlbums(null).Count());
        }

        [TestMethod]
        public void ListAlbumsByInterpretTest()
        {
            Assert.AreEqual(1, _albumService.ListAlbums(new AlbumFilter {InterpretId = _interpret1Id}).Count());
            Assert.AreEqual(1, _albumService.ListAlbums(new AlbumFilter {InterpretId = _interpret2Id}).Count());
        }

        [TestMethod]
        public void ListAlbumsByNameTest()
        {
            Assert.AreEqual(1, _albumService.ListAlbums(new AlbumFilter {Name = "Meteora"}).Count());
            Assert.AreEqual(1, _albumService.ListAlbums(new AlbumFilter {Name = "Toxicity"}).Count());
        }

        [TestMethod]
        public void ListAlbumsByContainsTest()
        {
            Assert.AreEqual(2, _albumService.ListAlbums(new AlbumFilter {Name = "T"}).Count());
            Assert.AreEqual(1, _albumService.ListAlbums(new AlbumFilter {Name = "Mete"}).Count());
        }

        [TestMethod]
        public void ListAlbumsByNameAndYearTest()
        {
            Assert.AreEqual(1, _albumService.ListAlbums(new AlbumFilter {Name = "Meteora", Year = 2003}).Count());
            Assert.AreEqual(0, _albumService.ListAlbums(new AlbumFilter {Name = "Toxicity", Year = 2003}).Count());
        }
    }
}