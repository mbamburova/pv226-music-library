using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class InterpretRepository : EntityFrameworkRepository<Interpret, int>
    {
        public InterpretRepository(IUnitOfWorkProvider provider) : base(provider) {}
    }
}