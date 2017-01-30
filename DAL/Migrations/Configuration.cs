using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using DAL.Entities;
using DAL.Enums;

namespace DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MusicLibraryDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "DAL.MusicLibraryDbContext";
        }

        protected override void Seed(MusicLibraryDbContext context)
        {
            var listOfTables = new List<string>
            {
                "AlbumReviews",
                "Albums",
                "Events",
                "Interprets",
                "LinkedAccountClaims",
                "LinkedAccounts",
                "PasswordResetSecrets",
                "Playlists",
                "SongLists",
                "SongReviews",
                "Songs",
                "TwoFactorAuthTokens",
                "UserAccounts",
                "UserCertificates",
                "UserClaims",
                "Users"
            };

            foreach (var tableName in listOfTables)
            {
                context.Database.ExecuteSqlCommand("DELETE FROM [" + tableName + "]" + "DBCC CHECKIDENT (" + tableName +
                                                   ",RESEED, 0)");
            }

            context.SaveChanges();

            var interpret1 = new Interpret
            {
                Name = "System of a down",
                Language = Language.English,
                InterpretImgUri = @"\Content\Images\Interprets\System-of-a-Down.jpg"
            };

            var interpret2 = new Interpret
            {
                Name = "Linkin Park",
                Language = Language.English,
                InterpretImgUri = @"\Content\Images\Interprets\linkin_park_photo.jpg"
            };


            var interpret3 = new Interpret
            {
                Name = "Twenty one Pilots",
                Language = Language.English,
                InterpretImgUri = @"\Content\Images\Interprets\Twenty-One-Pilots_ok-620x400.jpg"
            };

            var interpret4 = new Interpret
            {
                Name = "JP Cooper",
                Language = Language.English,
                InterpretImgUri = @"\Content\Images\Interprets\jpCooper.jpg"
            };

            var album1 = new Album
            {
                Interpret = interpret1,
                Name = "Toxicity",
                Year = 2000
            };

            var album2 = new Album
            {
                Interpret = interpret2,
                Name = "Meteora",
                Year = 2000
            };

            var album3 = new Album
            {
                Interpret = interpret1,
                Name = "Unknown"
            };

            var album4 = new Album
            {
                Interpret = interpret2,
                Name = "Unknown"
            };

            var album5 = new Album
            {
                Interpret = interpret3,
                Name = "Unknown"
            };

            var album6 = new Album
            {
                Interpret = interpret4,
                Name = "Unknown"
            };

            var song1 = new Song
            {
                Name = "Toxicity",
                Added = new DateTime(2014, 6, 8),
                Album = album1,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/iywaBOMvYLI",
                SongPath = @"\Content\Songs\SystemOfADown-Toxicity.mp3"
            };

            var song2 = new Song
            {
                Name = "Prison Song",
                Added = new DateTime(2008, 8, 25),
                Album = album1,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/yndfqN1VKhY",
                SongPath = @"\Content\Songs\SystemOfADown-Prison Song.mp3"
            };

            var song3 = new Song
            {
                Name = "Deer Dance",
                Added = new DateTime(2008, 9, 9),
                Album = album1,
                Genre = Genre.Rock,
                IsPublic = false,
                Publish = true,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/BkkOQjK71Ss",
                SongPath = @"\Content\Songs\SystemofADown-DeerDance.mp3"
            };

            var song4 = new Song
            {
                Name = "Numb",
                Added = new DateTime(2010, 6, 7),
                Album = album2,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/kXYiU_JCYtU",
                SongPath = @"\Content\Songs\LinkinPark-Numb.mp3"
            };

            var song5 = new Song
            {
                Name = "Castle of glass",
                Added = new DateTime(2004, 4, 9),
                Album = album2,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/ScNNfyq3d_w",
                SongPath = @"\Content\Songs\LinkinPark-CastleofGlass.mp3"
            };

            var song6 = new Song
            {
                Name = "Don't Stay",
                Added = new DateTime(2013, 2, 15),
                Album = album2,
                Genre = Genre.Rock,
                IsPublic = false,
                Publish = true,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/Nk3u7PuYOS0",
                SongPath = @"\Content\Songs\LinkinPark-DontStay.mp3"
            };

            var song7 = new Song
            {
                Name = "Somewhere I belong",
                Added = new DateTime(2016, 8, 9),
                Album = album2,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/zsCD5XCu6CM",
                SongPath = @"\Content\Songs\LinkinPark-SomewhereIBelong.mp3"
            };

            var song8 = new Song
            {
                Name = "Lying From You",
                Added = new DateTime(2015, 4, 16),
                Album = album2,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/1V4FLUOlMks",
                SongPath = @"\Content\Songs\LinkinPark-LyingFromYou.mp3"
            };

            var song9 = new Song
            {
                Name = "Hit The Floor",
                Added = new DateTime(2011, 11, 5),
                Album = album2,
                Genre = Genre.Rock,
                IsPublic = false,
                Publish = true,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/oMals9XXQY8",
                SongPath = @"\Content\Songs\LinkinPark-HitTheFloor.mp3"
            };

            var song10 = new Song
            {
                Name = "Needles",
                Added = new DateTime(2012, 8, 29),
                Album = album1,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/1UP8GrqmjMk",
                SongPath = @"\Content\Songs\SystemOfADown-Neddles.mp3"
            };

            var song11 = new Song
            {
                Name = "Jet Pilot",
                Added = new DateTime(2009, 8, 10),
                Album = album1,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/KR4Jye73e_o",
                SongPath = @"\Content\Songs\SystemOfADown-Jet Pilot.mp3"
            };

            var song12 = new Song
            {
                Name = "Chop Suey!",
                Added = new DateTime(2014, 3, 16),
                Album = album1,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/CSvFpBOe8eY",
                SongPath = @"\Content\Songs\SystemofADown-ChopSuey.mp3"
            };

            var song13 = new Song
            {
                Name = "Bounce",
                Added = new DateTime(2016, 8, 5),
                Album = album1,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/lITuHDdhdkw",
                SongPath = @"\Content\Songs\SystemOfADown-Bounce.mp3"
            };

            var song14 = new Song
            {
                Name = "Forest",
                Added = new DateTime(2015, 1, 1),
                Album = album1,
                Genre = Genre.Rock,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/xgP_2zf0NV8",
                SongPath = @"\Content\Songs\SystemOfADown-Forest.mp3"
            };

            var song15 = new Song
            {
                Name = "Satellite",
                Added = new DateTime(2015, 10, 2),
                Album = album6,
                Genre = Genre.Alternative,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/TeJgnno39d4",
                SongPath = @"\Content\Songs\JPCooper-Satellite.mp3"
            };

            var song16 = new Song
            {
                Name = "September Song",
                Added = new DateTime(2016, 12, 12),
                Album = album6,
                Genre = Genre.Romance,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/9ORWF5RkdO0",
                SongPath = @"\Content\Songs\JPCooper-SeptemberSong.mp3"
            };

            var song17 = new Song
            {
                Name = "Stressed Out",
                Added = new DateTime(2016, 1, 1),
                Album = album5,
                Genre = Genre.Alternative,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/pXRviuL6vMY",
                SongPath = @"\Content\Songs\TwentyOnePilots-StressedOut.mp3"
            };


            var song18 = new Song
            {
                Name = "Heathens",
                Added = new DateTime(2016, 7, 8),
                Album = album5,
                Genre = Genre.Alternative,
                IsPublic = true,
                Publish = false,
                OriginalSongId = int.MinValue,
                CreatedBy = int.MinValue,
                YTLink = "https://www.youtube.com/embed/UprcpdwuwCg",
                SongPath = @"\Content\Songs\TwentyOnePilots-Heathens.mp3"
            };

            var event1 = new Event
            {
                Interpret = interpret1,
                Name = "Festival Brno",
                Place = "Brno",
                Time = new DateTime(2017, 6, 6)
            };

            var event2 = new Event
            {
                Interpret = interpret3,
                Name = "Twenty one pilots with Bry",
                Place = "Wiener Stadthalle (Vienna)",
                Time = new DateTime(2017, 12, 12)
            };

            var event3 = new Event
            {
                Interpret = interpret4,
                Name = "JP Cooper Concert",
                Place = "O2 Academy Oxford",
                Time = new DateTime(2016, 10, 26)
            };

            var songReview1 = new SongReview
            {
                Note = "Perfect",
                Name = "John",
                Rating = 8,
                Song = song1
            };

            var albumReview1 = new AlbumReview
            {
                Album = album2,
                Name = "Nicole",
                Note = "Perfect album",
                Rating = 9
            };

            context.Interprets.AddOrUpdate(interpret1);
            context.Interprets.AddOrUpdate(interpret2);
            context.Interprets.AddOrUpdate(interpret3);
            context.Interprets.AddOrUpdate(interpret4);

            context.Albums.AddOrUpdate(album1);
            context.Albums.AddOrUpdate(album2);
            context.Albums.AddOrUpdate(album3);
            context.Albums.AddOrUpdate(album4);
            context.Albums.AddOrUpdate(album5);
            context.Albums.AddOrUpdate(album6);

            context.Songs.AddOrUpdate(song1);
            context.Songs.AddOrUpdate(song2);
            context.Songs.AddOrUpdate(song3);
            context.Songs.AddOrUpdate(song4);

            context.Songs.AddOrUpdate(song5);
            context.Songs.AddOrUpdate(song6);
            context.Songs.AddOrUpdate(song7);
            context.Songs.AddOrUpdate(song8);

            context.Songs.AddOrUpdate(song9);
            context.Songs.AddOrUpdate(song10);
            context.Songs.AddOrUpdate(song11);
            context.Songs.AddOrUpdate(song12);

            context.Songs.AddOrUpdate(song13);
            context.Songs.AddOrUpdate(song14);
            context.Songs.AddOrUpdate(song15);
            context.Songs.AddOrUpdate(song16);

            context.Songs.AddOrUpdate(song17);
            context.Songs.AddOrUpdate(song18);

            context.Events.AddOrUpdate(event1);
            context.Events.AddOrUpdate(event2);
            context.Events.AddOrUpdate(event3);
            context.SongReviews.AddOrUpdate(songReview1);
            context.AlbumReviews.AddOrUpdate(albumReview1);

            context.SaveChanges();
        }
    }
}