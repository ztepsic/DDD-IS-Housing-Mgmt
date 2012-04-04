using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.PersonsAndRoles {
	[TestFixture]
	public class BuildingManagersNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Save_BuildingManager_To_DB() {
			// Arrange
			LegalPerson legalPerson = new LegalPerson("12345678901", "Ime");
			BuildingManager buildingManager = new BuildingManager(legalPerson);

			BuildingManagersNHRepository buildingManagersNhRepository = new BuildingManagersNHRepository(SessionFactory);

			// Act
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					buildingManagersNhRepository.SaveOrUpdate(buildingManager);
					tx.Commit();
				}

			}

			IList<BuildingManager> managers = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					managers = buildingManagersNhRepository.GetAll().ToList();
					tx.Commit();
				}

			}

			// Assert
			Assert.IsTrue(managers.Count > 0, "Contractor has been saved to database.");
		}

	}
}
