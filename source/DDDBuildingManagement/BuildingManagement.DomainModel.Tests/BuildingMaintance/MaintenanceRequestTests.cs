using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.BuildingMaintenance;
using BuildingManagement.DomainModel.PersonsAndRoles;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.BuildingMaintance {
	[TestFixture]
	class MaintenanceRequestTests {

		[Test]
		public void Can_Create_MaintenanceRequest() {
			// Arrange
			LegalPerson person = new LegalPerson("12345678901", "Mile");

			// Act
			MaintenanceRequest maintenanceRequest = new MaintenanceRequest(person, "Kvar", "Ne radi grijanje.", "Stan Mile, prvi kat.");

			// Assert
			Assert.IsNotNull(maintenanceRequest, "MaintenanceRequest was not created.");
		}

		[Test]
		public void Date_And_Time_Of_Request_Are_Current_After_Creation() {
			// Arrange
			LegalPerson person = new LegalPerson("12345678901", "Mile");
			DateTime theTimeBefore = DateTime.Now.AddMilliseconds(-1);

			// Act
			MaintenanceRequest maintenanceRequest = new MaintenanceRequest(person, "Kvar", "Ne radi grijanje.", "Stan Mile, prvi kat.");

			// Assert
			Assert.IsTrue(maintenanceRequest.DateTimeOfRequest > theTimeBefore);
			Assert.IsTrue(maintenanceRequest.DateTimeOfRequest < DateTime.Now.AddMilliseconds(1));
		}

	}
}
