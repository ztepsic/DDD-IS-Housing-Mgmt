using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.MembershipAndRoles;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.BuildingMaintenance;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.BuildingManagement;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.Finances;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.Legislature;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.MembershipAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.Services;
using NHibernate;
using Ninject.Modules;

[assembly: WebActivator.PreApplicationStartMethod(typeof(ISHousingMgmt.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(ISHousingMgmt.Web.App_Start.NinjectWebCommon), "Stop")]

namespace ISHousingMgmt.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel) {
			kernel.Load(new ISHousingMgmtModule());
        }

		class ISHousingMgmtModule : NinjectModule {
			public override void Load() {
				Bind<ISessionFactory>().ToConstant(NHibernateSessionProvider.SessionFactory);
				Bind<ICitiesRepository>().To<CitiesNHRepository>();
				Bind<IRepairServicesRepository>().To<RepairServicesNHRepository>();
				Bind<IContractorsRepository>().To<ContractorsNHRepository>();
				Bind<IBuildingManagersRepository>().To<BuildingManagersNHRepository>();
				Bind<IPersonsRepository>().To<PersonsNHRepository>();
				Bind<ICadastresRepository>().To<CadastresNHRepository>();
				Bind<ILandRegistriesRepository>().To<LandRegistriesNHRepository>();
				Bind<IPartitionSpacesRepository>().To<PartitionSpacesNHRepository>();
				Bind<IBuildingsRepository>().To<BuildingsNHRepository>();
				Bind<IAdminJobsVotingsRepository>().To<AdminJobsVotingsNHRepository>();
				Bind<IRolesRepository>().To<RolesNHRepository>();
				Bind<IHousingMgmtUsersRepository>().To<HousingMgmtUsersNHRepository>();
				Bind<IMaintenancesRepository>().To<MaintenancesNHRepository>();
				Bind<IReservesRepository>().To<ReservesNHRepository>();
				Bind<IBillsRepository>().To<BillsNHRepository>();
				Bind<IEmailNotifier>().To<EmailNotifier>();
			}
		}
    }
}
