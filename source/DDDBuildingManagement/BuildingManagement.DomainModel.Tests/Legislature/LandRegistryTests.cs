using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Legislature;
using BuildingManagement.DomainModel.PersonsAndRoles;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.Legislature {
	[TestFixture]
	class LandRegistryTests {

		private Cadastre cadastre;
		private AbstractCadastralParticle cadastralParticle;
		private LandRegistry landRegistry;

		[SetUp]
		public void SetUp() {
			cadastre = new Cadastre("Trešnjevka", "332134", new City(10000, "Zagreb"));
			cadastralParticle = new CadastralParticle(cadastre, "123", 20, "opis");
			landRegistry = new LandRegistry(cadastralParticle);
		}

		[Test]
		public void Empty_PartitionSpaces_In_LandRegistry_Has_Zero_For_Total_SurfaceArea() {
			// Arrange

			// Act
			var result = landRegistry.TotalSurfaceOfPartitionSpaces;

			// Assert
			Assert.AreEqual(0, result);
		}

		[Test]
		public void LandRegistry_With_PartitionSpaces_Has_Total_SurfaceAres() {
			// Arrange
			landRegistry.CreatePartitionSpace(23.52m, "Stan 1");

			var owner = new PhysicalPerson("12345678901", "Mile", "Milic");

			landRegistry.CreatePartitionSpace(35.1m, "Stan 2", owner, 125);

			// Act
			var result = landRegistry.TotalSurfaceOfPartitionSpaces;

			// Assert
			Assert.AreEqual(58.62, result);
		}

	}
}
