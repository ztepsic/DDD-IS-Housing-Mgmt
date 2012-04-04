using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.BuildingManagement {
	public class AdminJobsVotingsNHRepository : NHibernateCrudRepository<AdministrationJobsVoting>, IAdminJobsVotingsRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public AdminJobsVotingsNHRepository() { }


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public AdminJobsVotingsNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }
		
		#endregion

		#region Methods


		/// <summary>
		/// Dohvaca sve poslove uprave za zadanu zgradu
		/// </summary>
		/// <param name="building">zgrada</param>
		/// <returns>poslovi uprave</returns>
		public IList<AdministrationJobsVoting> GetByBuilding(Building building) {
			return Session.QueryOver<AdministrationJobsVoting>()
				.Where(ajv => ajv.Building == building)
				.OrderBy(ajv => ajv.StartDateTime).Desc
				.List();
		}

		/// <summary>
		/// Dohvaca odreden broj poslova (najnovije dodane) uprave za zadanu zgradu
		/// </summary>
		/// <param name="building">zgrada</param>
		/// <param name="limit">broj poslova koji se dohvaca</param>
		/// <returns>poslovi uprave</returns>
		public IList<AdministrationJobsVoting> GetByBuilding(Building building, int limit) {
			return Session.QueryOver<AdministrationJobsVoting>()
				.Where(ajv => ajv.Building == building)
				.OrderBy(ajv => ajv.StartDateTime).Desc
				.Take(limit)
				.List();
		}

		#endregion

	}
}
