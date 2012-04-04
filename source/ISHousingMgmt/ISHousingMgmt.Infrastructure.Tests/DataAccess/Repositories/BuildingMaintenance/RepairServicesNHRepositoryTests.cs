using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.BuildingMaintenance;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.BuildingMaintenance {
	[TestFixture]
	public class RepairServicesNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Get_RepairServices_From_DB() {
			// Arrange
			RepairService repairService = new RepairService("Popravak vode");

			RepairServicesNHRepository repairServicesNhRepository = new RepairServicesNHRepository(SessionFactory);

			// Act
			IList<RepairService> repairServicesFromDb = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = session.BeginTransaction()) {
					session.Save(repairService);
					tx.Commit();
				}

				using (var tx = session.BeginTransaction()) {
					repairServicesFromDb = repairServicesNhRepository.GetAll().ToList();
					tx.Commit();
				}
			}

			// Assert
			Assert.IsTrue(repairServicesFromDb.Count == 1, "No RepairService from database.");
		}
	}
}
