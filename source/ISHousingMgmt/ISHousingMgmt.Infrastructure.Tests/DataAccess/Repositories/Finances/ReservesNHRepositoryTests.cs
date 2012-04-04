using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.Finances;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.Finances {
	[TestFixture]
	public class ReservesNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Read_Reserve_From_Db() {
			// Arrange
			City city = new City(10000, "Zagreb");
			Cadastre cadastre = new Cadastre("Trnje", "12345", city);
			CadastralParticle cadastralParticle = new CadastralParticle(cadastre, "123", 23, "Opis");
			LandRegistry landRegistry = new LandRegistry(cadastralParticle);

			var person = new PhysicalPerson("12345678901", "Ime", "Prezime");
			var partitionSpace = landRegistry.CreatePartitionSpace("123", 12, "Opis etaže", person);

			LegalPerson legalPerson = new LegalPerson("12345678902", "Ime") {
				NumberOfBankAccount = "12332213",
				Address = new Address("dsa", "2", city)
			};
			BuildingManager buildingManager = new BuildingManager(legalPerson);

			Building building = new Building(buildingManager) {
				LandRegistry = landRegistry
			};

			Bill bill = new Bill(legalPerson, building.Reserve, "račun", 23) {
				ReferenceNumber = "123"
			};
			building.Reserve.AddBillForPayment(bill);


			// Act
			using (var session = SessionFactory.OpenSession()) {
				using (var transaction = session.BeginTransaction()) {
					session.Save(city);
					session.Save(cadastre);
					session.Save(person);
					session.Save(landRegistry);
					session.Save(legalPerson);
					session.Save(buildingManager);
					session.Save(building);

					transaction.Commit();
				}
			}

			IReservesRepository reservesRepository = new ReservesNHRepository(SessionFactory);
			IList<Reserve> reservesFromDb = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var transaction = session.BeginTransaction()) {
					reservesFromDb = reservesRepository.GetAll().ToList();
					transaction.Commit();
				}
			}

			// Assert
			Assert.IsTrue(reservesFromDb.Count == 1, "No Reserves from database.");
			//Assert.IsTrue(reservesFromDb[0].ReserveBills.Count == 1);
		}

	}
}
