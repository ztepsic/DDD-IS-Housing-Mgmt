using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.Legislature;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.Legislature {
	[TestFixture]
	public class LandRegistriesNHRepositoryTests : NHibernateFixture{

		[Test]
		public void Can_Save_LandRegistry() {
			// Arrange
			City city = new City(10000, "Zagreb");
			Cadastre cadastre = new Cadastre("Trnje", "12345", city);
			CadastralParticle cadastralParticle = new CadastralParticle(cadastre, "123", 23, "Opis");
			LandRegistry landRegistry = new LandRegistry(cadastralParticle);
			var person = new PhysicalPerson("12345678901", "Ime", "Prezime");

			landRegistry.CreatePartitionSpace("123", 12, "Opis etaže", person);

			LandRegistriesNHRepository landRegistriesNhRepository = new LandRegistriesNHRepository(SessionFactory);

			// Act
			LandRegistry fetchedLandRegistry;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = session.BeginTransaction()) {
					session.Save(city);
					session.Save(person);
					session.Save(cadastre);
					landRegistriesNhRepository.SaveOrUpdate(landRegistry);
					tx.Commit();
				}

				using (var tx = session.BeginTransaction()) {
					fetchedLandRegistry =
						landRegistriesNhRepository.GetByNumberOfCadastralParticle(cadastralParticle.NumberOfCadastralParticle);
					tx.Commit();
				}
			}

			// Assert
			Assert.AreEqual(landRegistry, fetchedLandRegistry, "LandRegistries aren't equal");
			Assert.IsTrue(fetchedLandRegistry.PartitionSpaces.Count == 1, "LandRegistry haven't exactly one partition space.");
		}

	}
}
