using ISHousingMgmt.Domain;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories {
	[TestFixture]
	class CityNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Save_City_To_DB() {
			// Arrange
			CitiesNHRepository cityNhRepository = null;
			City city = new City(10000, "Zagreb");

			// Act
			City cityFromDb = null;
			using (var session = SessionFactory.OpenSession()) {
				using(var tx = session.BeginTransaction()) {
					session.Save(city);
					//var res = session.CreateSQLQuery("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;")
					//    .AddScalar("name", NHibernateUtil.String)
					//    .List();
					tx.Commit();
				}

				using (var tx = session.BeginTransaction()) {
					cityNhRepository = new CitiesNHRepository(SessionFactory);
					cityFromDb = cityNhRepository.GetCityByPostalCode(10000);
					tx.Commit();
				}
			}

			// Assert
			Assert.AreEqual(city, cityFromDb, "Two City entities aren't equal.");
		}

		[Test]
		public void Can_Get_City_By_PostalCode() {
			// Arrange
			

			// Act

			// Assert
		}

	}
}
