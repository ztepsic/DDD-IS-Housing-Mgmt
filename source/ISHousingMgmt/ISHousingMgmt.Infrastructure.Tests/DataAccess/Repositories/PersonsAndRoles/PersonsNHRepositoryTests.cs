using System.Collections.Generic;
using System.Linq;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.PersonsAndRoles {
	[TestFixture]
	class PersonsNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Save_Person_To_DB() {
			// Arrange
			PhysicalPerson physicalPerson = new PhysicalPerson("12345678901", "Ivo", "Ivic");
			LegalPerson legalPerson = new LegalPerson("12345678902", "FER");

			PersonsNHRepository personsNhRepository = new PersonsNHRepository(SessionFactory);

			// Act
			using (var session = SessionFactory.OpenSession()) {
				using(var tx = Session.BeginTransaction()) {
					personsNhRepository.SaveOrUpdate(physicalPerson);
					personsNhRepository.SaveOrUpdate(legalPerson);
					tx.Commit();
				}
				
			}

			IList<Person> persons = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					persons = personsNhRepository.GetAll().ToList();
					tx.Commit();
				}

			}

			// Assert
			Assert.IsTrue(persons.Contains(physicalPerson), "PhysicalPerson doesn't exists in database.");
			Assert.IsTrue(persons.Contains(legalPerson), "LegalPerson doesn't exists in database.");
		}

		[Test]
		public void Can_Save_Person_With_Address_To_DB() {
			// Arrange
			City zagreb = new City(10000, "Zagreb");
			Person person = new PhysicalPerson("12345678901", "Ivo", "Ivic") {
				Address = new Address("Ulica", "1A", zagreb)
			};

			PersonsNHRepository personsNhRepository = new PersonsNHRepository(SessionFactory);

			// Act
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = session.BeginTransaction()) {
					session.Save(zagreb);
					personsNhRepository.SaveOrUpdate(person);
					tx.Commit();
				}

			}

			IList<Person> persons = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = session.BeginTransaction()) {
					persons = personsNhRepository.GetAll().ToList();
					tx.Commit();
				}

			}

			// Assert
			Assert.IsTrue(persons.Contains(person), "PhysicalPerson doesn't exists in database.");
			Assert.AreEqual(person.Address, persons[0].Address, "Two persons haven't the sam addresses.");
			Assert.AreEqual(zagreb.Id, 1, "City isn't persisted to database.");
		}

		[Test]
		public void Can_Save_Person_With_Telephone_To_DB() {
			// Arrange
			Person person = new PhysicalPerson("12345678901", "Ivo", "Ivic");
			person.AddTelephone(new Telephone("Posao", "098 888 999"));

			PersonsNHRepository personsNhRepository = new PersonsNHRepository(SessionFactory);

			// Act
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = session.BeginTransaction()) {
					personsNhRepository.SaveOrUpdate(person);
					tx.Commit();
				}

			}

			IList<Person> persons = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = session.BeginTransaction()) {
					persons = personsNhRepository.GetAll().ToList();
					tx.Commit();
				}

			}

			// Assert
			Assert.IsTrue(persons.Contains(person), "PhysicalPerson doesn't exists in database.");
			Assert.IsTrue(person.Telephones.Contains(persons[0].Telephones.ElementAt(0)));

		}

	}
}
