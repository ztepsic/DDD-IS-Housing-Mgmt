using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.BuildingManagement;
using BuildingManagement.DomainModel.Legislature;
using BuildingManagement.DomainModel.PersonsAndRoles;
using Moq;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.BuildingManagement {
	[TestFixture]
	class OwnerVoteTests {

		private Cadastre cadastre;
		private AbstractCadastralParticle cadastralParticle;
		private LandRegistry landRegistry;

		[SetUp]
		public void SetUp() {
			cadastre = new Cadastre("Trešnjevka", "332134", new City(10000, "Zagreb"));
			cadastralParticle = new CadastralParticle(cadastre, "123", 20, "opis");
			landRegistry = new LandRegistry(cadastralParticle);
		}

		[Test]
		public void Can_Create_OwnerVote() {
			// Arrange
			IPartitionSpace partitionSpace = landRegistry.CreatePartitionSpace(23, "Stan 1", new PhysicalPerson("12345678901", "Mile", "Milic"), 20);

			// Act
			OwnerVote ownerVote = new OwnerVote(true, partitionSpace);

			// Assert
			Assert.IsNotNull(ownerVote);
		}

	}
}
