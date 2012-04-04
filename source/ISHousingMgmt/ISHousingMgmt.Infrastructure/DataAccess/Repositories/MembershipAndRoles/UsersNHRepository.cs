using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.MembershipAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.MembershipAndRoles {
	public class UsersNHRepository : NHibernateCrudRepository<User>, IUsersRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public UsersNHRepository() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public UsersNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca korisnika na temelju korisnickog imena
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>korisnik za zadano korisnicko ime</returns>
		public User GetUser(string username) {
			var query = @"from User user where user.UserName = :username";

			return Session.CreateQuery(query)
				.SetString("username", username)
				.UniqueResult() as User;

		}

		/// <summary>
		/// Dohvaca korisnika na temelju email adrese
		/// </summary>
		/// <param name="email">email adresa</param>
		/// <returns>korisnik koji posjeduje zadanu email adresu</returns>
		public User GetUserByEmail(string email) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Dohvaca korisnike cije korisnicko ime sadrzi zadatni oblik
		/// </summary>
		/// <param name="usernameToMatch">uvjet kojeg mora zadovoljiti korisnicko ime</param>
		/// <returns>korisnici cije korisnicko ime zadovoljava zadani uvjet</returns>
		public IList<User> GetUsersByUserName(string usernameToMatch) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Dohvaca korisnike koji posjeduju zadanu email adresu
		/// </summary>
		/// <param name="email">zadana email adresa</param>
		/// <returns>korisnici koji posjeduju zadanu email adresu</returns>
		public IList<User> GetUsersByEmailAddress(string email) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Dohvaca korisnicko ime na temelju email adrese
		/// </summary>
		/// <param name="email">email adresa</param>
		/// <returns>Korisnicko ime asocirano sa zadanom email adresom. Ukoliko korisnicko ime nije pronadeno
		/// za zadanu email adresu vraca null.</returns>
		public string GetUserNameByEmail(string email) {
			var query = @"select user.UserName from User user
							where user.Email = :email";

			return (string) Session.CreateQuery(query)
				.SetString("email", email)
				.UniqueResult();
		}

		/// <summary>
		/// Brise korisnika na temelju korisnickog imena
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>true ukoliko je brisanje uspjelo, inace false</returns>
		public bool DeleteUserByUserName(string username) {
			var queryString = @"delete from User user where user.UserName=:username and user.Version=:version";
			int result = Session.CreateQuery(queryString)
				.SetString("username", username)
				.SetInt32("version", 1)
				.ExecuteUpdate();
			return result > 0;
		}

		/// <summary>
		/// Dohvaca broj korisnika koji su trenutno online.
		/// </summary>
		/// <param name="userIsOnlineTimeWindow">specificira broj minuta od posljednjem datuma i vremena aktivnosti untar kojih
		/// se korisnik smatra da je online</param>
		/// <returns>broj korisnika koji su trenutno online</returns>
		public int GetNumberOfUsersOnline(int userIsOnlineTimeWindow) {
			throw new NotImplementedException();
		}

		#endregion
	}
}
