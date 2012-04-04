using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.PersonsAndRoles;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.PersonsAndRoles {
	[TestFixture]
	class BuildingManagerTests {

		[Test]
		public void Can_Create_BuildingManager() {
			// Arrange
			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o.");

			// Act
			BuildingManager buildingManager = new BuildingManager(legalPerson);

			// Assert
			Assert.IsNotNull(buildingManager);
			Assert.AreEqual(legalPerson, buildingManager.LegalPerson);
			Assert.IsFalse(buildingManager.Contractors.Any());
		}

		[Test]
		public void Can_Add_Contractors_To_BuildingManager() {
			// Arrange
			LegalPerson legalPersonForContractor1 = new LegalPerson("12345678902", "Contractor 1 d.o.o.");
			Contractor contractor1 = new Contractor(legalPersonForContractor1);
			LegalPerson legalPersonForContractor2 = new LegalPerson("12345678903", "Contractor 2 d.o.o.");
			Contractor contractor2 = new Contractor(legalPersonForContractor2);

			Contractor[] contractors = new Contractor[] {
				contractor1, 
				contractor2
			};

			LegalPerson legalPersonForContractor3 = new LegalPerson("12345678904", "Contractor 3 d.o.o.");
			Contractor contractor3 = new Contractor(legalPersonForContractor3);

			LegalPerson legalPersonForManager = new LegalPerson("12345678901", "Mile d.o.o.");

			// Act
			BuildingManager buildingManager = new BuildingManager(legalPersonForManager, contractors);
			buildingManager.AddContractor(contractor3);

			// Assert
			Assert.AreEqual(3, buildingManager.Contractors.Count);
			Assert.AreEqual(contractor1, buildingManager.Contractors[0]);
			Assert.AreEqual(contractor2, buildingManager.Contractors[1]);
			Assert.AreEqual(contractor3, buildingManager.Contractors[2]);
		}

		[Test]
		public void Can_Remove_Service_From_Contractor() {
			LegalPerson legalPersonForContractor1 = new LegalPerson("12345678902", "Contractor 1 d.o.o.");
			Contractor contractor1 = new Contractor(legalPersonForContractor1);
			LegalPerson legalPersonForContractor2 = new LegalPerson("12345678903", "Contractor 2 d.o.o.");
			Contractor contractor2 = new Contractor(legalPersonForContractor2);
			LegalPerson legalPersonForContractor3 = new LegalPerson("12345678904", "Contractor 3 d.o.o.");
			Contractor contractor3 = new Contractor(legalPersonForContractor3);

			Contractor[] contractors = new Contractor[] {
				contractor1, 
				contractor2,
				contractor3
			};

			LegalPerson legalPersonForManager = new LegalPerson("12345678901", "Mile d.o.o.");

			BuildingManager buildingManager = new BuildingManager(legalPersonForManager, contractors);

			// Act
			buildingManager.RemoveContractor(contractor2);

			// Assert
			Assert.AreEqual(2, buildingManager.Contractors.Count);
			Assert.AreEqual(contractor1, buildingManager.Contractors[0]);
			Assert.AreEqual(contractor3, buildingManager.Contractors[1]);
		}
	}
}
