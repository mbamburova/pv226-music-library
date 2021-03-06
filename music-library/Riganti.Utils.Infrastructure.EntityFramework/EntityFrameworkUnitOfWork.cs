using System;
using System.Data.Entity;
using System.Diagnostics;
using Riganti.Utils.Infrastructure.Core;

namespace Riganti.Utils.Infrastructure.EntityFramework
{
    /// <summary>
    ///     An implementation of unit of work in Entity ramework.
    /// </summary>
    public class EntityFrameworkUnitOfWork : UnitOfWorkBase
    {
        private readonly bool hasOwnContext;


        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityFrameworkUnitOfWork" /> class.
        /// </summary>
        public EntityFrameworkUnitOfWork(IUnitOfWorkProvider provider, Func<DbContext> dbContextFactory,
            DbContextOptions options)
        {
            if (options == DbContextOptions.ReuseParentContext)
            {
                var parentUow = provider.GetCurrent() as EntityFrameworkUnitOfWork;
                if (parentUow != null)
                {
                    Context = parentUow.Context;
                    return;
                }
            }

            Context = dbContextFactory();
            hasOwnContext = true;
        }

        /// <summary>
        ///     Gets the <see cref="DbContext" />.
        /// </summary>
        public DbContext Context { get; }


        /// <summary>
        ///     Commits this instance when we have to.
        /// </summary>
        public override void Commit()
        {
            if (hasOwnContext)
            {
                base.Commit();
            }
        }

        /// <summary>
        ///     Commits the changes.
        /// </summary>
        protected override void CommitCore()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An exception was thrown while performing SaveChanges():" + ex.Message);
            }
        }

        /// <summary>
        ///     Disposes the context.
        /// </summary>
        protected override void DisposeCore()
        {
            if (hasOwnContext)
            {
                Context.Dispose();
            }
        }

        /// <summary>
        ///     Tries to get the <see cref="DbContext" /> in the current scope.
        /// </summary>
        public static DbContext TryGetDbContext(IUnitOfWorkProvider provider)
        {
            var uow = provider.GetCurrent() as EntityFrameworkUnitOfWork;
            if (uow == null)
            {
                throw new InvalidOperationException(
                    "The EntityFrameworkRepository must be used in a unit of work of type EntityFrameworkUnitOfWork!");
            }
            return uow.Context;
        }
    }
}