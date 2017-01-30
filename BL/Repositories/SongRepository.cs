using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class SongRepository : EntityFrameworkRepository<Song, int>
    {
        public SongRepository(IUnitOfWorkProvider provider) : base(provider) {}
    }
}