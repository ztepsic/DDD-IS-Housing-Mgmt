using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using Moq;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.BuildingManagement {
	[TestFixture]
	class BuildingTests {

		#region SetUp Helper Methods

		private static Person getPerson() {
			// Dohvacanje osobe iz repozitorija
			Person personForMock = new PhysicalPerson("12345678901", "Mile", "Milic");

			Mock<IPersonsRepository> personRepositoryMock = new Mock<IPersonsRepository>();
			personRepositoryMock.Setup(x => x.GetByOib("12345678901")).Returns(personForMock);

			return personRepositoryMock.Object.GetByOib("12345678901");
		}

		private static City getCity() {
			City cityForMock = new City(10000, "Zagreb");
			Mock<ICitiesRepository> citiesRepositoryMock = new Mock<ICitiesRepository>();
			citiesRepositoryMock.Setup(x => x.GetCityByPostalCode(10000)).Returns(cityForMock);

			return citiesRepositoryMock.Object.GetCityByPostalCode(10000);
		}

		public static Cadastre getCadastre() {
			Cadastre cadastreForMock = new Cadastre("Trešnjevka", "12354", getCity());
			Mock<ICadastresRepository> cadastresRepositoryMock = new Mock<ICadastresRepository>();
			cadastresRepositoryMock.Setup(x => x.GetByMbr("12354")).Returns(cadastreForMock);

			return cadastresRepositoryMock.Object.GetByMbr("12345");
		}

		public static LandRegistry getLandRegistry() {
			CadastralParticle cadastralParticle = new CadastralParticle(getCadastre(), "123/12", 50, "Zgrada i dvorište");
			LandRegistry landRegistryForMock = new LandRegistry(cadastralParticle);
			Mock<ILandRegistriesRepository> landRegistriesRepositoryMock = new Mock<ILandRegistriesRepository>();
			landRegistriesRepositoryMock.Setup(x => x.GetByNumberOfCadastralParticle("123/12")).Returns(landRegistryForMock);

			return landRegistriesRepositoryMock.Object.GetByNumberOfCadastralParticle("123/12");
		}

		public static BuildingManager getBuildingManager() {
			LegalPerson legalPerson = new LegalPerson("12345678902", "Upravitelj");
			BuildingManager buildingManager = new BuildingManager(legalPerson);

			return buildingManager;
		}

		#endregion


		[Test]
		public void Can_Create_Building() {
			// Arrange
			LandRegistry landRegistry = getLandRegistry();
			BuildingManager buildingManager = getBuildingManager();

			// Act
			Building building = new Building(buildingManager) {
				LandRegistry = landRegistry
			};

			// Assert
			Assert.IsNotNull(building);
			Assert.IsNotNull(building.BuildingManager);
		}

		[Test]
		[ExpectedException(typeof(BuildingMustHaveBuildingManagerException))]
		public void Building_Always_Must_Have_BuildingManager() {
			// Arrange
			LandRegistry landRegistry = getLandRegistry();
			BuildingManager buildingManager = getBuildingManager();

			Building building = new Building(buildingManager) {
				LandRegistry = landRegistry
			};

			// Act
			building.SetBuildingManager(null);

			// Assert
		}

	}
}
