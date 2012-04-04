using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.BuildingMaintenance;
using BuildingManagement.DomainModel.PersonsAndRoles;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.PersonsAndRoles {
	[TestFixture]
	class ContractorTests {

		[Test]
		public void Can_Create_Contractor() {
			// Arrange
			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o.");

			// Act
			Contractor contractor = new Contractor(legalPerson);

			// Assert
			Assert.IsNotNull(contractor);
			Assert.AreEqual(legalPerson, contractor.LegalPerson);
			Assert.IsFalse(contractor.RepairServices.Any());
		}

		[Test]
		public void Can_Add_Services_To_Contractor() {
			// Arrange
			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o.");

			RepairService repairService1 = new RepairService("Ličenje");
			RepairService repairService2 = new RepairService("vodovodne cijevi");
			RepairService[] repairServices = new RepairService[] {
				repairService1, 
				repairService2
			};

			
			// Act
			Contractor contractor = new Contractor(legalPerson, repairServices);
			RepairService repairService3 = new RepairService("parketi");
			contractor.AddRepairService(repairService3);

			// Assert
			Assert.AreEqual(3, contractor.RepairServices.Count);
			Assert.AreEqual(repairService1, contractor.RepairServices[0]);
			Assert.AreEqual(repairService2, contractor.RepairServices[1]);
			Assert.AreEqual(repairService3, contractor.RepairServices[2]);
		}

		[Test]
		public void Can_Remove_Service_From_Contractor() {
			// Arrange
			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o.");

			RepairService repairService1 = new RepairService("Ličenje");
			RepairService repairService2 = new RepairService("vodovodne cijevi");
			RepairService repairService3 = new RepairService("parketi");
			RepairService[] repairServices = new RepairService[] {
				repairService1, 
				repairService2,
				repairService3
			};

			Contractor contractor = new Contractor(legalPerson, repairServices);

			// Act
			contractor.RemoveRepairService(repairService2);

			// Assert
			Assert.AreEqual(2, contractor.RepairServices.Count);
			Assert.AreEqual(repairService1, contractor.RepairServices[0]);
			Assert.AreEqual(repairService3, contractor.RepairServices[1]);
		}

	}
}
