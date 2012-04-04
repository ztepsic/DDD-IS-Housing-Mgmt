using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.PersonsAndRoles;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.PersonsAndRoles {
	[TestFixture]
	class LegalPersonTests {

		[Test]
		[ExpectedException(typeof(BusinessRulesException<Person>))]
		public void If_Oib_Is_Invalid_Should_Throw_RuleException() {
			var person1 = new LegalPerson("123456", "Mile");
			var person2 = new LegalPerson("abs", "Mile");
		}

		[Test]
		public void Can_Take_Snapshot_Of_Person() {
			// Arrange
			LegalPerson person = new LegalPerson("12345678901", "Mile");
			Telephone telephone1 = new Telephone("Kucni telefon", "123456");
			Telephone telephone2 = new Telephone("Mobitel", "098123456");

			person.AddTelephone(telephone1);
			person.AddTelephone(telephone2);

			// Act
			var personSnapshot = new PersonSnapshot(person);

			// Assert
			Assert.AreEqual(person.Oib, personSnapshot.Oib);
			Assert.AreEqual(person.Name, personSnapshot.FullName);
			Assert.AreEqual(person.Address, personSnapshot.Address);
		}
	}
}
