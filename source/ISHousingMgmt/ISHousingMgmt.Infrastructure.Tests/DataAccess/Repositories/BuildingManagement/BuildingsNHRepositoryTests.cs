using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.BuildingManagement;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.BuildingManagement {
	[TestFixture]
	class BuildingsNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Save_Building_To_DB() {
			// Arrange
			City city = new City(10000, "Zagreb");
			Cadastre cadastre = new Cadastre("Trnje", "12345", city);
			CadastralParticle cadastralParticle = new CadastralParticle(cadastre, "123", 23, "Opis");
			LandRegistry landRegistry = new LandRegistry(cadastralParticle);

			var person = new PhysicalPerson("12345678901", "Ime", "Prezime");
			var partitionSpace = landRegistry.CreatePartitionSpace("123", 12, "Opis etaže", person);

			LegalPerson legalPerson = new LegalPerson("12345678902", "Ime");
			BuildingManager buildingManager = new BuildingManager(legalPerson);

			Building building = new Building(buildingManager) {
				LandRegistry = landRegistry
			};

			BuildingsNHRepository buildingsNhRepository = new BuildingsNHRepository(SessionFactory);

			// Act
			Building buildingFromDb = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = session.BeginTransaction()) {
					session.Save(city);
					session.Save(person);
					session.Save(legalPerson);
					session.Save(cadastre);
					session.Save(landRegistry);
					session.Save(buildingManager);

					buildingsNhRepository.SaveOrUpdate(building);
					tx.Commit();
				}

				using (var tx = session.BeginTransaction()) {
					buildingFromDb = buildingsNhRepository.GetById(building.Id);
					tx.Commit();
				}
			}

			// Assert
			Assert.AreEqual(building, buildingFromDb, "Two building entities aren't equal.");
		}

	}
}
