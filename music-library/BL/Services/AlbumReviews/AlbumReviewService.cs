using System;
using System.Collections.Generic;
using AutoMapper;
using BL.DTOs.Filters;
using BL.DTOs.Reviews;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services.AlbumReviews
{
    public class AlbumReviewService : MusicLibraryService, IAlbumReviewService
    {

        #region Dependencies

        private readonly AlbumReviewRepository _albumReviewRepository;
        private readonly AlbumReviewListQuery _albumReviewListQuery;
        private readonly UserRepository _userRepository;
        private readonly AlbumRepository _albumRepository;

        public AlbumReviewService(AlbumReviewRepository albumReviewRepository, AlbumReviewListQuery albumReviewListQuery,
            UserRepository userRepository, AlbumRepository albumRepository)
        {
            _albumReviewRepository = albumReviewRepository;
            _albumReviewListQuery = albumReviewListQuery;
            _userRepository = userRepository;
            _albumRepository = albumRepository;
        }

        #endregion
        public void CreateAlbumReview(AlbumReviewDTO albumReviewDto, int albumId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var albumReview = Mapper.Map<AlbumReview>(albumReviewDto);
                albumReview.Album = GetAlbumReviewAlbum(albumId);
                _albumReviewRepository.Insert(albumReview);
                uow.Commit();
            }
        }

        public void EditAlbumReview(AlbumReviewDTO albumReviewDto, int albumId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var albumReview = _albumReviewRepository.GetById(albumReviewDto.ID);
                Mapper.Map(albumReviewDto, albumReview);
                albumReview.Album = GetAlbumReviewAlbum(albumId);
                _albumReviewRepository.Update(albumReview);
                uow.Commit();
            }
        }

        public void DeleteAlbumReview(int albumReviewId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _albumReviewRepository.Delete(albumReviewId);
                uow.Commit();
            }
        }

        public AlbumReviewDTO GetAlbumReview(int albumReviewId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var albumReview = _albumReviewRepository.GetById(albumReviewId);
                return albumReview != null ? Mapper.Map<AlbumReviewDTO>(albumReview) : null;
            }
        }

        public IEnumerable<AlbumReviewDTO> ListAlbumReviews(AlbumReviewFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = GetQuery(filter);

                _albumReviewListQuery.Filter = filter;
                var sortAlbumReviews = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                query.AddSortCriteria("Rating", sortAlbumReviews);
                return query.Execute() ?? new List<AlbumReviewDTO>();
            }
        }

        private User GetAlbumReviewUser(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new NullReferenceException("AlbumReview service - CreateAlbumReview(...) user cant be null");
            }
            return user;
        }

        private Album GetAlbumReviewAlbum(int albumId)
        {
            var album = _albumRepository.GetById(albumId);
            if (album == null)
            {
                throw new NullReferenceException("AlbumReview service - CreateAlbumReview(...) album cant be null");
            }
            return album;
        }

        private IQuery<AlbumReviewDTO> GetQuery(AlbumReviewFilter filter)
        {
            var query = _albumReviewListQuery;
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }

   
    }
}