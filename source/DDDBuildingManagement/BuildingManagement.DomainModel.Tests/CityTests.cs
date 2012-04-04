using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests {
	[TestFixture]
	class CityTests {

		[Test]
		public void City_Id_Is_Null_After_Creation() {
			// Arrange

			// Act
			var city = new City(10000, "Zagreb");

			// Assert
			Assert.IsNull(city.Id, "The Id is not null.");
		}

	}
}
