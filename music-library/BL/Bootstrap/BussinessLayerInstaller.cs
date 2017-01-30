using System;
using System.Data.Entity;
using BL.AppInfrastructure;
using BL.Repositories.UserAccount;
using BL.Services;
using BL.Services.UserAccounts;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DAL;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Bootstrap
{
    public class BussinessLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Func<DbContext>>()
                    .Instance(() => new MusicLibraryDbContext())
                    .LifestyleTransient(),
                Component.For<IUnitOfWorkProvider>()
                    .ImplementedBy<AppUnitOfWorkProvider>()
                    .LifestyleSingleton(),
                Component.For<IUnitOfWorkRegistry>()
                    .Instance(new HttpContextUnitOfWorkRegistry(new ThreadLocalUnitOfWorkRegistry()))
                    .LifestyleSingleton(),
                Component.For<BrockAllen.MembershipReboot.IUserAccountRepository<UserAccount>>()
                    .ImplementedBy<UserAccountManager>()
                    .LifestyleTransient(),
                Component.For<BrockAllen.MembershipReboot.UserAccountService<UserAccount>>()
                    .ImplementedBy<BrockAllen.MembershipReboot.UserAccountService<UserAccount>>()
                    .DependsOn(
                        Dependency
                            .OnComponent
                            <BrockAllen.MembershipReboot.IUserAccountRepository<UserAccount>, UserAccountManager>())
                    .LifestyleTransient(),
                Component.For<AuthenticationWrapper>()
                    .ImplementedBy<AuthenticationWrapper>()
                    .DependsOn(
                        Dependency
                            .OnComponent
                            <BrockAllen.MembershipReboot.UserAccountService<UserAccount>,
                                BrockAllen.MembershipReboot.UserAccountService<UserAccount>>())
                    .LifestyleTransient(),
                Component.For(typeof (IRepository<,>))
                    .ImplementedBy(typeof (EntityFrameworkRepository<,>))
                    .LifestyleTransient(),
                Classes.FromAssemblyContaining<AppUnitOfWork>()
                    .BasedOn(typeof (AppQuery<>))
                    .LifestyleTransient(),
                Classes.FromAssemblyContaining<AppUnitOfWork>()
                    .BasedOn(typeof (IRepository<,>))
                    .LifestyleTransient(),
                Classes.FromThisAssembly()
                    .BasedOn<MusicLibraryService>()
                    .WithServiceDefaultInterfaces()
                    .LifestyleTransient(),
                Classes.FromThisAssembly()
                    .InNamespace("BL.Facades")
                    .LifestyleTransient()
                );
        }
    }
}