using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.PersonsAndRoles {
	[TestFixture]
	public class ContractorsNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Save_Contractor_To_DB() {
			// Arrange
			LegalPerson legalPerson = new LegalPerson("12345678901", "Ime");
			Contractor contractor = new Contractor(legalPerson);

			ContractorsNHRepository contractorNhRepository = new ContractorsNHRepository(SessionFactory);

			// Act
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					contractorNhRepository.SaveOrUpdate(contractor);
					tx.Commit();
				}

			}

			IList<Contractor> contractors = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					contractors = contractorNhRepository.GetAll().ToList();
					tx.Commit();
				}

			}

			// Assert
			Assert.IsTrue(contractors.Count > 0, "Contractor has been saved to database.");
		}

	}
}
