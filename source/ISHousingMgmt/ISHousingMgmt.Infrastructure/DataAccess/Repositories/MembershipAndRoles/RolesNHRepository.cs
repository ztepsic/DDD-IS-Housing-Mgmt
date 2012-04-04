using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.MembershipAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.MembershipAndRoles {
	public class RolesNHRepository : NHibernateCrudRepository<Role>, IRolesRepository {

		#region Members

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public RolesNHRepository() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public RolesNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca uloge za zadano korisnicko ime
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>uloge korisnikas</returns>
		public IList<Role> GetRolesForUser(string username) {
			string hql = @"select role
				from User user
				join user.Roles role
				where user.UserName = :username";

			return Session.CreateQuery(hql)
				.SetString("username", username)
				.List<Role>();
		}

		/// <summary>
		/// Provjerava da li zadani korisnik pripada zadanoj ulozi
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <param name="roleName">korisnicka uloga</param>
		/// <returns>true ukoliko korisnik pripada zadanoj ulozi, inace false</returns>
		public bool IsUserInRole(string username, string roleName) {
			string hql = @"select count(*)
				from User user
				join user.Roles role
				where user.UserName = :username and
				role.Name = :rolename";

			var isUserInRoleCount = (int) Session.CreateQuery(hql)
				.SetString("username", username)
				.SetString("rolename", roleName)
				.UniqueResult();

			return isUserInRoleCount == 1;
		}

		/// <summary>
		/// Dohvaca ulogu za zadano ime uloge
		/// </summary>
		/// <param name="roleName">ime uloge</param>
		/// <returns>uloga</returns>
		public Role GetRole(string roleName) {
			return Session.QueryOver<Role>()
				.Where(r => r.Name == roleName)
				.SingleOrDefault();
		}

		#endregion
	}
}
