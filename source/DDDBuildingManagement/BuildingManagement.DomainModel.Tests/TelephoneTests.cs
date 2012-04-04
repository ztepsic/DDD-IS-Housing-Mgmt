using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.PersonsAndRoles;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests {
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
	}
}
