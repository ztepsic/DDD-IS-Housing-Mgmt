using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
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
		LegalPerson GetLegalPersonById(object id);

		/// <summary>
		/// Dohvaca fizicku osobu preko identifikatora
		/// </summary>
		/// <param name="id">id</param>
		/// <returns>fizicka osoba</returns>
		PhysicalPerson GetPhysicalPersonById(object id);

		/// <summary>
		/// Dohvaca sve stanare zgrade
		/// </summary>
		/// <param name="buildingId">dohvaca sve stanare zgrade</param>
		/// <returns></returns>
		IList<Person> GetTenantsFrom(object buildingId);

	}
}
