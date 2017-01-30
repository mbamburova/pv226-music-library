using Riganti.Utils.Infrastructure.Core;

namespace BL.Services
{
    public abstract class MusicLibraryService
    {
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }
    }
}