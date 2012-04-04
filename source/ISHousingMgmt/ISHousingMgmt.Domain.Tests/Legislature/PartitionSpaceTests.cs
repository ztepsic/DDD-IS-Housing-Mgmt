using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using Moq;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.Legislature {
	[TestFixture]
	public class PartitionSpaceTests {

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
		public void Can_Create_PartitionSpace() {
			// Arrange

			// Act
			var partitionSpace = landRegistry.CreatePartitionSpace("123", 23, "Dvosoban stan");

			// Assert
			Assert.IsNotNull(partitionSpace);
		}

		[Test]
		public void Can_Create_Owned_PartitionSpace() {
			// Arrange
			var owner = new LegalPerson("12345678901", "Coca Cola");

			// Act
			var ownedPartitionSpace = landRegistry.CreatePartitionSpace("123", 23, "Dvosoban stan", owner);

			// Assert
			Assert.IsNotNull(ownedPartitionSpace);
		}

	}
}
