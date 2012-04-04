using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.Tests.Helpers;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.Legislature;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.Legislature {
	[TestFixture]
	public class CadastresNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Read_Cadastre_From_DB() {
			// Arrange
			City city = new City(10000, "Zagreb");
			Cadastre cadastre = new Cadastre("Črnomerec", "335266", city);

			CadastresNHRepository cadastresNhRepository = new CadastresNHRepository(SessionFactory);

			// Act
			Cadastre cadastreFromDb = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = session.BeginTransaction()) {
					session.Save(city);
					session.Save(cadastre);
					tx.Commit();
				}

				using (var tx = session.BeginTransaction()) {
					cadastreFromDb = cadastresNhRepository.GetById(1);
					tx.Commit();
				}
			}

			// Assert
			Assert.AreEqual(cadastreFromDb, cadastre, "Two Cadastre entities aren't equal.");
		}


	}

}