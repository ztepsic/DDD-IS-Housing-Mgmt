using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using Moq;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.Finances {
	[TestFixture]
	class BillTests {

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
		public void Can_Create_Bill() {
			// Arrange
			Person person = new PhysicalPerson("12345678901", "Mile", "Milic");

			// Act
			Bill bill = new Bill(building.Reserve, person, "opis plaćanja" ,23);

			// Assert
			Assert.IsNotNull(bill);
			Assert.IsFalse(bill.IsPaid);

		}

		[Test]
		[ExpectedException(typeof(BusinessRulesException))]
		public void PersonFrom_Must_Have_Valid_Bank_Account_Number() {
			// Arrange
			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o");

			// Act
			Bill bill = new Bill(legalPerson, building.Reserve, "opis plaćanja", 23);

			// Assert
		}

		[Test]
		public void Can_Calculate_Correct_Total_Amount_With_And_Without_Tax() {
			// Arrange
			Person person = new PhysicalPerson("12345678901", "Mile", "Milic");

			Bill bill = new Bill(building.Reserve, person, "opis plaćanja", 23);
			bill.AddBillItem(new BillItem(1, 23.5m, "Kruške"));
			bill.AddBillItem(new BillItem(3, 46.8m, "Jabuke"));

			// Act
			var totalAmount = bill.TotalAmount;
			var totalAmountWithTax = bill.TotalAmountWithTax;
			

			// Assert
			Assert.AreEqual(163.9m, totalAmount);
			Assert.AreEqual(201.597m, totalAmountWithTax);
		}

	}
}
