using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.BuildingManagement {
	[TestFixture]
	public class OwnerVoteTests {

		private Cadastre cadastre;
		private CadastralParticle cadastralParticle;
		private LandRegistry landRegistry;

		[SetUp]
		public void SetUp() {
			cadastre = new Cadastre("Trešnjevka", "332134", new City(10000, "Zagreb"));
			cadastralParticle = new CadastralParticle(cadastre, "123", 100, "opis");
			landRegistry = new LandRegistry(cadastralParticle);
		}

		[Test]
		public void Can_Create_OwnerVote() {
			// Arrange
			IPartitionSpace partitionSpace = landRegistry.CreatePartitionSpace("123", 23, "Stan 1", new PhysicalPerson("12345678901", "Mile", "Milic"));

			// Act
			OwnerVote ownerVote = new OwnerVote(true, partitionSpace);

			// Assert
			Assert.IsNotNull(ownerVote);
		}

	}
}
