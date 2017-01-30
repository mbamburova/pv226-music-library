using System;
using System.Collections.Generic;
using System.Linq;
using BL.Bootstrap;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.DTOs.Playlists;
using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using BL.Services.Albums;
using BL.Services.Interprets;
using BL.Services.Playlists;
using BL.Services.Songlists;
using BL.Services.Songs;
using BL.Services.Users;
using Castle.Windsor;
using DAL;
using DAL.Entities;
using DAL.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTests.Services.Songlists
{
    [TestClass]
    public class SonglistServiceTests
    {
        private static readonly MusicLibraryDbContext Context = new MusicLibraryDbContext();
        public static readonly IWindsorContainer Container = new WindsorContainer();

        private static ISonglistService _songlistService;
        private static ISongService _songService;
        private static IPlaylistService _playlistService;
        private static IUserService _userService;
        private static IInterpretService _interpretService;
        private static IAlbumService _albumService;
        private static BrockAllen.MembershipReboot.UserAccountService<UserAccount> _userAccountService;

        private static int _song1Id;
        private static int _song2Id;
        private static int _song3Id;

        private static int _album1Id;
        private static int _album2Id;

        private static int _playlist1Id;
        private static int _playlist2Id;


        private static int _songlist1Id;
        private static int _songlist2Id;
        private static int _songlist3Id;

        private AlbumDTO _album1;
        private AlbumDTO _album2;

        private InterpretDTO _interpret1;
        private InterpretDTO _interpret2;

        private PlaylistDTO _playlist1;
        private PlaylistDTO _playlist2;

        private SongDTO _song1;
        private SongDTO _song2;
        private SongDTO _song3;

        private SongListDTO _songList1;
        private SongListDTO _songList2;
        private SongListDTO _songList3;


        [ClassInitialize]
        public static void ClassInit(TestContext context1)
        {
            Container.Install(new BussinessLayerInstaller());
            MappingInit.ConfigureMapping();
            _playlistService = Container.Resolve<IPlaylistService>();
            _userService = Container.Resolve<IUserService>();
            _songService = Container.Resolve<ISongService>();
            _songlistService = Container.Resolve<ISonglistService>();
            _interpretService = Container.Resolve<IInterpretService>();
            _albumService = Container.Resolve<IAlbumService>();
            _userAccountService = Container.Resolve<BrockAllen.MembershipReboot.UserAccountService<UserAccount>>();

            DeleteTables();
        }

        [TestInitialize]
        public void Init()
        {
            var guid = _userAccountService.CreateAccount("Martin", "Password", "410452@mail.muni.cz");
            _userService.CreateUser(guid.ID);

            _playlist1Id = 1;
            _playlist1 = new PlaylistDTO
            {
                Name = "Favorite songs",
                Created = new DateTime(2016, 10, 10),
                ID = _playlist1Id,
                UserId = 1
            };

            _playlist2Id = 2;
            _playlist2 = new PlaylistDTO
            {
                Name = "Best songs",
                Created = new DateTime(2016, 1, 1),
                ID = _playlist2Id,
                UserId = 1
            };

            _playlistService.CreatePlaylist(_playlist1, 1);
            _playlistService.CreatePlaylist(_playlist2, 1);

            _interpret1 = new InterpretDTO
            {
                Name = "System of a down",
                Language = Language.English,
                ID = 1
            };

            _interpret2 = new InterpretDTO
            {
                Name = "Linkin Park",
                Language = Language.English,
                ID = 2
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

            _songlist1Id = 1;
            _songList1 = new SongListDTO
            {
                ID = _songlist1Id,
                PlaylistId = _playlist1Id,
                SongId = _song1Id
            };

            _songlist2Id = 2;
            _songList2 = new SongListDTO
            {
                ID = _songlist2Id,
                PlaylistId = _playlist1Id,
                SongId = _song2Id
            };

            _songlist3Id = 3;
            _songList3 = new SongListDTO
            {
                ID = _songlist3Id,
                PlaylistId = _playlist2Id,
                SongId = _song3Id
            };

            _songlistService.CreateSonglist(_songList1, _song1Id, _playlist1Id);
            _songlistService.CreateSonglist(_songList2, _song1Id, _playlist1Id);
            _songlistService.CreateSonglist(_songList3, _song1Id, _playlist2Id);
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
        public void CreateSonglistTest()
        {
            Assert.AreEqual(3, _songlistService.ListSonglists(null).Count());

            var songlist = new SongListDTO
            {
                PlaylistId = _playlist2Id,
                SongId = _song2Id
            };

            _songlistService.CreateSonglist(songlist, _song2Id, _playlist2Id);

            Assert.AreEqual(4, _songlistService.ListSonglists(null).Count());
        }

        [TestMethod]
        public void EditSonglistTest()
        {
            _songlistService.EditSonglist(
                new SongListDTO {ID = _songlist1Id, SongId = _song3Id, PlaylistId = _playlist2Id}, _song3Id,
                _playlist2Id);

            var editedSonglist = _songlistService.GetSonglist(_songlist1Id);

            Assert.AreEqual(_song3Id, editedSonglist.SongId);
            Assert.AreEqual(_playlist2Id, editedSonglist.PlaylistId);
        }

        [TestMethod]
        public void DeleteSonglistTest()
        {
            _songlistService.DeleteSonglist(_songlist1Id);
            Assert.IsNull(_songlistService.GetSonglist(_songlist1Id));

            _songlistService.DeleteSonglist(_songlist2Id);
            Assert.IsNull(_songlistService.GetSonglist(_songlist2Id));

            _songlistService.DeleteSonglist(_songlist3Id);
            Assert.IsNull(_songlistService.GetSonglist(_songlist3Id));
        }

        [TestMethod]
        public void GetSongListTest()
        {
            var songlist = _songlistService.GetSonglist(_songlist1Id);

            Assert.AreEqual(_songList1.SongId, songlist.SongId);
            Assert.AreEqual(_songList1.PlaylistId, songlist.PlaylistId);
        }

        [TestMethod]
        public void ListSonglistTest()
        {
            Assert.AreEqual(3, _songlistService.ListSonglists(null).Count());
        }

        [TestMethod]
        public void ListSongListByPlaylist()
        {
            Assert.AreEqual(2, _songlistService.ListSonglists(new SongListFilter {PlaylistId = _playlist1Id}).Count());
        }
    }
}