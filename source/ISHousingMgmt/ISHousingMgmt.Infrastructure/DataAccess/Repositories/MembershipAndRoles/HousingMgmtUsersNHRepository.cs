using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.MembershipAndRoles;
using ISHousingMgmt.Domain.PersonsAndRoles;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.MembershipAndRoles {
	public class HousingMgmtUsersNHRepository : UsersNHRepository, IHousingMgmtUsersRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public HousingMgmtUsersNHRepository() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public HousingMgmtUsersNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca korisnika na temelju osobe
		/// </summary>
		/// <param name="person">osoba</param>
		/// <returns>korisnik</returns>
		public HousingMgmtUser GetUserByPerson(Person person) {
			return Session.QueryOver<HousingMgmtUser>()
				.Where(hmu => hmu.Person.Id == person.Id)
				.SingleOrDefault();
		}

		#endregion

	}
}
