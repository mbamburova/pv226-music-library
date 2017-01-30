using System;
using System.Collections.Generic;
using System.Linq;
using BL.Bootstrap;
using BL.DTOs.Filters;
using BL.DTOs.Playlists;
using BL.Services.Playlists;
using BL.Services.Users;
using Castle.Windsor;
using DAL;
using DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTests.Services.Playlists
{
    [TestClass]
    public class PlaylistServiceTests
    {
        private static IPlaylistService _playlistService;
        private static IUserService _userService;
        private static BrockAllen.MembershipReboot.UserAccountService<UserAccount> _userAccountService;

        private static int _playlist1Id;
        private static int _playlist2Id;

        private static readonly MusicLibraryDbContext Context = new MusicLibraryDbContext();
        public static readonly IWindsorContainer Container = new WindsorContainer();

        private PlaylistDTO _playlist1;
        private PlaylistDTO _playlist2;


        [ClassInitialize]
        public static void ClassInit(TestContext context1)
        {
            Container.Install(new BussinessLayerInstaller());
            MappingInit.ConfigureMapping();
            _playlistService = Container.Resolve<IPlaylistService>();
            _userService = Container.Resolve<IUserService>();
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
                Name = "Christmas songs",
                Created = new DateTime(2016, 1, 1),
                ID = _playlist2Id,
                UserId = 1
            };

            _playlistService.CreatePlaylist(_playlist1, 1);
            _playlistService.CreatePlaylist(_playlist2, 1);
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
        public void CreatePlaylistTest()
        {
            Assert.AreEqual(2, _playlistService.ListPlaylists(null).Count());

            var playlist = new PlaylistDTO
            {
                Name = "My playlist",
                Created = new DateTime(2016, 11, 1),
                UserId = 1
            };

            _playlistService.CreatePlaylist(playlist, 1);

            Assert.AreEqual(3, _playlistService.ListPlaylists(null).Count());
        }

        [TestMethod]
        public void EditPlaylistTest()
        {
            const string editedName = "EDITED NAME";
            var editedCreated = new DateTime(2020, 10, 10);

            _playlistService.EditPlaylist(
                new PlaylistDTO {ID = _playlist1Id, Name = editedName, Created = editedCreated, UserId = 1},
                1);

            var editedPlaylist = _playlistService.GetPlaylist(_playlist1Id);

            Assert.AreEqual(editedName, editedPlaylist.Name);
            Assert.AreEqual(editedCreated, editedPlaylist.Created);
            Assert.AreEqual(1, editedPlaylist.UserId);
        }

        [TestMethod]
        public void DeletePlaylistTest()
        {
            _playlistService.DeletePlaylist(_playlist1Id);
            Assert.IsNull(_playlistService.GetPlaylist(_playlist1Id));

            _playlistService.DeletePlaylist(_playlist2Id);
            Assert.IsNull(_playlistService.GetPlaylist(_playlist2Id));
        }

        [TestMethod]
        public void GetPlaylistTest()
        {
            var playlist = _playlistService.GetPlaylist(_playlist1Id);

            Assert.AreEqual(_playlist1.ID, playlist.ID);
            Assert.AreEqual(_playlist1.Name, playlist.Name);
            Assert.AreEqual(_playlist1.Created, playlist.Created);
            Assert.AreEqual(_playlist1.UserId, playlist.UserId);
        }

        [TestMethod]
        public void ListPlaylistByUserTest()
        {
            Assert.AreEqual(2, _playlistService.ListPlaylists(new PlaylistFilter {UserId = 1}).Count());
            Assert.AreEqual(0, _playlistService.ListPlaylists(new PlaylistFilter {UserId = int.MaxValue}).Count());
        }

        [TestMethod]
        public void ListPlaylistByNameTest()
        {
            Assert.AreEqual(1, _playlistService.ListPlaylists(new PlaylistFilter {Name = _playlist1.Name}).Count());
            Assert.AreEqual(0, _playlistService.ListPlaylists(new PlaylistFilter {Name = "Unknown"}).Count());
        }

        [TestMethod]
        public void ListPlaylistsTest()
        {
            Assert.AreEqual(2, _playlistService.ListPlaylists(null).Count());
        }
    }
}