using System;
using System.Collections.Generic;
using System.Linq;
using BL.Bootstrap;
using BL.DTOs.Albums;
using BL.DTOs.Events;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.Services.Albums;
using BL.Services.Events;
using BL.Services.Interprets;
using Castle.Windsor;
using DAL;
using DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTests.Services.Interprets
{
    [TestClass]
    public class InterpretServiceTests
    {
        private static IInterpretService _interpretService;
        private static IAlbumService _albumService;
        private static IEventService _eventService;

        private static int _interpret1Id;
        private static int _interpret2Id;
        private static int _interpret3Id;

        private static int _album1Id;
        private static int _album2Id;
        private static int _album3Id;
        private static int _album4Id;

        private static int _event1Id;
        private static int _event2Id;

        private static readonly MusicLibraryDbContext Context = new MusicLibraryDbContext();
        public static readonly IWindsorContainer Container = new WindsorContainer();

        private AlbumDTO _album1;
        private AlbumDTO _album2;
        private AlbumDTO _album3;
        private AlbumDTO _album4;

        private EventDTO _event1;
        private EventDTO _event2;

        private InterpretDTO _interpret1;
        private InterpretDTO _interpret2;
        private InterpretDTO _interpret3;


        [ClassInitialize]
        public static void ClassInit(TestContext context1)
        {
            Container.Install(new BussinessLayerInstaller());
            MappingInit.ConfigureMapping();

            _interpretService = Container.Resolve<IInterpretService>();
            _albumService = Container.Resolve<IAlbumService>();
            _eventService = Container.Resolve<IEventService>();

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
                ID = _interpret1Id,
                IsPublic = false
            };

            _interpret2Id = 2;
            _interpret2 = new InterpretDTO
            {
                Name = "Linkin Park",
                Language = Language.English,
                ID = _interpret2Id,
                IsPublic = false
            };

            _interpret3Id = 3;
            _interpret3 = new InterpretDTO
            {
                Name = "No name",
                Language = Language.Slovak,
                ID = _interpret3Id,
                IsPublic = false
            };

            _album1Id = 4;
            _album1 = new AlbumDTO
            {
                Name = "Toxicity",
                Year = 2001,
                InterpretId = _interpret1Id,
                ID = _album1Id
            };

            _album2Id = 5;
            _album2 = new AlbumDTO
            {
                Name = "Meteora",
                Year = 2003,
                InterpretId = _interpret2Id,
                ID = _album2Id
            };

            _album3Id = 6;
            _album3 = new AlbumDTO
            {
                Name = "V rovnováhe",
                Year = 2008,
                InterpretId = _interpret3Id,
                ID = _album3Id
            };

            _album4Id = 7;
            _album4 = new AlbumDTO
            {
                Name = "Čím to je?!",
                Year = 2005,
                InterpretId = _interpret3Id,
                ID = _album4Id
            };

            _event1Id = 8;
            _event1 = new EventDTO
            {
                Name = "Deň matiek v Púchove",
                Place = "Púchov Kultúrny dom",
                Time = new DateTime(2017, 5, 14),
                InterpretId = _interpret3Id,
                ID = _event1Id
            };

            _event2Id = 9;
            _event2 = new EventDTO
            {
                Name = "Vianočné trhy 2016",
                Place = "Terchová Amfiteáter",
                Time = new DateTime(2016, 12, 25),
                InterpretId = _interpret3Id,
                ID = _event2Id
            };

            _interpretService.CreateInterpret(_interpret1);
            _interpretService.CreateInterpret(_interpret2);
            _interpretService.CreateInterpret(_interpret3);

            _albumService.CreateAlbum(_album1, _interpret1Id);
            _albumService.CreateAlbum(_album2, _interpret2Id);
            _albumService.CreateAlbum(_album3, _interpret3Id);
            _albumService.CreateAlbum(_album4, _interpret3Id);

            _eventService.CreateEvent(_event1, _interpret3Id);
            _eventService.CreateEvent(_event2, _interpret3Id);
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
        public void CreateInterpretTest()
        {
            Assert.AreEqual(3, _interpretService.ListInterprets(null).Count());
        }

        [TestMethod]
        public void EditInterpretTest()
        {
            _interpretService.EditInterpret(
                new InterpretDTO {ID = _interpret1Id, Name = "EDITED", Language = Language.English}, new[] {1, 2}, 4, 5);
            var editedInterpret = _interpretService.GetInterpret(_interpret1Id);
            Assert.AreEqual("EDITED", editedInterpret.Name);
            Assert.AreEqual(Language.English, editedInterpret.Language);
        }

        [TestMethod]
        public void DeleteInterpretTest()
        {
            _interpretService.DeleteInterpret(_interpret1Id);
            Assert.IsNull(_interpretService.GetInterpret(_interpret1Id));

            _interpretService.DeleteInterpret(_interpret2Id);
            Assert.IsNull(_interpretService.GetInterpret(_interpret2Id));

            _interpretService.DeleteInterpret(_interpret3Id);
            Assert.IsNull(_interpretService.GetInterpret(_interpret3Id));
        }

        [TestMethod]
        public void GetInterpretTest()
        {
            var interpret = _interpretService.GetInterpret(_interpret1Id);

            Assert.AreEqual(_interpret1.ID, interpret.ID);
            Assert.AreEqual(_interpret1.Name, interpret.Name);
            Assert.AreEqual(_interpret1.Language, interpret.Language);
        }

        [TestMethod]
        public void GetListInterpretsByLanguageTest()
        {
            Assert.AreEqual(0,
                _interpretService.ListInterprets(new InterpretFilter {Language = (int) Language.Japanese}).Count());
            Assert.AreEqual(1,
                _interpretService.ListInterprets(new InterpretFilter {Language = (int) Language.Slovak}).Count());
            Assert.AreEqual(2,
                _interpretService.ListInterprets(new InterpretFilter {Language = (int) Language.English}).Count());
        }

        [TestMethod]
        public void GetListInterpretsByNameTest()
        {
            Assert.AreEqual(0, _interpretService.ListInterprets(new InterpretFilter {Name = "Unknown"}).Count());
            Assert.AreEqual(1, _interpretService.ListInterprets(new InterpretFilter {Name = _interpret1.Name}).Count());
        }

        [TestMethod]
        public void GetListInterpretsByContainsTest()
        {
            Assert.AreEqual(1, _interpretService.ListInterprets(new InterpretFilter {Name = "No"}).Count());
            Assert.AreEqual(1, _interpretService.ListInterprets(new InterpretFilter {Name = "park"}).Count());
        }

        [TestMethod]
        public void GetInterpretsByLanguageAndNameTest()
        {
            Assert.AreEqual(1,
                _interpretService.ListInterprets(new InterpretFilter
                {
                    Name = _interpret1.Name,
                    Language = (int) _interpret1.Language
                }).Count());
        }

        [TestMethod]
        public void ListAllInterpretsTest()
        {
            Assert.AreEqual(3, _interpretService.ListInterprets(null).Count());
        }

        [TestMethod]
        public void MakeInterpretPublic()
        {
            _interpretService.MakeInterpretPublic(new InterpretDTO
            {
                ID = _interpret1Id,
                Name = "System of a down",
                Language = Language.English,
                IsPublic = true
            });
            Assert.AreEqual(true, _interpretService.GetInterpret(_interpret1Id).IsPublic);
        }
    }
}