using System;
using System.Collections.Generic;
using BL.DTOs.Filters;
using BL.DTOs.Playlists;
using BL.DTOs.Reviews;
using BL.DTOs.User;

namespace BL.Services.Users
{
    public interface IUserService
    {
        void CreateUser(Guid userAccountId);

        void EditUser(UserDTO userDto);

        UserDTO GetUser(int userId);

        IEnumerable<UserDTO> ListUsers();

        IEnumerable<AlbumReviewDTO> ListAlbumReviews(AlbumReviewFilter filter);

        IEnumerable<SongReviewDTO> ListSongReviews(SongReviewFilter filter);

        IEnumerable<PlaylistDTO> ListPlaylists(PlaylistFilter filter);

        UserDTO GetUserAccordingToEmail(string email);
    }
}