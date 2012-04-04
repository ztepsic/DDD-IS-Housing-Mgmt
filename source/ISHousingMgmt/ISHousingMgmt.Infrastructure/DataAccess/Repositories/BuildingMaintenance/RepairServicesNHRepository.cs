using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.BuildingMaintenance {
	public class RepairServicesNHRepository : NHibernateReadOnlyRepository<RepairService>, IRepairServicesRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public RepairServicesNHRepository() { }


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public RepairServicesNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		public override IEnumerable<RepairService> GetAll() {
			return Session.QueryOver<RepairService>()
				.OrderBy(rs => rs.Name).Asc
				.List();
		}

		#endregion
	}
}
