using System;
using System.Collections.Generic;
using System.Linq;
using BL.Bootstrap;
using BL.DTOs.Events;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.Services.Events;
using BL.Services.Interprets;
using Castle.Windsor;
using DAL;
using DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTests.Services.Events
{
    [TestClass]
    public class EventServiceTests
    {
        private static IEventService _eventService;
        private static IInterpretService _interpretService;

        private static int _interpret1Id;

        private static int _event1Id;
        private static int _event2Id;

        private static readonly MusicLibraryDbContext Context = new MusicLibraryDbContext();
        public static readonly IWindsorContainer Container = new WindsorContainer();

        private EventDTO _event1;
        private EventDTO _event2;

        private InterpretDTO _interpret1;

        [ClassInitialize]
        public static void ClassInit(TestContext context1)
        {
            Container.Install(new BussinessLayerInstaller());
            MappingInit.ConfigureMapping();
            _eventService = Container.Resolve<IEventService>();
            _interpretService = Container.Resolve<IInterpretService>();

            DeleteTables();
        }

        [TestInitialize]
        public void Init()
        {
            _interpret1Id = 1;
            _interpret1 = new InterpretDTO
            {
                Name = "No name",
                Language = Language.Slovak,
                ID = _interpret1Id
            };

            _event1Id = 1;
            _event1 = new EventDTO
            {
                Name = "Deň matiek v Púchove",
                Place = "Púchov Kultúrny dom",
                Time = new DateTime(2017, 5, 14),
                InterpretId = _interpret1Id,
                ID = _event1Id
            };

            _event2Id = 2;
            _event2 = new EventDTO
            {
                Name = "Vianočné trhy 2016",
                Place = "Terchová Amfiteáter",
                Time = new DateTime(2016, 12, 25),
                InterpretId = _interpret1Id,
                ID = _event2Id
            };

            _interpretService.CreateInterpret(_interpret1);

            _eventService.CreateEvent(_event1, _interpret1Id);
            _eventService.CreateEvent(_event2, _interpret1Id);
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
        public void CreateEventTest()
        {
            Assert.AreEqual(2, _eventService.ListEvents(new EventFilter {SortAscending = true}).Count());

            var e = new EventDTO
            {
                Name = "Imatrikulácie",
                Place = "FI MUNI",
                Time = new DateTime(2017, 11, 1),
                InterpretId = _interpret1Id
            };

            _eventService.CreateEvent(e, _interpret1Id);

            Assert.AreEqual(3, _eventService.ListEvents(new EventFilter {SortAscending = true}).Count());
        }

        [TestMethod]
        public void SortTest()
        {
            Assert.AreEqual(_event2Id, _eventService.ListEvents(new EventFilter {SortAscending = true}).First().ID);

            var e = new EventDTO
            {
                Name = "Imatrikulácie",
                Place = "FI MUNI",
                Time = new DateTime(2017, 11, 1),
                InterpretId = _interpret1Id
            };

            _eventService.CreateEvent(e, _interpret1Id);

            Assert.AreEqual(e.Name, _eventService.ListEvents(new EventFilter {SortAscending = false}).First().Name);
        }


        [TestMethod]
        public void EditEventTest()
        {
            const string editedName = "EDITED NAME";
            const string editedPlace = "EDITED PLACE";
            var editedTime = new DateTime(2020, 5, 14);

            _eventService.EditEvent(
                new EventDTO {ID = _event1Id, Name = editedName, Place = editedPlace, Time = editedTime}, _interpret1Id);
            var editedEvent = _eventService.GetEvent(_event1Id);

            Assert.AreEqual(editedName, editedEvent.Name);
            Assert.AreEqual(editedPlace, editedEvent.Place);
            Assert.AreEqual(editedTime, editedEvent.Time);
        }

        [TestMethod]
        public void DeleteEventTest()
        {
            _eventService.DeleteEvent(_event1Id);
            Assert.IsNull(_eventService.GetEvent(_event1Id));

            _eventService.DeleteEvent(_event2Id);
            Assert.IsNull(_eventService.GetEvent(_event2Id));
        }

        [TestMethod]
        public void GetEventTest()
        {
            var e = _eventService.GetEvent(_event2Id);

            Assert.AreEqual(_event2.ID, e.ID);
            Assert.AreEqual(_event2.Name, e.Name);
            Assert.AreEqual(_event2.Place, e.Place);
            Assert.AreEqual(_event2.Time, e.Time);
        }

        [TestMethod]
        public void ListEventsByInterpretTest()
        {
            Assert.AreEqual(2,
                _eventService.ListEvents(new EventFilter {InterpretId = _interpret1Id, SortAscending = true}).Count());
            Assert.AreEqual(0, _eventService.ListEvents(new EventFilter {InterpretId = 10}).Count());
        }

        [TestMethod]
        public void ListEventsByPlaceTest()
        {
            Assert.AreEqual(1, _eventService.ListEvents(new EventFilter {Place = _event1.Place}).Count());
            Assert.AreEqual(0, _eventService.ListEvents(new EventFilter {Place = "Unknown"}).Count());
        }

        [TestMethod]
        public void ListEventsByContainsTest()
        {
            Assert.AreEqual(2, _eventService.ListEvents(new EventFilter {Place = "v"}).Count());
            Assert.AreEqual(1, _eventService.ListEvents(new EventFilter {Place = "Pú"}).Count());
        }

        [TestMethod]
        public void ListEventsByNameTest()
        {
            Assert.AreEqual(1, _eventService.ListEvents(new EventFilter {Name = _event1.Name}).Count());
            Assert.AreEqual(0, _eventService.ListEvents(new EventFilter {Name = "Unknown"}).Count());
        }

        [TestMethod]
        public void ListEventsByNameAndInterpretTest()
        {
            Assert.AreEqual(1,
                _eventService.ListEvents(new EventFilter {Name = _event1.Name, InterpretId = _event1.InterpretId})
                    .Count());
            Assert.AreEqual(0, _eventService.ListEvents(new EventFilter {Name = "Unknown", InterpretId = null}).Count());
        }

        [TestMethod]
        public void ListAllEventsTest()
        {
            Assert.AreEqual(2, _eventService.ListEvents(new EventFilter {SortAscending = true}).Count());
        }
    }
}