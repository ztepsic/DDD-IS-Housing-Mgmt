using System.Linq;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.PersonsAndRoles;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.PersonsAndRoles {
	[TestFixture]
	class PhysicalPersonTests {

		[Test]
		public void Can_Add_Multiple_Telephone_Numbers() {
			// Arrange
			PhysicalPerson person = new PhysicalPerson("12345678901", "Mile", "Milic");
			Telephone telephone1 = new Telephone("Kucni telefon", "123456");
			Telephone telephone2 = new Telephone("Mobitel", "098123456");

			// Act
			person.AddTelephone(telephone1);
			person.AddTelephone(telephone2);

			// Assert
			Assert.AreEqual(2, person.Telephones.Count, "Trebala bi postojati dva telefonska broja.");
			Assert.AreEqual("123456", person.Telephones.ElementAt(0).TelephoneNumber, "Prvi broj telefona nije isti kao zadani.");
			Assert.AreEqual("098123456", person.Telephones.ElementAt(1).TelephoneNumber, "Drugi broj telefona nije isti kao zadani.");
		}

		[Test]
		public void Can_Remove_Existing_Telehone_Number() {
			// Arange
			PhysicalPerson person = new PhysicalPerson("12345678901", "Mile", "Milic");
			Telephone telephone1 = new Telephone("Kucni telefon", "123456");
			Telephone telephone2 = new Telephone("Mobitel", "098123456");

			person.AddTelephone(telephone1);
			person.AddTelephone(telephone2);

			// Act
			var isSuccess = person.RemoveTelephone(telephone1);

			// Assert
			Assert.IsTrue(isSuccess);
			Assert.AreEqual(1, person.Telephones.Count, "Trebao bi postojati jedan telefonski broj.");
			Assert.AreEqual("098123456", person.Telephones.ElementAt(0).TelephoneNumber, "To nije broj mobitela, odnosno drugi zadani broj telefona..");
		}

		[Test]
		[ExpectedException(typeof(BusinessRulesException<Person>))]
		public void If_Oib_Is_Invalid_Should_Throw_RuleException() {
			var person1 = new PhysicalPerson("123456", "Mile", "Milic");
			var person2 = new PhysicalPerson("abs", "Mile", "Milic");
		}

		[Test]
		public void Can_Take_Snapshot_Of_Person() {
			// Arrange
			PhysicalPerson person = new PhysicalPerson("12345678901", "Mile", "Milic");
			Telephone telephone1 = new Telephone("Kucni telefon", "123456");
			Telephone telephone2 = new Telephone("Mobitel", "098123456");

			person.AddTelephone(telephone1);
			person.AddTelephone(telephone2);

			// Act
			var personSnapshot = new PersonSnapshot(person);

			// Assert
			Assert.AreEqual(person.Oib, personSnapshot.Oib);
			Assert.AreEqual(person.FullName, personSnapshot.FullName);
			Assert.AreEqual(person.Address, personSnapshot.Address);
		}

	}
}
