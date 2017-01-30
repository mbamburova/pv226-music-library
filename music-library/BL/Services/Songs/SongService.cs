using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BL.DTOs.Filters;
using BL.DTOs.Songs;
using BL.Queries;
using BL.Repositories;
using BL.Utils.Shuffle;
using DAL.Entities;

namespace BL.Services.Songs
{
    public class SongService : MusicLibraryService, ISongService
    {
        #region Dependencies

        private readonly SongRepository _songRepository;
        private readonly SongListQuery _songListQuery;
        private readonly AlbumRepository _albumRepository;
        private readonly SongReviewRepository _songReviewRepository;

        public SongService(SongRepository songRepository, SongListQuery songListQuery, AlbumRepository albumRepository,
            SongReviewRepository songReviewRepository)
        {
            _songRepository = songRepository;
            _songListQuery = songListQuery;
            _albumRepository = albumRepository;
            _songReviewRepository = songReviewRepository;
        }

        #endregion 

        public int CreateSong(SongDTO songDto, int albumId)
        {
            Song song;
            using (var uow = UnitOfWorkProvider.Create())
            {
                song = Mapper.Map<Song>(songDto);
                song.Album = GetSongAlbum(albumId);
                _songRepository.Insert(song);
                uow.Commit();
            }
            return song.ID;
        }

        public void EditSong(SongDTO songDto, int albumId, params int[] songReviewIds)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var song = _songRepository.GetById(songDto.ID);
                Mapper.Map(songDto, song);

                song.Album = GetSongAlbum(albumId);

                if (songReviewIds != null && songReviewIds.Any())
                {
                    var songReviews = _songReviewRepository.GetByIds(songReviewIds);
                    song.SongReviews.RemoveAll(review => !songReviews.Contains(review));
                    song.SongReviews.AddRange(
                        songReviews.Where(review => !song.SongReviews.Contains(review)));
                }
                else
                {
                    song.SongReviews.Clear();
                }
                _songRepository.Update(song);
                uow.Commit();
            }
        }

        public void DeleteSong(int songId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _songRepository.Delete(songId);
                uow.Commit();
            }
        }

        public SongDTO GetSong(int songId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var song = _songRepository.GetById(songId);
                return song != null ? Mapper.Map<SongDTO>(song) : null;
            }
        }

        public IEnumerable<SongDTO> ListSongs(SongFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _songListQuery.Filter = filter;

                return _songListQuery.Execute() ?? new List<SongDTO>();
            }
        }

        public IEnumerable<SongDTO> ListSongsShuffle(SongFilter filter)
        {
            return ListSongs(filter).Shuffle();
        }

        public void MakeSongPublic(SongDTO songDto, int albumId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var song = _songRepository.GetById(songDto.ID);
                Mapper.Map(songDto, song);
                song.Album = GetSongAlbum(albumId);
                _songRepository.Update(song);
                uow.Commit();
            }
        }

        private Album GetSongAlbum(int albumId)
        {
            var album = _albumRepository.GetById(albumId);
            if (album == null)
            {
                throw new NullReferenceException("Song service - CreateSong(...) album cant be null");
            }
            return album;
        }

        private Song GetSongOriginal(int songId)
        {
            var song = _songRepository.GetById(songId);
            return song;
        }


    }
}