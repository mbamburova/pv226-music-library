using System.Data.Entity;
using Riganti.Utils.Infrastructure.Core;

namespace Riganti.Utils.Infrastructure.EntityFramework
{
    /// <summary>
    ///     A base implementation of query object in Entity Framework.
    /// </summary>
    public abstract class EntityFrameworkQuery<TResult> : QueryBase<TResult>
    {
        private readonly IUnitOfWorkProvider provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityFrameworkQuery{TResult}" /> class.
        /// </summary>
        public EntityFrameworkQuery(IUnitOfWorkProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        ///     Gets the <see cref="DbContext" />.
        /// </summary>
        protected DbContext Context
        {
            get { return EntityFrameworkUnitOfWork.TryGetDbContext(provider); }
        }
    }
}