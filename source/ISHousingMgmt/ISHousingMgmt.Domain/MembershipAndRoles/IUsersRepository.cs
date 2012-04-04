using System.Collections.Generic;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.MembershipAndRoles {
	/// <summary>
	/// Repozitorij korisnika
	/// </summary>
	public interface IUsersRepository : ICrudRepository<User> {

		/// <summary>
		/// Dohvaca korisnika na temelju korisnickog imena
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>korisnik za zadano korisnicko ime</returns>
		User GetUser(string username);

		/// <summary>
		/// Dohvaca korisnika na temelju email adrese
		/// </summary>
		/// <param name="email">email adresa</param>
		/// <returns>korisnik koji posjeduje zadanu email adresu</returns>
		User GetUserByEmail(string email);

		/// <summary>
		/// Dohvaca korisnike cije korisnicko ime sadrzi zadatni oblik
		/// </summary>
		/// <param name="usernameToMatch">uvjet kojeg mora zadovoljiti korisnicko ime</param>
		/// <returns>korisnici cije korisnicko ime zadovoljava zadani uvjet</returns>
		IList<User> GetUsersByUserName(string usernameToMatch);

		/// <summary>
		/// Dohvaca korisnike koji posjeduju zadanu email adresu
		/// </summary>
		/// <param name="email">zadana email adresa</param>
		/// <returns>korisnici koji posjeduju zadanu email adresu</returns>
		IList<User> GetUsersByEmailAddress(string email);

		/// <summary>
		/// Dohvaca korisnicko ime na temelju email adrese
		/// </summary>
		/// <param name="email">email adresa</param>
		/// <returns>Korisnicko ime asocirano sa zadanom email adresom. Ukoliko korisnicko ime nije pronadeno
		/// za zadanu email adresu vraca null.</returns>
		string GetUserNameByEmail(string email);

		/// <summary>
		/// Brise korisnika na temelju korisnickog imena
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>true ukoliko je brisanje uspjelo, inace false</returns>
		bool DeleteUserByUserName(string username);

		/// <summary>
		/// Dohvaca broj korisnika koji su trenutno online.
		/// </summary>
		/// <param name="userIsOnlineTimeWindow">specificira broj minuta od posljednjem datuma i vremena aktivnosti untar kojih
		/// se korisnik smatra da je online</param>
		/// <returns>broj korisnika koji su trenutno online</returns>
		int GetNumberOfUsersOnline(int userIsOnlineTimeWindow);
	}
}
