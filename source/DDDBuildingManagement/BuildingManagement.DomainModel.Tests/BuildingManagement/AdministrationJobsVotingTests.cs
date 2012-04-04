using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Legislature;
using BuildingManagement.DomainModel.BuildingManagement;
using BuildingManagement.DomainModel.PersonsAndRoles;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.BuildingManagement {
	[TestFixture]
	class AdministrationJobsVotingTests {

		private Cadastre cadastre;
		private AbstractCadastralParticle cadastralParticle;
		private LandRegistry landRegistry;
		private Building building;
		private IPartitionSpace partitionSpace1;
		private IPartitionSpace partitionSpace2;
		private IPartitionSpace partitionSpace3;
		private IPartitionSpace partitionSpace4;

		[SetUp]
		public void SetUp() {
			cadastre = new Cadastre("Trešnjevka", "332134", new City(10000, "Zagreb"));
			cadastralParticle = new CadastralParticle(cadastre, "123", 20, "opis");
			landRegistry = new LandRegistry(cadastralParticle);

			var buildingManager = new BuildingManager(new LegalPerson("12345678903", "Upravitelj"));

			building = new Building(landRegistry, buildingManager);

			partitionSpace1 = landRegistry.CreatePartitionSpace(23, "Stan 1", new PhysicalPerson("12345678903", "Mile1", "Milic"), 20);
			var apartment1 = new Apartment(partitionSpace1);
			building.AddApartment(apartment1);

			partitionSpace2 = landRegistry.CreatePartitionSpace(23, "Stan 2", new PhysicalPerson("12345678904", "Mile2", "Milic"), 20);
			var apartment2 = new Apartment(partitionSpace2);
			building.AddApartment(apartment2);

			partitionSpace3 = landRegistry.CreatePartitionSpace(23, "Stan 3", new PhysicalPerson("12345678905", "Mile3", "Milic"), 11);
			var apartment3 = new Apartment(partitionSpace1);
			building.AddApartment(apartment3);

			partitionSpace4 = landRegistry.CreatePartitionSpace(23, "Stan 4", new PhysicalPerson("12345678906", "Mile4", "Milic"), 49);
			var apartment4 = new Apartment(partitionSpace1);
			building.AddApartment(apartment4);
		}

		[Test]
		public void Can_Create_AdministrationJobsVoting() {
			// Arrange

			// Act
			AdministrationJobsVoting administrationJobsVoting = new AdministrationJobsVoting(AdministrationJobsType.Regular, building); 

			// Assert
			Assert.IsNotNull(administrationJobsVoting);
		}

		[Test]
		public void If_Regular_AdministrationJobType_Then_More_Than_Half_Positive_Votes_Is_Success() {
			// Arrange
			AdministrationJobsVoting administrationJobsVoting = new AdministrationJobsVoting(AdministrationJobsType.Regular, building);

			// Act
			administrationJobsVoting.AddVote(new OwnerVote(true, partitionSpace1));
			administrationJobsVoting.AddVote(new OwnerVote(true, partitionSpace2));
			administrationJobsVoting.AddVote(new OwnerVote(false, partitionSpace4));
			administrationJobsVoting.AddVote(new OwnerVote(true, partitionSpace3));

			// Assert
			Assert.IsTrue(administrationJobsVoting.IsFinished);
			Assert.IsTrue(administrationJobsVoting.IsAccepted);
		}

		[Test]
		public void If_Extraordinary_AdministrationJobType_Then_All_Positive_Votes_Is_Success() {
			// Arrange
			AdministrationJobsVoting administrationJobsVoting = new AdministrationJobsVoting(AdministrationJobsType.Extraordinary, building);

			// Act
			administrationJobsVoting.AddVote(new OwnerVote(true, partitionSpace1));
			administrationJobsVoting.AddVote(new OwnerVote(true, partitionSpace2));
			administrationJobsVoting.AddVote(new OwnerVote(true, partitionSpace4));
			administrationJobsVoting.AddVote(new OwnerVote(true, partitionSpace3));

			// Assert
			Assert.IsTrue(administrationJobsVoting.IsFinished);
			Assert.IsTrue(administrationJobsVoting.IsAccepted);
		}

	}
}

