using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.BuildingManagement;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.BuildingManagement {
	[TestFixture]
	public class AdminJobsVotingsNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Save_AdminJobsVoting_To_DB() {
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

			AdministrationJobsVoting administrationJobsVoting = new AdministrationJobsVoting(
				AdministrationJobsType.Regular,
				building,
				"Subject",
				"Description",
				new DateTime(2011, 11, 11));

			AdminJobsVotingsNHRepository adminJobsVotingsNhRepository = new AdminJobsVotingsNHRepository(SessionFactory);

			// Act
			AdministrationJobsVoting adminJobsVotingFromDb = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = session.BeginTransaction()) {
					session.Save(city);
					session.Save(person);
					session.Save(legalPerson);
					session.Save(cadastre);
					session.Save(landRegistry);
					session.Save(buildingManager);
					session.Save(building);

					adminJobsVotingsNhRepository.SaveOrUpdate(administrationJobsVoting);
					tx.Commit();
				}

				using (var tx = session.BeginTransaction()) {
					adminJobsVotingFromDb = adminJobsVotingsNhRepository.GetById(administrationJobsVoting.Id);
					tx.Commit();
				}
			}

			// Assert
			Assert.AreEqual(administrationJobsVoting, adminJobsVotingFromDb, "Two administration jobs voting entities aren't equal.");
		}

	}
}
