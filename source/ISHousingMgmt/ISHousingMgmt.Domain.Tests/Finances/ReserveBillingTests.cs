using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using Moq;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.Finances {
	[TestFixture]
	class ReserveBillingTests {

		private BuildingManager buildingManager;
		private Building building;
		private IPartitionSpace partitionSpace;

		[SetUp]
		public void SetUp() {
			Cadastre cadastre = new Cadastre("Trešnjevka", "332134", new City(10000, "Zagreb"));
			CadastralParticle cadastralParticle = new CadastralParticle(cadastre, "123", 20, "opis");
			LandRegistry landRegistry = new LandRegistry(cadastralParticle);
			partitionSpace = landRegistry.CreatePartitionSpace("123", 23, "Stan 1", new PhysicalPerson("12345678903", "Mile1", "Milic"));

			buildingManager = new BuildingManager(new LegalPerson("12345678903", "Upravitelj") { NumberOfBankAccount = "1234"});
			building = new Building(buildingManager) {
				LandRegistry = landRegistry
			};
		}
	}
}
