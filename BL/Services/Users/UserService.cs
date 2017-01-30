using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BL.DTOs.Filters;
using BL.DTOs.Playlists;
using BL.DTOs.Reviews;
using BL.DTOs.User;
using BL.Queries;
using BL.Repositories;
using BL.Repositories.UserAccount;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services.Users
{
    public class UserService : MusicLibraryService, IUserService
    {
        #region Dependencies

        private readonly UserAccountRepository _userAccountRepository;

        private readonly UserRepository _userRepository;

        private readonly UserListQuery _userListQuery;

        private readonly PlaylistListQuery _playlistListQuery;

        private readonly UserAccordingToEmailQuery _userAccordingToEmailQuery;

        private readonly AlbumReviewListQuery _albumReviewListQuery;

        private readonly SongReviewListQuery _songReviewListQuery;

        public UserService(UserAccountRepository _userAccountRepository, UserRepository _userRepository,
            UserListQuery _userListQuery, PlaylistListQuery _playlistListQuery,
            UserAccordingToEmailQuery _userAccordingToEmailQuery,
            AlbumReviewListQuery _albumReviewListQuery, SongReviewListQuery _songReviewListQuery)
        {
            this._userAccountRepository = _userAccountRepository;
            this._userRepository = _userRepository;
            this._userListQuery = _userListQuery;
            this._playlistListQuery = _playlistListQuery;
            this._userAccordingToEmailQuery = _userAccordingToEmailQuery;
            this._albumReviewListQuery = _albumReviewListQuery;
            this._songReviewListQuery = _songReviewListQuery;
        }

        #endregion

        public void CreateUser(Guid userGuid)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var userAccount = _userAccountRepository.GetById(userGuid);
                var user = new User {Account = userAccount};
                _userRepository.Insert(user);

                uow.Commit();
            }
        }


        public void EditUser(UserDTO userDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var user = _userRepository.GetById(userDto.ID);
                Mapper.Map(userDto, user);

                _userRepository.Update(user);

                uow.Commit();
            }
        }

        public UserDTO GetUser(int userId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var user = _userRepository.GetById(userId, c => c.Account);
                return user != null ? Mapper.Map<UserDTO>(user) : null;
            }
        }

        public IEnumerable<UserDTO> ListUsers()
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = GetUserQuery();

                query.AddSortCriteria(user => user.LastName);

                return _userListQuery.Execute();
            }
        }

        public IEnumerable<AlbumReviewDTO> ListAlbumReviews(AlbumReviewFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _albumReviewListQuery.Filter = filter;

                var sortAlbumReviews = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                _albumReviewListQuery.AddSortCriteria("Rating", sortAlbumReviews);
                return _albumReviewListQuery.Execute();
            }
        }

        public IEnumerable<SongReviewDTO> ListSongReviews(SongReviewFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _songReviewListQuery.Filter = filter;

                var sortSongReviews = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                _albumReviewListQuery.AddSortCriteria("Rating", sortSongReviews);
                return _songReviewListQuery.Execute();
            }
        }

        public IEnumerable<PlaylistDTO> ListPlaylists(PlaylistFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _playlistListQuery.Filter = filter;

                return _playlistListQuery.Execute();
            }
        }

        public UserDTO GetUserAccordingToEmail(string email)
        {
            using (UnitOfWorkProvider.Create())
            {
                _userAccordingToEmailQuery.Email = email;
                return _userAccordingToEmailQuery.Execute().SingleOrDefault();
            }
        }

        private IQuery<UserDTO> GetUserQuery()
        {
            var query = _userListQuery;
            query.ClearSortCriterias();
            return query;
        }


    }
}