using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests {
	[TestFixture]
	public class CityTests {

		[Test]
		[Ignore("Ignore a test")]
		public void City_Id_Is_Empty_After_Creation() {
			// Arrange

			// Act
			var city = new City(10000, "Zagreb");

			// Assert
			Assert.AreEqual(0, city.Id, "The Id isn't null.");
		}

		[Test]
		[Ignore("Ignore a test")]
		[ExpectedException(typeof(BusinessRulesException))]
		public void City_PostalCode_Is_Invalid_When_It_Have_Not_Five_Digits() {
			// Arrange

			// Act
			var city = new City(42, "Zagreb");

			// Assert
		}

		[Test]
		[Ignore("Ignore a test")]
		[ExpectedException(typeof(BusinessRulesException))]
		public void City_Name_Is_Invalid_When_It_Have_NonAlpha_Character() {
			// Arrange
			
			// Act
			var city = new City(10000, "7agr3b");

			// Assert
		}

		[Test]
		public void Two_Equal_No_Persisted_Entities_Should_Be_Equal() {
			// Arrange
			var city1 = new City(10000, "Zagreb");
			var city2 = new City(10000, "Zagreb");

			// Act

			// Assert
			Assert.AreEqual(city1, city2, "Two no persisted city entities aren't equal.");
		}
		

	}
}
