using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class PlaylistRepository : EntityFrameworkRepository<Playlist, int>
    {
        public PlaylistRepository(IUnitOfWorkProvider provider) : base(provider) {}
    }
}