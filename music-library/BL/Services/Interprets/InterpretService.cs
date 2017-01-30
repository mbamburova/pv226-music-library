using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;

namespace BL.Services.Interprets
{
    public class InterpretService : MusicLibraryService, IInterpretService
    {
        #region Dependencies

        private readonly InterpretRepository _interpretRepository;

        private readonly InterpretListQuery _interpretListQuery;

        private readonly AlbumRepository _albumRepository;

        private readonly EventRepository _eventRepository;


        public InterpretService(InterpretRepository interpretRepository, InterpretListQuery interpretListQuery,
            AlbumRepository albumRepository, EventRepository eventRepository)
        {
            _interpretRepository = interpretRepository;
            _interpretListQuery = interpretListQuery;
            _albumRepository = albumRepository;
            _eventRepository = eventRepository;
        }

        #endregion

        public int CreateInterpret(InterpretDTO interpretDto)
        {
            Interpret interpret;

            using (var uow = UnitOfWorkProvider.Create())
            {
                interpret = Mapper.Map<Interpret>(interpretDto);
                _interpretRepository.Insert(interpret);
                uow.Commit();
            }
            return interpret.ID;
        }

        public void EditInterpret(InterpretDTO interpretDto, int[] albumIds, int[] eventIds)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var interpret = _interpretRepository.GetById(interpretDto.ID);
                Mapper.Map(interpretDto, interpret);

                if (albumIds != null)
                {
                    if (albumIds.Length != 0)
                    {
                        var albums = _albumRepository.GetByIds(albumIds);
                        interpret.Albums.RemoveAll(album => !albums.Contains(album));
                        interpret.Albums.AddRange(albums.Where(album => !interpret.Albums.Contains(album)));
                    }
                    else
                    {
                        interpret.Albums.Clear();
                    }
                }
                if (eventIds != null)
                {
                    if (eventIds.Length != 0)
                    {
                        var events = _eventRepository.GetByIds(eventIds);
                        interpret.Events.RemoveAll(e => !events.Contains(e));
                        interpret.Events.AddRange(events.Where(e => !interpret.Events.Contains(e)));
                    }
                    else
                    {
                        interpret.Events.Clear();
                    }
                }
                _interpretRepository.Update(interpret);
                uow.Commit();
            }
        }

        public void DeleteInterpret(int interpretId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _interpretRepository.Delete(interpretId);
                uow.Commit();
            }
        }

        public InterpretDTO GetInterpret(int interpretId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var interpret = _interpretRepository.GetById(interpretId);
                return interpret != null ? Mapper.Map<InterpretDTO>(interpret) : null;
            }
        }

        public IEnumerable<InterpretDTO> ListInterprets(InterpretFilter filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                _interpretListQuery.Filter = filter;
                return _interpretListQuery.Execute() ?? new List<InterpretDTO>();
            }
        }

        public void MakeInterpretPublic(InterpretDTO interpretDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var interpret = _interpretRepository.GetById(interpretDto.ID);
                Mapper.Map(interpretDto, interpret);
                _interpretRepository.Update(interpret);
                uow.Commit();
            }
        }


    }
}