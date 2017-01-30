using DAL;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.AppInfrastructure
{
    public abstract class AppQuery<T> : EntityFrameworkQuery<T>
    {
        protected AppQuery(IUnitOfWorkProvider provider) : base(provider) {}

        public new MusicLibraryDbContext Context => (MusicLibraryDbContext) base.Context;
    }
}