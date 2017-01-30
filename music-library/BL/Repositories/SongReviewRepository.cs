using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class SongReviewRepository : EntityFrameworkRepository<SongReview, int>
    {
        public SongReviewRepository(IUnitOfWorkProvider provider) : base(provider) {}
    }
}