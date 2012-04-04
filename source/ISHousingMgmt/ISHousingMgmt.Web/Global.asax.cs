using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ISHousingMgmt.Infrastructure.DataAccess;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using ISHousingMgmt.Web.App_Start;
using NHibernate;
using NHibernate.Cfg;

namespace ISHousingMgmt.Web {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication {
		public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"",
				"",
				new { controller = "account", action = "login"}
			);

			routes.MapRoute(
				"",
				"buildingmanager/bills/{id}",
				new { controller = "buildingmanager", action = "bills", Id = 0 }
			);

			routes.MapRoute(
				"",
				"finances/reservebills/{id}/{date}",
				new { controller = "finances", action = "reservebills" }
			);

			routes.MapRoute(
				"",
				"dashboard/contractor/",
				new { controller = "contractor", action = "index" }
			);

			routes.MapRoute(
				"",
				"dashboard/owner/",
				new { controller = "owner", action = "index" }
			);

			routes.MapRoute(
				"",
				"dashboard/representative/",
				new { controller = "buildingmanagement", action = "index" }
			);

			routes.MapRoute(
				"",
				"dashboard/buildingmanager/",
				new { controller = "buildingmanagement", action = "index" }
			);

			routes.MapRoute(
				"",
				"legislature/addpartitionspace/{id}/{owned}",
				new { controller="legislature", action = "addpartitionspace", owned = UrlParameter.Optional }
			);

			routes.MapRoute(
				"",
				"legislature/create/{buildingId}/",
				new { controller = "legislature", action = "create" }
			);

			routes.MapRoute(
				"",
				"account/register/owner",
				new { controller = "account", action = "registerowner" }
			);

			routes.MapRoute(
				"",
				"account/register/contractor",
				new { controller = "account", action = "registercontractor" }
			);

			routes.MapRoute(
				"",
				"account/register/manager",
				new { controller = "account", action = "registermanager" }
			);

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "account", action = "index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			log4net.Config.XmlConfigurator.Configure();
			NHibernateSessionProvider.Init();

			AutoMapperBootstrapper.Bootstrap();
			NHibernateProfilerBootstrapper.PreStart();
		}

		protected void Application_BeginRequest() {
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("hr-HR");
		}

	}
}