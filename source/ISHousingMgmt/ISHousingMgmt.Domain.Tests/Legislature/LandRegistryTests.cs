using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using System.Linq;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.Legislature {
	[TestFixture]
	public class LandRegistryTests {

		private Cadastre cadastre;
		private CadastralParticle cadastralParticle;
		private LandRegistry landRegistry;

		[SetUp]
		public void SetUp() {
			cadastre = new Cadastre("Trešnjevka", "332134", new City(10000, "Zagreb"));
			cadastralParticle = new CadastralParticle(cadastre, "123", 120, "opis");
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
		public void LandRegistry_With_PartitionSpaces_Has_Total_SurfaceAreas() {
			// Arrange
			landRegistry.CreatePartitionSpace("123", 23.52m, "Stan 1");

			var owner = new PhysicalPerson("12345678901", "Mile", "Milic");

			landRegistry.CreatePartitionSpace("123", 35.1m, "Stan 2", owner);

			// Act
			var result = landRegistry.TotalSurfaceOfPartitionSpaces;

			// Assert
			Assert.AreEqual(58.62, result);
		}

		[Test]
		[ExpectedException(typeof(BusinessRulesException))]
		public void Cant_Add_PartitionSpace_If_TotalSurfaceArea_Is_Grater_Than_CadastralParticle_TotalSurfaceArea() {
			// Arrange

			// Act
			landRegistry.CreatePartitionSpace("1234", 123, "Opis");

			// Assert
		}
	}
}
