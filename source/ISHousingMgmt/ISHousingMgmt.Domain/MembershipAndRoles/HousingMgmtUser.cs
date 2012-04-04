using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.MembershipAndRoles {
	/// <summary>
	/// Predstavlja korisnika aplikacije za upravljanje stambenim zgradama
	/// </summary>
	public class HousingMgmtUser : User {

		#region Members

		/// <summary>
		/// Osoba
		/// </summary>
		public virtual Person Person { get; set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor radi podrske NHibernate lazy loadingu
		/// </summary>
		protected HousingMgmtUser() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <param name="password">lozinka</param>
		/// <param name="passwordCoder">koder lozinke pomocu kojeg se lozinka kodira</param>
		public HousingMgmtUser(string username, string password, IPasswordCoder passwordCoder) 
			: base(username, password, passwordCoder) { }

		#endregion

		#region Methods

		#endregion

	}
}
