using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.PersonsAndRoles;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests {
	[TestFixture]
	class TelephoneTests {

		[Test]
		public void Two_Telehones_Should_Be_Equal() {
			// Arrange
			Telephone telephone1 = new Telephone("Kucni", "123");
			Telephone telephone2 = new Telephone("Kucni", "123");

			// Act
			var areEqual = telephone1.Equals(telephone2);

			// Assert
			Assert.IsTrue(areEqual);
		}

		[Test]
		public void Telephone_Must_Be_A_Value_Object() {
			// Arrange
			Telephone telephone = new Telephone("Kucni", "123");

			// Act

			// Assert
			Assert.IsInstanceOf(typeof(ValueObject), telephone);
		}
	}
}
