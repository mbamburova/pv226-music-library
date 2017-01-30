using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;

namespace BL.Services.Albums
{
    public class AlbumService : MusicLibraryService, IAlbumService
    {

        #region Dependencies

        private readonly AlbumRepository _albumRepository;
        private readonly AlbumListQuery _albumListQuery;
        private readonly InterpretRepository _interpretRepository;
        private readonly AlbumReviewRepository _albumReviewRepository;
        private readonly SongRepository _songRepository;

        public AlbumService(AlbumRepository albumRepository, AlbumListQuery albumListQuery,
            InterpretRepository interpretRepository, AlbumReviewRepository albumReviewRepository,
            SongRepository songRepository)
        {
            _albumRepository = albumRepository;
            _albumListQuery = albumListQuery;
            _interpretRepository = interpretRepository;
            _albumReviewRepository = albumReviewRepository;
            _songRepository = songRepository;
        }

        #endregion
        public void CreateAlbum(AlbumDTO albumDto, int interpretId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var album = Mapper.Map<Album>(albumDto);
                album.Interpret = GetAlbumInterpret(interpretId);
                _albumRepository.Insert(album);
                uow.Commit();
            }
        }

        public void EditAlbum(AlbumDTO albumDto, int interpretId, int[] songIds, int[] albumReviewIds)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var album = _albumRepository.GetById(albumDto.ID);
                Mapper.Map(albumDto, album);

                album.Interpret = GetAlbumInterpret(interpretId);

                if (albumReviewIds != null && albumReviewIds.Any())
                {
                    var albumReviews = _albumReviewRepository.GetByIds(albumReviewIds);
                    album.AlbumReviews.RemoveAll(review => !albumReviews.Contains(review));
                    album.AlbumReviews.AddRange(
                        albumReviews.Where(review => !album.AlbumReviews.Contains(review)));
                }
                else
                {
                    album.AlbumReviews.Clear();
                }

                if (songIds != null && songIds.Any())
                {
                    var songs = _songRepository.GetByIds(songIds);
                    album.Songs.RemoveAll(song => !songs.Contains(song));
                    album.Songs.AddRange(
                        songs.Where(song => !album.Songs.Contains(song)));
                }
                else
                {
                    album.Songs.Clear();
                }

                _albumRepository.Update(album);
                uow.Commit();
            }
        }

        public void DeleteAlbum(int albumId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _albumRepository.Delete(albumId);
                uow.Commit();
            }
        }

        public AlbumDTO GetAlbum(int albumId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var album = _albumRepository.GetById(albumId);
                return album != null ? Mapper.Map<AlbumDTO>(album) : null;
            }
        }

        public IEnumerable<AlbumDTO> ListAlbums(AlbumFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _albumListQuery.Filter = filter;
                return _albumListQuery.Execute() ?? new List<AlbumDTO>();
            }
        }

        public void MakeAlbumPublic(AlbumDTO albumDto, int interpretId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var album = _albumRepository.GetById(albumDto.ID);
                Mapper.Map(albumDto, album);
                album.Interpret = GetAlbumInterpret(interpretId);
                _albumRepository.Update(album);
                uow.Commit();
            }
        }

        private Interpret GetAlbumInterpret(int interpretId)
        {
            var interpret = _interpretRepository.GetById(interpretId);
            if (interpret == null)
            {
                throw new NullReferenceException("Album service - CreateAlbum(...) interpret cant be null");
            }
            return interpret;
        }

  
    }
}