using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests {
	[TestFixture]
	public class AddressTests {

		[Test]
		public void Address_Must_Be_A_Value_Object() {
			// Arrange
			var address = new Address("Ulica", "1", new City(10000, "Zagreb"));

			// Act

			// Assert
			Assert.IsInstanceOf(typeof(ValueObject), address);
		}

		[Test]
		[Ignore("Ignore a test")]
		[ExpectedException(typeof(BusinessRulesException))]
		public void Address_Name_Is_Invalid_If_Contains_NonAlpha_Caracters() {
			// Arrange
			var address = new Address("Ulica", "1", new City(10000, "Zagreb"));

			// Act
			// exception should be trown
		}

		[Test]
		[ExpectedException(typeof(BusinessRulesException))]
		public void Address_Must_Have_Not_Null_City() {
			// Arrange
			var address = new Address("Ulica", "1", null);

			// Act

			// Assert
			// exception should be trown
		}

		[Test]
		public void Can_Upercase() {
			string name = "želko";
			TextInfo myTI = Thread.CurrentThread.CurrentCulture.TextInfo;
			Assert.AreEqual("Želko", myTI.ToTitleCase(name));
		}

	}
}
