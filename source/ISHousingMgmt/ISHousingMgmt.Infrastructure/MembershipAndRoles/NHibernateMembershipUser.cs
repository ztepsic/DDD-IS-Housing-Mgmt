using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using ISHousingMgmt.Domain.MembershipAndRoles;

namespace ISHousingMgmt.Infrastructure.MembershipAndRoles {
	public class NHibernateMembershipUser : MembershipUser {

		#region Members

		private HousingMgmtUser user;

		public HousingMgmtUser User { get { return user; } }

		#region Overrides of MembershipUser

		/// <summary>
		/// Dohvaca korisnicko ime
		/// </summary>
		public new string UserName { get { return user.UserName; } }

		/// <summary>
		/// Dohvaca primarni kljuc
		/// </summary>
		public new object ProviderUserKey { get { return user.Id; } }

		/// <summary>
		/// Dohvaca Email adresu
		/// </summary>
		public new string Email { get { return user.Email; } }

		/// <summary>
		/// Dohvaca pitanje
		/// </summary>
		public new string PasswordQuestion { get { return user.PasswordQuestion; } }

		/// <summary>
		/// Dohvaca komentar
		/// </summary>
		public new string Comment { get { return user.Comment; } }

		/// <summary>
		/// Idikacija da li je korisnik odobren
		/// </summary>
		public new bool IsApproved { get { return user.IsApproved; } }

		/// <summary>
		/// Indikacija da li je koirsnik zakljucan
		/// </summary>
		public new bool IsLockedOut { get { return user.IsLockedOut; } }

		/// <summary>
		/// Dohvaca datum i vrijeme posljednjeg zakljucavanja
		/// </summary>
		public new DateTime LastLockoutDate { get { return user.LastLockoutDate; } }

		/// <summary>
		/// Dohvaca datum i vrijeme stvaranja korisnika
		/// </summary>
		public new DateTime CreationDate { get { return user.CreationDate; } }

		/// <summary>
		/// Dohvaca datum i vrijeme posljednje prijave korisnika
		/// </summary>
		public new DateTime LastLoginDate { get { return user.LastLoginDate; } }

		/// <summary>
		/// Dohvaca datum i vrijeme posljednje aktivnosti korisnika
		/// </summary>
		public new DateTime LastActivityDate { get { return user.LastActivityDate; } }

		/// <summary>
		/// Dohvaca datum i vrijeme posljednje promjene lozinke
		/// </summary>
		public new DateTime LastPasswordChangedDate { get { return user.LastPasswordChangedDate; } }

		//public new bool IsOnline { get { return user.IsOnline()}}

		#endregion

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="user">korisnik</param>
		public NHibernateMembershipUser(HousingMgmtUser user) {
			this.user = user;
		}

		#endregion

		#region Methods

		#region Overrides of MembershipUser

		#endregion

		#endregion

	}
}
