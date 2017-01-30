using System.Collections.Generic;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;
using BL.Services.Interprets;

namespace BL.Facades
{
    public class InterpretFacade
    {
        private readonly IInterpretService _interpretService;

        public InterpretFacade(IInterpretService interpretService)
        {
            _interpretService = interpretService;
        }

        public int CreateInterpret(InterpretDTO interpretDto)
        {
            return _interpretService.CreateInterpret(interpretDto);
        }

        public void EditInterpret(InterpretDTO interpretDto, int[] albumIds, int[] eventIds)
        {
            _interpretService.EditInterpret(interpretDto, albumIds, eventIds);
        }

        public void DeleteInterpret(int interpretId)
        {
            _interpretService.DeleteInterpret(interpretId);
        }

        public InterpretDTO GetInterpret(int interpretId)
        {
            return _interpretService.GetInterpret(interpretId);
        }

        public IEnumerable<InterpretDTO> ListInterprets(InterpretFilter filter)
        {
            return _interpretService.ListInterprets(filter);
        }
    }
}