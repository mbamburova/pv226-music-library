using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories.UserAccount
{
    public class UserAccountRepository :
        EntityFrameworkRepository<DAL.Entities.UserAccount, Guid>
    {
        public UserAccountRepository(IUnitOfWorkProvider provider) : base(provider) {}
    }
}