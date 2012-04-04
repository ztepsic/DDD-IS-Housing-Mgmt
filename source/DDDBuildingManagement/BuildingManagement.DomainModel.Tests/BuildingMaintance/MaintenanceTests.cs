using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.BuildingMaintenance;
using BuildingManagement.DomainModel.BuildingManagement;
using BuildingManagement.DomainModel.Legislature;
using BuildingManagement.DomainModel.PersonsAndRoles;
using Moq;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.BuildingMaintance {
	[TestFixture]
	class MaintenanceTests {

		private Building building;
		private Person person;

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
			AbstractCadastralParticle cadastralParticle = new CadastralParticle(getCadastre(), "123/12", 50, "Zgrada i dvorište");
			LandRegistry landRegistryForMock = new LandRegistry(cadastralParticle);
			Mock<ILandRegistriesRepository> landRegistriesRepositoryMock = new Mock<ILandRegistriesRepository>();
			landRegistriesRepositoryMock.Setup(x => x.GetByNumberOfCadastralParticle("123/12")).Returns(landRegistryForMock);

			return landRegistriesRepositoryMock.Object.GetByNumberOfCadastralParticle("123/12");
		}

		public static Building getBuilding(Person person) {
			Building buildingForMock = new Building(getLandRegistry(), getBuildingManager());
			IList<Building> buildingsForMock = new List<Building>() { buildingForMock };
			Mock<IBuildingsRepository> buildingRepositoryMock = new Mock<IBuildingsRepository>();
			buildingRepositoryMock.Setup(x => x.GetBuildings(person.Oib)).Returns(buildingsForMock);

			return buildingRepositoryMock.Object.GetBuildings(person.Oib)[0];
		}

		public static BuildingManager getBuildingManager() {
			LegalPerson legalPerson = new LegalPerson("12345678902", "Upravitelj");
			BuildingManager buildingManager = new BuildingManager(legalPerson);

			return buildingManager;
		}

		#endregion

		[SetUp]
		public void SetUp() {
			// Dohvacanje osobe iz repozitorija
			person = getPerson();

			// Dohvacanje zgrade za zadanu osobu
			building = getBuilding(person);

		}

		[Test]
		public void Person_Can_Issue_Maitenance_Request() {
			// Arrange
			MaintenanceRequest maintenanceRequest = new MaintenanceRequest(person, "Kvar", "Ne radi grijanje.", "Stan Mile, prvi kat.");

			RepairService serviceTypeForMock = new RepairService("Popravci grijanja i toplana");
			Mock<IRepairServicesRepository> repairServicesRepositoryMock = new Mock<IRepairServicesRepository>();
			repairServicesRepositoryMock.Setup(x => x.GetById(23)).Returns(serviceTypeForMock);

			RepairService serviceType = repairServicesRepositoryMock.Object.GetById(23);

			// Act
			Maintenance maintenance = new Maintenance(maintenanceRequest, Urgency.High, serviceType, building);

			// Assert
			Assert.AreEqual(maintenanceRequest, maintenance.MaintenanceRequest);
			Assert.AreEqual(StatusOfMaintenance.NotStarted, maintenance.StatusOfMaintenance);
			Assert.IsNotNull(maintenance.Urgency, "Hitnost nije definirana.");
			Assert.AreEqual(Urgency.High, maintenance.Urgency);
			Assert.IsNotNull(maintenance.ServiceType, "Usluga popravka nije definirana.");
			Assert.AreEqual(serviceType, maintenance.ServiceType);
			
		}

		[Test]
		public void BuildingManager_Can_Set_Cotractor_For_Maitenance() {
			// Arrange
			MaintenanceRequest maintenanceRequest = new MaintenanceRequest(person, "Kvar", "Ne radi grijanje.", "Stan Mile, prvi kat.");

			RepairService serviceTypeForMock = new RepairService("Ličenje");
			Mock<IRepairServicesRepository> repairServicesRepositoryMock = new Mock<IRepairServicesRepository>();
			repairServicesRepositoryMock.Setup(x => x.GetById(23)).Returns(serviceTypeForMock);

			RepairService serviceType = repairServicesRepositoryMock.Object.GetById(23);
			Maintenance maintenance = new Maintenance(maintenanceRequest, Urgency.High, serviceType, building);

			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o.");
			Contractor contractor = new Contractor(legalPerson).AddRepairService(serviceType);

			BuildingManager buildingManager = maintenance.BuildingManager;
			buildingManager.AddContractor(contractor);

			// Act
			buildingManager.SetContractorForMaintenance(contractor, maintenance);

			// Assert
			Assert.IsNotNull(maintenance.Contractor);
			Assert.AreEqual(new PersonSnapshot(contractor.LegalPerson), maintenance.Contractor );
		}

	}
}
