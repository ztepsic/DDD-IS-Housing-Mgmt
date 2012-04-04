using System;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.BuildingManagement {
	[TestFixture]
	public class AdministrationJobsVotingTests {

		private Cadastre cadastre;
		private CadastralParticle cadastralParticle;
		private LandRegistry landRegistry;
		private Building building;
		private IPartitionSpace partitionSpace1;
		private IPartitionSpace partitionSpace2;
		private IPartitionSpace partitionSpace3;
		private IPartitionSpace partitionSpace4;

		[SetUp]
		public void SetUp() {
			cadastre = new Cadastre("Trešnjevka", "332134", new City(10000, "Zagreb"));
			cadastralParticle = new CadastralParticle(cadastre, "123", 120, "opis");
			landRegistry = new LandRegistry(cadastralParticle);

			var buildingManager = new BuildingManager(new LegalPerson("12345678903", "Upravitelj"));

			building = new Building(buildingManager) {
				LandRegistry = landRegistry
			};

			partitionSpace1 = landRegistry.CreatePartitionSpace("123", 23, "Stan 1", new PhysicalPerson("12345678903", "Mile1", "Milic"));
			partitionSpace2 = landRegistry.CreatePartitionSpace("123", 23, "Stan 2", new PhysicalPerson("12345678904", "Mile2", "Milic"));
			partitionSpace3 = landRegistry.CreatePartitionSpace("123", 23, "Stan 3", new PhysicalPerson("12345678905", "Mile3", "Milic"));
			partitionSpace4 = landRegistry.CreatePartitionSpace("123", 23, "Stan 4", new PhysicalPerson("12345678906", "Mile4", "Milic"));
		}

		[Test]
		public void Can_Create_AdministrationJobsVoting() {
			// Arrange

			// Act
			AdministrationJobsVoting administrationJobsVoting = new AdministrationJobsVoting(AdministrationJobsType.Regular, building,
				"Subject", "Description", new DateTime(2011, 11, 11)); 

			// Assert
			Assert.IsNotNull(administrationJobsVoting);
		}

		[Test]
		public void If_Regular_AdministrationJobType_Then_More_Than_Half_Positive_Votes_Is_Success() {
			// Arrange
			AdministrationJobsVoting administrationJobsVoting = new AdministrationJobsVoting(AdministrationJobsType.Regular, building,
				"Subject", "Description", new DateTime(2011, 11, 11)); 

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
			AdministrationJobsVoting administrationJobsVoting = new AdministrationJobsVoting(AdministrationJobsType.Extraordinary, building,
				"Subject", "Description", new DateTime(2011, 11, 11)); 

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

