using System.Collections.Generic;
using System.Linq;
using BL.DTOs.Filters;
using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using BL.Services.Albums;
using BL.Services.Interprets;
using BL.Services.Songlists;
using BL.Services.SongReviews;
using BL.Services.Songs;
using BL.Utils.Shuffle;

namespace BL.Facades
{
    public class SongFacade
    {
        private readonly IAlbumService _albumService;
        private readonly IInterpretService _interpretService;
        private readonly ISonglistService _songlistService;
        private readonly ISongReviewService _songReviewService;
        private readonly ISongService _songService;

        public SongFacade(ISonglistService songlistService, ISongService songService,
            IAlbumService albumService, ISongReviewService songReviewService,
            IInterpretService interpretService)
        {
            _songlistService = songlistService;
            _songService = songService;
            _albumService = albumService;
            _songReviewService = songReviewService;
            _interpretService = interpretService;
        }

        public int CreateSong(SongDTO songDto, int albumId)
        {
            return _songService.CreateSong(songDto, albumId);
        }

        public void EditSong(SongDTO songDto, int albumId, params int[] songReviewIds)
        {
            _songService.EditSong(songDto, albumId, songReviewIds);
        }

        public void DeleteSong(int songId)
        {
            _songService.DeleteSong(songId);
        }

        public SongDTO GetSong(int songId)
        {
            return _songService.GetSong(songId);
        }

        public IEnumerable<SongDTO> ListSongs(SongFilter filter)
        {
            return _songService.ListSongs(filter);
        }

        public IEnumerable<SongDTO> ListSongsShuffle(SongFilter filter)
        {
            return _songService.ListSongsShuffle(filter);
        }

        public void CreateSonglist(SongListDTO songListDto, int songId, int playlistId)
        {
            _songlistService.CreateSonglist(songListDto, songId, playlistId);
        }

        public void EditSonglist(SongListDTO songListDto, int songId, int playlistId)
        {
            _songlistService.EditSonglist(songListDto, songId, playlistId);
        }

        public void DeleteSonglist(int songlistId)
        {
            _songlistService.DeleteSonglist(songlistId);
        }

        public SongListDTO GetSongList(int songlistId)
        {
            return _songlistService.GetSonglist(songlistId);
        }

        public IEnumerable<SongListDTO> ListSonglists(SongListFilter filter)
        {
            return _songlistService.ListSonglists(filter);
        }

        public IList<SongDTO> ListSongsByPlaylist(int playlistId)
        {
            var songLists = ListSonglists(new SongListFilter {PlaylistId = playlistId});

            return songLists.Select(song => _songService.GetSong(song.SongId)).ToList();
        }

        public IList<SongDTO> ListSongsByPlaListShuffle(int playlistId)
        {
            return ListSongsByPlaylist(playlistId).Shuffle().ToList();
        }

        public IList<SongDTO> ListSongsByInterpret(int interpretId)
        {
            var albums = _albumService.ListAlbums(new AlbumFilter {InterpretId = interpretId});
            var songs = new List<SongDTO>();

            foreach (var album in albums)
            {
                songs.AddRange(_songService.ListSongs(new SongFilter {AlbumId = album.ID}));
            }
            return songs;
        }

        public IList<SongDTO> ListSongsPublic()
        {
            return (IList<SongDTO>) ListSongs(new SongFilter {IsPublic = true});
        }

        public IList<SongDTO> ListSongsNotPublic()
        {
            return (IList<SongDTO>) ListSongs(new SongFilter {IsPublic = false});
        }

        public IList<SongDTO> ListSongsByCreatedUser(int userId)
        {
            return (IList<SongDTO>) ListSongs(new SongFilter {CreatedBy = userId});
        }

        public void PublishSong(int songId)
        {
            var song = GetSong(songId);
            song.IsPublic = true;
            song.Publish = false;
            EditSong(song, song.AlbumId);
        }

        public IList<SongManagementDTO> ListSongBeforeUpdate()
        {
            var updateSongs = ListSongs(new SongFilter {OriginalSongId = int.MinValue});
            var beforeUpdateIds = updateSongs.Select(updateSong => updateSong.OriginalSongId).ToList();

            var beforeUpdateDtos = new List<SongManagementDTO>();

            foreach (var songId in beforeUpdateIds)
            {
                var song = GetSong(songId);
                var album = _albumService.GetAlbum(song.AlbumId);

                beforeUpdateDtos.Add(new SongManagementDTO
                {
                    Name = song.Name,
                    ID = song.ID,
                    Album = album.Name,
                    Genre = song.Genre,
                    YTLink = song.YTLink,
                    Interpret = _interpretService.GetInterpret(album.InterpretId).Name,
                    SongPath = song.SongPath
                });
            }

            return beforeUpdateDtos;
        }

        public IList<SongManagementDTO> ListSongEditedUnconfirmed()
        {
            return
                ConvertSongToSongManagement(ListSongs(new SongFilter {OriginalSongId = int.MinValue, IsPublic = true}));
        }


        public IList<SongManagementDTO> ListSongsForPublish()
        {
            return ConvertSongToSongManagement(ListSongs(new SongFilter {Publish = true}));
        }

        private IList<SongManagementDTO> ConvertSongToSongManagement(IEnumerable<SongDTO> songDtos)
        {
            return (from song in songDtos
                let album = _albumService.GetAlbum(song.AlbumId)
                select new SongManagementDTO
                {
                    Name = song.Name,
                    ID = song.ID,
                    Album = album.Name,
                    Genre = song.Genre,
                    YTLink = song.YTLink,
                    Interpret = _interpretService.GetInterpret(album.InterpretId).Name,
                    OriginalSong = song.OriginalSongId,
                    SongPath = song.SongPath
                }).ToList();
        }

        public void ConfirmEditedSong(int songId)
        {
            var editedVersion = GetSong(songId);

            var song = GetSong(editedVersion.OriginalSongId);
            song.Genre = editedVersion.Genre;
            song.YTLink = editedVersion.YTLink;
            song.Name = editedVersion.Name;
            song.OriginalSongId = int.MinValue;
            song.SongPath = editedVersion.SongPath;

            var reviews = _songReviewService.ListSongReviews(new SongReviewFilter {SongId = song.ID});

            var reviewIds = reviews.Select(a => a.ID).ToArray();

            EditSong(song, song.AlbumId, reviewIds);
            DeleteSong(editedVersion.ID);
        }

        public void EditSongUnconfirmed(SongDTO songDto, int albumId, params int[] songReviewIds)
        {
            var song = new SongDTO
            {
                AlbumId = songDto.AlbumId,
                IsPublic = false,
                Added = songDto.Added,
                Genre = songDto.Genre,
                YTLink = songDto.YTLink,
                Name = songDto.Name,
                CreatedBy = songDto.CreatedBy,
                OriginalSongId = songDto.OriginalSongId,
                SongPath = songDto.SongPath
            };

            CreateSong(song, albumId);
        }

        public void CancelRequestForPublish(int songId)
        {
            var song = GetSong(songId);
            song.Publish = false;
            EditSong(song, song.AlbumId);
        }

        public void AskForPublication(int songId)
        {
            var song = GetSong(songId);
            song.Publish = true;
            var reviews = _songReviewService.ListSongReviews(new SongReviewFilter {SongId = song.ID});

            var reviewIds = reviews.Select(a => a.ID).ToArray();

            EditSong(song, song.AlbumId, reviewIds);
        }
    }
}