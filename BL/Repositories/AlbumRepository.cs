using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class AlbumRepository : EntityFrameworkRepository<Album, int>
    {
        public AlbumRepository(IUnitOfWorkProvider provider) : base(provider) {}
    }
}