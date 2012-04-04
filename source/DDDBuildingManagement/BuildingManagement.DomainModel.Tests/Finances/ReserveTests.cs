using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.BuildingManagement;
using BuildingManagement.DomainModel.Finances;
using BuildingManagement.DomainModel.Legislature;
using BuildingManagement.DomainModel.PersonsAndRoles;
using NUnit.Framework;
using Moq;

namespace BuildingManagement.DomainModel.Tests.Finances {
	[TestFixture]
	class ReserveTests {

		private Building building;

		#region SetUp Helper Methods

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

		public static Building getBuilding() {
			Building building = new Building(getLandRegistry(), getBuildingManager()) {
				RepresentativeOfPartOwners = new PhysicalPerson("12345678902", "Mile", "Milic")
			};

			return building;
		}


		public static BuildingManager getBuildingManager() {
			LegalPerson legalPerson = new LegalPerson("12345678902", "Upravitelj") {
				NumberOfBankAccount = "1234567"
			};
			BuildingManager buildingManager = new BuildingManager(legalPerson);

			return buildingManager;
		}

		#endregion

		[SetUp]
		public void SetUp() {
			// Dohvacanje zgrade za zadanu osobu
			building = getBuilding();
		}

		[Test]
		public void Can_Create_Reserve() {
			// Arrange

			// Act
			Reserve reserve = new Reserve(building);

			// Assert
			Assert.IsNotNull(reserve);
		}

		[Test]
		public void Can_Pay_Bill_From_Reserve() {
			// Arrange
			Reserve reserve = new Reserve(building);
			decimal currentMoneyStatus = reserve.MoneyStatus;

			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o.") {
				NumberOfBankAccount = "1234"
			};

			Bill bill = new Bill(building.RepresentativeOfPartOwners, legalPerson, 23);

			// Act
			reserve.PayBill(bill);

			// Assert
			Assert.AreEqual(currentMoneyStatus - bill.TotalAmountWithTax, reserve.MoneyStatus);
			Assert.IsTrue(bill.IsPayed);
		}

		[Test]
		public void Can_Receive_Money_From_Payed_Bill() {
			// Arrange
			Reserve reserve = new Reserve(building);
			decimal currentMoneyStatus = reserve.MoneyStatus;

			Bill bill = new Bill(new PhysicalPerson("12345678903", "Mile", "Milic"), building.BuildingManager.LegalPerson, 23);

			// Act
			reserve.PayBill(bill);

			// Assert
			Assert.AreEqual(currentMoneyStatus + bill.TotalAmountWithTax, reserve.MoneyStatus);
			Assert.IsTrue(bill.IsPayed);
		}

	}
}
