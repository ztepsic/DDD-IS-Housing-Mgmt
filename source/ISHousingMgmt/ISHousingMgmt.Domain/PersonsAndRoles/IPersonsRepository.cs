using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.Finances;

namespace ISHousingMgmt.Domain.PersonsAndRoles {
	public interface IPersonsRepository : ICrudRepository<Person> {
		/// <summary>
		/// Dohvaca pravnu osobu preko oib-a
		/// </summary>
		/// <param name="oib">oib</param>
		/// <returns>pravna osoba</returns>
		LegalPerson GetLegalPersonByOib(string oib);

		/// <summary>
		/// Dohvaca fizicku osobu preko oib-a
		/// </summary>
		/// <param name="oib">oib</param>
		/// <returns>fizicka osoba</returns>
		PhysicalPerson GetPhysicalPersonByOib(string oib);

		/// <summary>
		/// Dohvaca osobu preko oib-a
		/// </summary>
		/// <param name="oib"></param>
		/// <returns></returns>
		Person GetByOib(string oib);

		/// <summary>
		/// Dohvaca pravnu osobu preko identifikatora
		/// </summary>
		/// <param name="id">identifikator osobe</param>
		/// <returns>pravna osoba</returns>
		LegalPerson GetLegalPersonById(int id);

		/// <summary>
		/// Dohvaca fizicku osobu preko identifikatora
		/// </summary>
		/// <param name="id">id</param>
		/// <returns>fizicka osoba</returns>
		PhysicalPerson GetPhysicalPersonById(int id);

		/// <summary>
		/// Dohvaca pravnu osobu preko korisnickog imena
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>pravna osoba</returns>
		LegalPerson GetLegalPersonByUsername(string username);

		/// <summary>
		/// Dohvaca fizičku osobu preko korisnickog imena
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>fizička osoba</returns>
		PhysicalPerson GetPhysicalPersonByUsername(string username);

		/// <summary>
		/// Dohvaca osobu preko korisnickog imena
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>osoba</returns>
		Person GetPersonByUsername(string username);

		/// <summary>
		/// Dohvaca sve stanare zgrade
		/// </summary>
		/// <param name="buildingId">dohvaca sve stanare zgrade</param>
		/// <returns></returns>
		IList<Person> GetTenantsFrom(int buildingId);

	}
}
