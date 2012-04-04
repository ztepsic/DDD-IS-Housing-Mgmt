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
	public class BillsNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Save_Bill_To_Db() {
			// Arrange
			City city = new City(10000, "Zagreb");
			Cadastre cadastre = new Cadastre("Trnje", "12345", city);
			CadastralParticle cadastralParticle = new CadastralParticle(cadastre, "123", 23, "Opis");
			LandRegistry landRegistry = new LandRegistry(cadastralParticle);

			var person = new PhysicalPerson("12345678901", "Ime", "Prezime") {
				Address = new Address("Ulica", "1", city)
			};

			var partitionSpace = landRegistry.CreatePartitionSpace("123", 12, "Opis etaže", person);

			LegalPerson legalPerson = new LegalPerson("12345678902", "Ime") {
				NumberOfBankAccount = "123456",
				Address = new Address("Ulica", "2", city)
			};
			BuildingManager buildingManager = new BuildingManager(legalPerson);

			Building building = new Building(buildingManager) {
				LandRegistry = landRegistry
			};


			Bill bill = new Bill(legalPerson, building.Reserve, "opis plaćanja", 23) {
				ReferenceNumber = "123"
			};
			bill.AddBillItem(new BillItem(1, 12.3m, "Opis"));

			IBillsRepository billsRepository = new BillsNHRepository(SessionFactory);

			// Act
			using (var session = SessionFactory.OpenSession()) {
				using (var transaction = session.BeginTransaction()) {
					session.Save(city);
					session.Save(person);
					session.Save(legalPerson);
					session.Save(cadastre);
					session.Save(landRegistry);
					session.Save(buildingManager);
					session.Save(building);

					billsRepository.SaveOrUpdate(bill);

					transaction.Commit();
				}
			}

			IList<Bill> billsFromDb = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var transaction = session.BeginTransaction()) {
					billsFromDb = billsRepository.GetAll().ToList();
					transaction.Commit();
				}
			}


			// Assert
			Assert.IsTrue(billsFromDb.Count == 1, "No Bill from database.");
		}

	}
}
