using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.MembershipAndRoles {
	public interface IRolesRepository : ICrudRepository<Role> {

		/// <summary>
		/// Dohvaca uloge za zadano korisnicko ime
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>uloge korisnikas</returns>
		IList<Role> GetRolesForUser(string username);

		/// <summary>
		/// Provjerava da li zadani korisnik pripada zadanoj ulozi
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <param name="roleName">korisnicka uloga</param>
		/// <returns>true ukoliko korisnik pripada zadanoj ulozi, inace false</returns>
		bool IsUserInRole(string username, string roleName);

		/// <summary>
		/// Dohvaca ulogu za zadano ime uloge
		/// </summary>
		/// <param name="roleName">ime uloge</param>
		/// <returns>uloga</returns>
		Role GetRole(string roleName);

	}
}
