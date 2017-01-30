using System;
using System.Data.Entity;
using System.Linq;
using BrockAllen.MembershipReboot.Ef;
using DAL;

namespace BL.Repositories.UserAccount
{
    public class UserAccountManager : DbContextUserAccountRepository<MusicLibraryDbContext, DAL.Entities.UserAccount>
    {
        public UserAccountManager(Func<DbContext> dbContextFactory)
            : base(dbContextFactory.Invoke() as MusicLibraryDbContext) {}

        protected override IQueryable<DAL.Entities.UserAccount> DefaultQueryFilter(
            IQueryable<DAL.Entities.UserAccount> query, string claimFilter)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (claimFilter == null)
            {
                throw new ArgumentNullException(nameof(claimFilter));
            }

            return query.SelectMany(account => account.ClaimCollection, (account, claims) => new {account, claims})
                .Where(t => t.claims.Value.Contains(claimFilter))
                .Select(t => t.account);
        }
    }
}