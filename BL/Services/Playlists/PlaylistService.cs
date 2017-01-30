using System;
using System.Collections.Generic;
using AutoMapper;
using BL.DTOs.Filters;
using BL.DTOs.Playlists;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;

namespace BL.Services.Playlists
{
    public class PlaylistService : MusicLibraryService, IPlaylistService
    {

        #region Dependencies

        private readonly PlaylistListQuery playlistListQuery;

        private readonly PlaylistRepository playlistRepository;

        private readonly UserRepository userRepository;

        public PlaylistService(PlaylistListQuery playlistListQuery, PlaylistRepository playlistRepository,
            UserRepository userRepository)
        {
            this.playlistListQuery = playlistListQuery;
            this.playlistRepository = playlistRepository;
            this.userRepository = userRepository;
        }

        #endregion

        public void CreateInitPlaylist(int userId)
        {
            var initPlaylist = new PlaylistDTO
            {
                Name = "All my songs",
                Created = DateTime.Now
            };

            using (var uow = UnitOfWorkProvider.Create())
            {
                var mappedPlaylist = Mapper.Map<Playlist>(initPlaylist);
                mappedPlaylist.User = GetPlaylistUser(userId);
                playlistRepository.Insert(mappedPlaylist);
                uow.Commit();
            }
        }


        public void CreatePlaylist(PlaylistDTO playlistDto, int userId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var playlist = Mapper.Map<Playlist>(playlistDto);
                playlist.User = GetPlaylistUser(userId);
                playlistRepository.Insert(playlist);
                uow.Commit();
            }
        }

        public void EditPlaylist(PlaylistDTO playlistDto, int userId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var playlist = playlistRepository.GetById(playlistDto.ID);
                Mapper.Map(playlistDto, playlist);
                playlist.User = GetPlaylistUser(userId);
                playlistRepository.Update(playlist);
                uow.Commit();
            }
        }

        public void DeletePlaylist(int playlistId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                playlistRepository.Delete(playlistId);
                uow.Commit();
            }
        }

        public PlaylistDTO GetPlaylist(int playlistId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var playlist = playlistRepository.GetById(playlistId);
                return playlist != null ? Mapper.Map<PlaylistDTO>(playlist) : null;
            }
        }

        public IEnumerable<PlaylistDTO> ListPlaylists(PlaylistFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                playlistListQuery.Filter = filter;
                return playlistListQuery.Execute() ?? new List<PlaylistDTO>();
            }
        }

        private User GetPlaylistUser(int userId)
        {
            var user = userRepository.GetById(userId);
            if (user == null)
            {
                throw new NullReferenceException("Playlist service - CreatePlaylist(...) user cant be null");
            }
            return user;
        }

    }
}