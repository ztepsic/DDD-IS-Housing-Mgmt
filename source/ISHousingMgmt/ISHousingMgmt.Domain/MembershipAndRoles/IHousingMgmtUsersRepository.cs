using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.MembershipAndRoles {
	public interface IHousingMgmtUsersRepository : IUsersRepository {

		/// <summary>
		/// Dohvaca korisnika na temelju osobe
		/// </summary>
		/// <param name="person">osoba</param>
		/// <returns>korisnik</returns>
		HousingMgmtUser GetUserByPerson(Person person);

	}
}
