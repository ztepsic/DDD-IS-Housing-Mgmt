using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.BuildingMaintenance {
	public interface IMaintenancesRepository : ICrudRepository<Maintenance> {

		/// <summary>
		/// Dohaca sve kvarove koje je prijavila zadana osoba
		/// </summary>
		/// <param name="submitter">osoba koja je prijavila kvarove</param>
		/// <returns>kvarovi prijavljeni od strane zadane osobe</returns>
		IList<Maintenance> GetAllMaintenancesBySubmitter(Person submitter);

		/// <summary>
		/// Dohaca sve kvarove koje je prijavila zadana osoba za zadanu zgradu
		/// </summary>
		/// <param name="submitter">osoba koja je prijavila kvarove</param>
		/// <param name="buildingId">identifikator zgrade</param>
		/// <returns>kvarovi prijavljeni od strane zadane osobe</returns>
		IList<Maintenance> GetAllMaintenancesBySubmitter(Person submitter, int buildingId);

		/// <summary>
		/// Dohaca sve kvarove za zadanu zgrade
		/// </summary>
		/// <param name="buildingId">identifikator zgrade</param>
		/// <returns>kvarovi za zadanu zgradu</returns>
		IList<Maintenance> GetAllMaintenancesByBuilding(int buildingId);

		/// <summary>
		/// Dohvaca sve kvarove za zadanog izvodaca radova
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		/// <returns>svi kvarovi</returns>
		IList<Maintenance> GetAllMaintenancesByContractor(Contractor contractor);

		/// <summary>
		/// Dohvaca sve kvarove za zadanog izvodaca radova
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		/// <returns>svi kvarovi</returns>
		IList<Maintenance> GetAllMaintenancesByContractor(LegalPerson contractor);

		/// <summary>
		/// Dohvaca sve zavrsne popravke za koje nije izdan racun od strane izvodaca radova
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		/// <returns>zavrseni popraviz za kojie nije izdan racun</returns>
		IList<Maintenance> GetAllMaintenancesWithNoBillByContractor(LegalPerson contractor);


		/// <summary>
		/// Dohvaca sve zavrsne popravke za koje nije izdan racun od strane izvodaca radova
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		/// <param name="limit">broj popravaka koji se dohvaca</param>
		/// <returns>zavrseni popraviz za kojie nije izdan racun</returns>
		IList<Maintenance> GetAllMaintenancesByContractor(LegalPerson contractor, int limit);
	}
}
