using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.BuildingManagement;
using BuildingManagement.DomainModel.Finances;
using BuildingManagement.DomainModel.Legislature;
using BuildingManagement.DomainModel.PersonsAndRoles;
using Moq;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.BuildingManagement {
	[TestFixture]
	class ReserveBillingTests {

		private BuildingManager buildingManager;
		private Apartment apartment;

		#region SetUp Helper Methods

		public static BuildingManager getBuildingManager() {
			LegalPerson legalPerson = new LegalPerson("12345678902", "Upravitelj") {
				NumberOfBankAccount = "1234"
			};
			BuildingManager buildingManager = new BuildingManager(legalPerson);

			return buildingManager;
		}

		public static Apartment getApartment() {
			Mock<IPartitionSpace> partitionSpaceMock = new Mock<IPartitionSpace>();
			partitionSpaceMock.Setup(x => x.SurfaceArea).Returns(50);

			Apartment apartment = new Apartment(partitionSpaceMock.Object) {
				ResponsibleTenant = new PhysicalPerson("12345678901", "Mile", "Milic")
			};

			return apartment;
		}

		#endregion

		[SetUp]
		public void SetUp() {
			buildingManager = getBuildingManager();
			apartment = getApartment();
		}

		[Test]
		public void Can_Create_ReserveBilling() {
			// Arrange

			// Act
			ReserveBilling reserveBilling = new ReserveBilling(23, buildingManager);

			// Assert
			Assert.IsNotNull(reserveBilling);
		}

		[Test]
		public void Can_Create_ReserveBill() {
			// Arrange
			ReserveBilling reserveBilling = new ReserveBilling(23,buildingManager);


			// Act
			Bill bill = reserveBilling.IssueReserveBillFor(apartment);

			// Assert
			Assert.AreEqual(94.095m, bill.TotalAmountWithTax);
		}
	}
}
