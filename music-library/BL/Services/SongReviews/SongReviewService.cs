using System;
using System.Collections.Generic;
using AutoMapper;
using BL.DTOs.Filters;
using BL.DTOs.Reviews;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;

namespace BL.Services.SongReviews
{
    public class SongReviewService : MusicLibraryService, ISongReviewService
    {
        #region Dependencies

        private readonly SongReviewRepository _songReviewRepository;
        private readonly SongReviewListQuery _songReviewListQuery;
        private readonly UserRepository _userRepository;
        private readonly SongRepository _songRepository;

        public SongReviewService(SongReviewRepository songReviewRepository, SongReviewListQuery songReviewListQuery,
            UserRepository userRepository, SongRepository songRepository)
        {
            _songReviewRepository = songReviewRepository;
            _songReviewListQuery = songReviewListQuery;
            _userRepository = userRepository;
            _songRepository = songRepository;
        }

        #endregion

        public void CreateSongReview(SongReviewDTO songReviewDto, int songId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var songReview = Mapper.Map<SongReview>(songReviewDto);
                songReview.Song = GetSongReviewSong(songId);
                _songReviewRepository.Insert(songReview);
                uow.Commit();
            }
        }

        public void EditSongReview(SongReviewDTO songReviewDto, int songId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var songReview = _songReviewRepository.GetById(songReviewDto.ID);
                Mapper.Map(songReviewDto, songReview);
                songReview.Song = GetSongReviewSong(songId);
                _songReviewRepository.Update(songReview);
                uow.Commit();
            }
        }

        public void DeleteSongReview(int songReviewId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _songReviewRepository.Delete(songReviewId);
                uow.Commit();
            }
        }

        public SongReviewDTO GetSongReview(int songReviewId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var songReview = _songReviewRepository.GetById(songReviewId);
                return songReview != null ? Mapper.Map<SongReviewDTO>(songReview) : null;
            }
        }

        public IEnumerable<SongReviewDTO> ListSongReviews(SongReviewFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _songReviewListQuery.Filter = filter;
                return _songReviewListQuery.Execute() ?? new List<SongReviewDTO>();
            }
        }

        private User GetSongReviewUser(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new NullReferenceException("SongReview service - CreateSongReview(...) user cant be null");
            }
            return user;
        }

        private Song GetSongReviewSong(int songId)
        {
            var song = _songRepository.GetById(songId);
            if (song == null)
            {
                throw new NullReferenceException("SongReview service - CreateSongReview(...) song cant be null");
            }
            return song;
        }


    }
}