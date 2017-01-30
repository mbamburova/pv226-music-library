using System;
using System.Collections.Generic;
using System.Linq;
using BL.Bootstrap;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.DTOs.Songs;
using BL.Services.Albums;
using BL.Services.Interprets;
using BL.Services.Songs;
using Castle.Windsor;
using DAL;
using DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTests.Services.Songs
{
    [TestClass]
    public class SongServiceTests
    {
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

        private AlbumDTO _album1;
        private AlbumDTO _album2;

        private InterpretDTO _interpret1;
        private InterpretDTO _interpret2;

        private SongDTO _song1;
        private SongDTO _song2;
        private SongDTO _song3;

        [ClassInitialize]
        public static void ClassInit(TestContext context1)
        {
            Container.Install(new BussinessLayerInstaller());
            MappingInit.ConfigureMapping();
            _albumService = Container.Resolve<IAlbumService>();
            _songService = Container.Resolve<ISongService>();
            _interpretService = Container.Resolve<IInterpretService>();

            DeleteTables();
        }

        [TestInitialize]
        public void Init()
        {
            _interpret1 = new InterpretDTO
            {
                Name = "System of a down",
                Language = Language.English,
                ID = 1,
                IsPublic = false
            };

            _interpret2 = new InterpretDTO
            {
                Name = "Linkin Park",
                Language = Language.English,
                ID = 2,
                IsPublic = false
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
                Genre = Genre.Rock,
                IsPublic = false
            };

            _song2Id = 2;
            _song2 = new SongDTO
            {
                ID = _song2Id,
                Name = "Deer Dance",
                Added = new DateTime(2016, 11, 6),
                AlbumId = _album1Id,
                Genre = Genre.Rock,
                IsPublic = false
            };

            _song3Id = 3;
            _song3 = new SongDTO
            {
                ID = _song3Id,
                Name = "Numb",
                Added = new DateTime(2016, 11, 6),
                AlbumId = _album2Id,
                Genre = Genre.Rock,
                IsPublic = false
            };

            _songService.CreateSong(_song1, _album1Id);
            _songService.CreateSong(_song2, _album1Id);
            _songService.CreateSong(_song3, _album2Id);
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
        public void CreateSongTest()
        {
            Assert.AreEqual(3, _songService.ListSongs(null).Count());

            var interpret3 = new InterpretDTO
            {
                Name = "Coldplay",
                Language = Language.English,
                ID = 3,
                IsPublic = false
            };
            _interpretService.CreateInterpret(interpret3);

            var album3 = new AlbumDTO
            {
                ID = 3,
                InterpretId = interpret3.ID,
                Name = "Ghost stories",
                Year = 2014
            };
            _albumService.CreateAlbum(album3, interpret3.ID);

            var song4 = new SongDTO
            {
                ID = 4,
                Name = "A sky full of stars",
                Added = new DateTime(2016, 11, 6),
                AlbumId = album3.ID,
                Genre = Genre.Pop,
                IsPublic = false
            };
            _songService.CreateSong(song4, album3.ID);

            Assert.AreEqual(4, _songService.ListSongs(null).Count());
        }

        [TestMethod]
        public void GetSongsByAlbumTest()
        {
            var songs = _songService.ListSongs(new SongFilter {AlbumId = _album1Id});
            Assert.AreEqual(2, songs.Count());
        }

        [TestMethod]
        public void GetSongsByGenreTest()
        {
            var songs2 = _songService.ListSongs(new SongFilter {Genre = (int) Genre.Alternative});
            Assert.AreEqual(0, songs2.Count());
        }

        [TestMethod]
        public void GetSongsByNameTest()
        {
            var songs3 = _songService.ListSongs(new SongFilter {Name = "Numb"});
            Assert.AreEqual(1, songs3.Count());
        }

        [TestMethod]
        public void GetSongsByContainsTest()
        {
            var songs3 = _songService.ListSongs(new SongFilter {Name = "n"});
            Assert.AreEqual(3, songs3.Count());
        }

        [TestMethod]
        public void GetSongsByGenreAndAlbumTest()
        {
            var songs4 = _songService.ListSongs(new SongFilter {Genre = (int) Genre.Rock, AlbumId = _album1Id});
            Assert.AreEqual(2, songs4.Count());
        }


        [TestMethod]
        public void EditSongTest()
        {
            _songService.EditSong(
                new SongDTO
                {
                    ID = _song3Id,
                    AlbumId = _album2Id,
                    Name = "NumbEDIT",
                    Genre = Genre.Alternative,
                    Added = new DateTime(2016, 11, 1)
                }, _album2Id);
            var editedSong = _songService.GetSong(_song3Id);
            Assert.AreEqual("NumbEDIT", editedSong.Name);
            Assert.AreEqual(Genre.Alternative, editedSong.Genre);
            Assert.AreEqual(new DateTime(2016, 11, 1), editedSong.Added);
        }

        [TestMethod]
        public void DeleteSongTest()
        {
            _songService.DeleteSong(_song3Id);
            _songService.DeleteSong(_song2Id);
            Assert.IsNull(_songService.GetSong(_song3Id));
            Assert.IsNull(_songService.GetSong(_song2Id));
        }

        [TestMethod]
        public void GetSongTest()
        {
            Assert.AreEqual(_song1.ID, _songService.GetSong(_song1Id).ID);
            Assert.AreEqual(_song2.Name, _songService.GetSong(_song2Id).Name);

            Assert.AreEqual(_song2.ID, _songService.GetSong(_song2Id).ID);
            Assert.AreEqual(_song3.Genre, _songService.GetSong(_song3Id).Genre);
        }

        [TestMethod]
        public void ListSongsTest()
        {
            var songs = _songService.ListSongs(null);
            Assert.AreEqual(3, songs.Count());
        }

        [TestMethod]
        public void MakeSongPublic()
        {
            _songService.MakeSongPublic(new SongDTO
            {
                ID = _song1Id,
                Name = "Prison Song",
                Added = new DateTime(2016, 11, 6),
                AlbumId = _album1Id,
                Genre = Genre.Rock,
                IsPublic = true
            }, _album1Id);
            Assert.AreEqual(true, _songService.GetSong(_song1Id).IsPublic);
        }
    }
}