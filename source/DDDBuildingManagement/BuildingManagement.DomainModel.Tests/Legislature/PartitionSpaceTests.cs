using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Legislature;
using BuildingManagement.DomainModel.PersonsAndRoles;
using Moq;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.Legislature {
	[TestFixture]
	class PartitionSpaceTests {

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
		public void Can_Create_PartitionSpace() {
			// Arrange

			// Act
			var partitionSpace = landRegistry.CreatePartitionSpace(23, "Dvosoban stan");

			// Assert
			Assert.IsNotNull(partitionSpace);
		}

		[Test]
		public void Can_Create_Owned_PartitionSpace() {
			// Arrange
			var owner = new LegalPerson("12345678901", "Coca Cola");

			// Act
			var ownedPartitionSpace = landRegistry.CreatePartitionSpace(23, "Dvosoban stan", owner, 32);

			// Assert
			Assert.IsNotNull(ownedPartitionSpace);
		}

		[Test]
		[Ignore]
		public void Can_Find_PartitionSpace_Via_Id() {
			// Arrange
			Mock<IPartitionSpacesRepository> partitionSpacesRepositoryMock = new Mock<IPartitionSpacesRepository>();

			// potrebno je kreirati nekako PartitionSpace sa dodjeljenim ID-em
			partitionSpacesRepositoryMock.Setup(x => x.GetById(23)).Returns((PartitionSpace)null);

			// Act
			var partitionSpace = partitionSpacesRepositoryMock.Object.GetById(23);

			// Assert
			Assert.IsNotNull(partitionSpace, "Object was not created.");
			Assert.AreEqual(23, partitionSpace.Id, "Id's must be the same.");
		}

		[Test]
		public void Can_Find_PartitionSpace_Via_CadastralParticleNumber() {
			// Arrange

			// Act

			// Assert
		}

	}
}
