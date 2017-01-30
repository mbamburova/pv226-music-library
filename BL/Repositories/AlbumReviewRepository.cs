using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class AlbumReviewRepository : EntityFrameworkRepository<AlbumReview, int>
    {
        public AlbumReviewRepository(IUnitOfWorkProvider provider) : base(provider) {}
    }
}