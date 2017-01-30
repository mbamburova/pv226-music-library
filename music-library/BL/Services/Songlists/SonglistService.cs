using System;
using System.Collections.Generic;
using AutoMapper;
using BL.DTOs.Filters;
using BL.DTOs.Songlists;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;

namespace BL.Services.Songlists
{
    public class SonglistService : MusicLibraryService, ISonglistService
    {
        #region Dependencies

        private readonly SonglistRepository _songlistRepository;

        private readonly SonglistListQuery _songlistListQuery;

        private readonly SongRepository _songRepository;

        private readonly PlaylistRepository _playlistRepository;

        public SonglistService(SonglistRepository songlistRepository, SonglistListQuery songlistListQuery,
            SongRepository songRepository, PlaylistRepository playlistRepository)
        {
            _songlistRepository = songlistRepository;
            _songlistListQuery = songlistListQuery;
            _songRepository = songRepository;
            _playlistRepository = playlistRepository;
        }

        #endregion

        public void CreateSonglist(SongListDTO songListDto, int songId, int playlistId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var songlist = Mapper.Map<SongList>(songListDto);
                songlist.Playlist = GetSonglistPlaylist(playlistId);
                songlist.Song = GetSonglistSong(songId);
                _songlistRepository.Insert(songlist);
                uow.Commit();
            }
        }

        public void EditSonglist(SongListDTO songListDto, int songId, int playlistId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var songlist = _songlistRepository.GetById(songListDto.ID);
                Mapper.Map(songListDto, songlist);
                songlist.Playlist = GetSonglistPlaylist(playlistId);
                songlist.Song = GetSonglistSong(songId);
                _songlistRepository.Update(songlist);
                uow.Commit();
            }
        }

        public void DeleteSonglist(int songlistId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _songlistRepository.Delete(songlistId);
                uow.Commit();
            }
        }

        public SongListDTO GetSonglist(int songlistId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var songlist = _songlistRepository.GetById(songlistId);
                return songlist != null ? Mapper.Map<SongListDTO>(songlist) : null;
            }
        }

        public IEnumerable<SongListDTO> ListSonglists(SongListFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _songlistListQuery.Filter = filter;
                return _songlistListQuery.Execute() ?? new List<SongListDTO>();
            }
        }

        private Playlist GetSonglistPlaylist(int playlistId)
        {
            var playlist = _playlistRepository.GetById(playlistId);
            if (playlist == null)
            {
                throw new NullReferenceException("Songlist service - GetSonglistSong(...) song cant be null");
            }
            return playlist;
        }

        private Song GetSonglistSong(int songId)
        {
            var song = _songRepository.GetById(songId);
            if (song == null)
            {
                throw new NullReferenceException("Songlist service - GetSonglistSong(...) song cant be null");
            }
            return song;
        }


    }
}