using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.PersonsAndRoles {
	public class BuildingManagersNHRepository : NHibernateCrudRepository<BuildingManager>, IBuildingManagersRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public BuildingManagersNHRepository() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public BuildingManagersNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca upravitelja na temelju pravne osobe
		/// </summary>
		/// <param name="legalPerson">pravna osoba za koju se pretpostavlja da je upravitelj</param>
		/// <returns>upravitelj</returns>
		public BuildingManager GetByLegalPerson(LegalPerson legalPerson) {
			return Session.QueryOver<BuildingManager>()
				.Where(bm => bm.LegalPerson == legalPerson)
				.SingleOrDefault();
		}


		#endregion

	}
}
