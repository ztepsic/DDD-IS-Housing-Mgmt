using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using NUnit.Framework;
using Moq;

namespace ISHousingMgmt.Domain.Tests.Finances {
	[TestFixture]
	public class ReserveTests {

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
			CadastralParticle cadastralParticle = new CadastralParticle(getCadastre(), "123/12", 50, "Zgrada i dvorište");
			LandRegistry landRegistryForMock = new LandRegistry(cadastralParticle);
			Mock<ILandRegistriesRepository> landRegistriesRepositoryMock = new Mock<ILandRegistriesRepository>();
			landRegistriesRepositoryMock.Setup(x => x.GetByNumberOfCadastralParticle("123/12")).Returns(landRegistryForMock);

			return landRegistriesRepositoryMock.Object.GetByNumberOfCadastralParticle("123/12");
		}

		public static Building getBuilding() {
			Building building = new Building(getBuildingManager()) {
				RepresentativeOfPartOwners = new PhysicalPerson("12345678902", "Mile", "Milic"),
				LandRegistry = getLandRegistry()
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
			var reserve = building.Reserve;

			// Assert
			Assert.IsNotNull(reserve);
		}

		[Test]
		public void Can_Pay_Bill_From_Reserve() {
			// Arrange
			Reserve reserve = building.Reserve;
			decimal currentMoneyStatus = reserve.Money;

			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o.") {
				NumberOfBankAccount = "1234"
			};

			Bill bill = new Bill(legalPerson, building.Reserve, "opis plaćanja", 23);
			reserve.AddBillForPayment(bill);

			// Act
			reserve.PayBill(bill);

			// Assert
			Assert.AreEqual(currentMoneyStatus - bill.TotalAmountWithTax, reserve.Money);
			Assert.IsTrue(bill.IsPaid);
		}

		[Test]
		public void Can_Receive_Money_From_Payed_Bill() {
			// Arrange
			Reserve reserve = building.Reserve;
			decimal currentMoneyStatus = reserve.Money;
			var person = new PhysicalPerson("12345678903", "Mile", "Milic");

			Bill bill = new Bill(building.Reserve, person, "opis plaćanja", 23);
			var partitionSpace = building.LandRegistry.CreatePartitionSpace("123", 12m, "Opis", person);
			reserve.IssueReserveBillFor(partitionSpace, 23);

			// Act
			reserve.ReceivePaymentFor(bill);

			// Assert
			Assert.AreEqual(currentMoneyStatus + bill.TotalAmountWithTax, reserve.Money);
			Assert.IsTrue(bill.IsPaid);
		}

	}
}
