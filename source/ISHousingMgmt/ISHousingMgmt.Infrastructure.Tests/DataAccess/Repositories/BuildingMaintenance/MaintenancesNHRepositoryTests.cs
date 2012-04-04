using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.BuildingMaintenance;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.BuildingMaintenance {
	[TestFixture]
	public class MaintenancesNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Save_Maintenance_To_DB() {
			// Arrange
			var person = new PhysicalPerson("12345678901", "Ime", "Prezime");
			MaintenanceRequest maintenanceRequest = new MaintenanceRequest(
				person
				, "Kvar na grijanju"
				, "Grijanje ne radi"
				, "Prvi kat, stan 2");

			City city = new City(10000, "Zagreb");
			Cadastre cadastre = new Cadastre("Trnje", "12345", city);
			CadastralParticle cadastralParticle = new CadastralParticle(cadastre, "123", 23, "Opis");
			LandRegistry landRegistry = new LandRegistry(cadastralParticle);

			var partitionSpace = landRegistry.CreatePartitionSpace("123", 12, "Opis etaže", person);

			LegalPerson legalPerson = new LegalPerson("12345678902", "Ime");
			BuildingManager buildingManager = new BuildingManager(legalPerson);

			Building building = new Building(buildingManager) {
				LandRegistry = landRegistry
			};

			RepairService repairService = new RepairService("Popravak grijanja");

			Maintenance maintenance = new Maintenance(maintenanceRequest, Urgency.Normal, repairService, building);

			IMaintenancesRepository maintenancesRepository = new MaintenancesNHRepository(SessionFactory);

			// Act
			IList<Maintenance> maintenancesFromDb = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = session.BeginTransaction()) {
					session.Save(person);
					session.Save(city);
					session.Save(legalPerson);
					session.Save(cadastre);
					session.Save(landRegistry);
					session.Save(buildingManager);
					session.Save(building);
					session.Save(repairService);

					maintenancesRepository.SaveOrUpdate(maintenance);

					tx.Commit();
				}

				using (var tx = session.BeginTransaction()) {
					maintenancesFromDb = maintenancesRepository.GetAll().ToList();
					tx.Commit();
				}
			}

			// Assert
			Assert.IsTrue(maintenancesFromDb.Count == 1, "No Maintenance from database.");
		}

	}
}
