using System.Collections.Generic;
using BL.DTOs.Filters;
using BL.DTOs.Interprets;

namespace BL.Services.Interprets
{
    public interface IInterpretService
    {
        int CreateInterpret(InterpretDTO interpretDto);

        void EditInterpret(InterpretDTO interpretDto, int[] albumIds, params int[] eventIds);

        void DeleteInterpret(int interpretId);

        InterpretDTO GetInterpret(int interpretId);

        IEnumerable<InterpretDTO> ListInterprets(InterpretFilter interpretFilter);

        void MakeInterpretPublic(InterpretDTO interpretDto);
    }
}