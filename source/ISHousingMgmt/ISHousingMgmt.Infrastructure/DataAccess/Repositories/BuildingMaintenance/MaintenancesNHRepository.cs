using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.BuildingMaintenance {
	public class MaintenancesNHRepository : NHibernateCrudRepository<Maintenance>, IMaintenancesRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public MaintenancesNHRepository() { }


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public MaintenancesNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dohaca sve kvarova koje je prijavila zadana osoba
		/// </summary>
		/// <param name="submitter">osoba koja je prijavila kvarove</param>
		/// <returns>kvarovi prijavljeni od strane zadane osobe</returns>
		public IList<Maintenance> GetAllMaintenancesBySubmitter(Person submitter) {
			return Session.QueryOver<Maintenance>()
				.Where(m => m.MaintenanceRequest.Submitter.Oib == submitter.Oib)
				.OrderBy(m => m.CompletitionDateTime).Desc
				.OrderBy(m => m.MaintenanceRequest.DateTimeOfRequest).Desc
				.OrderBy(m => m.StatusOfMaintenance).Asc
				.List();
		}

		/// <summary>
		/// Dohaca sve kvarove koje je prijavila zadana osoba za zadanu zgradu
		/// </summary>
		/// <param name="submitter">osoba koja je prijavila kvarove</param>
		/// <param name="buildingId">identifikator zgrade</param>
		/// <returns>kvarovi prijavljeni od strane zadane osobe</returns>
		public IList<Maintenance> GetAllMaintenancesBySubmitter(Person submitter, int buildingId) {
			return Session.QueryOver<Maintenance>()
				.Where(m => m.MaintenanceRequest.Submitter.Oib == submitter.Oib)
				.And(m => m.Building.Id == buildingId)
				.OrderBy(m => m.CompletitionDateTime).Desc
				.OrderBy(m => m.MaintenanceRequest.DateTimeOfRequest).Desc
				.OrderBy(m => m.StatusOfMaintenance).Asc
				.List();
		}

		/// <summary>
		/// Dohaca sve kvarove za zadanu zgrade
		/// </summary>
		/// <param name="buildingId">identifikator zgrade</param>
		/// <returns>kvarovi za zadanu zgradu</returns>
		public IList<Maintenance> GetAllMaintenancesByBuilding(int buildingId) {
			return Session.QueryOver<Maintenance>()
				.Where(m => m.Building.Id == buildingId)
				.OrderBy(m => m.CompletitionDateTime).Desc
				.OrderBy(m => m.MaintenanceRequest.DateTimeOfRequest).Desc
				.OrderBy(m => m.StatusOfMaintenance).Asc
				.List();
		}

		/// <summary>
		/// Dohvaca sve kvarove za zadanog izvodaca radova
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		/// <returns>svi kvarovi</returns>
		public IList<Maintenance> GetAllMaintenancesByContractor(Contractor contractor) {
			return GetAllMaintenancesByContractor(contractor.LegalPerson);
		}

		/// <summary>
		/// Dohvaca sve kvarove za zadanog izvodaca radova
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		/// <returns>svi kvarovi</returns>
		public IList<Maintenance> GetAllMaintenancesByContractor(LegalPerson contractor) {
			return Session.QueryOver<Maintenance>()
				.Where(m => m.Contractor.Oib == contractor.Oib)
				.OrderBy(m => m.CompletitionDateTime).Desc
				.OrderBy(m => m.MaintenanceRequest.DateTimeOfRequest).Desc
				.OrderBy(m => m.StatusOfMaintenance).Asc
				.List();
		}

		/// <summary>
		/// Dohvaca sve zavrsne popravke za koje nije izdan racun od strane izvodaca radova
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		/// <returns>zavrseni popraviz za kojie nije izdan racun</returns>
		public IList<Maintenance> GetAllMaintenancesWithNoBillByContractor(LegalPerson contractor) {
			return Session.QueryOver<Maintenance>()
				.Where(m => m.Contractor.Oib == contractor.Oib)
				.And(m => m.Bill == null)
				.OrderBy(m => m.CompletitionDateTime).Desc
				.OrderBy(m => m.MaintenanceRequest.DateTimeOfRequest).Desc
				.OrderBy(m => m.StatusOfMaintenance).Asc
				.List();
		}

		/// <summary>
		/// Dohvaca sve zavrsne popravke za koje nije izdan racun od strane izvodaca radova
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		/// <param name="limit">broj popravaka koji se dohvaca</param>
		/// <returns>zavrseni popraviz za kojie nije izdan racun</returns>
		public IList<Maintenance> GetAllMaintenancesByContractor(LegalPerson contractor, int limit) {
			return Session.QueryOver<Maintenance>()
				.Where(m => m.Contractor.Oib == contractor.Oib)
				.OrderBy(m => m.CompletitionDateTime).Desc
				.OrderBy(m => m.MaintenanceRequest.DateTimeOfRequest).Desc
				.OrderBy(m => m.StatusOfMaintenance).Asc
				.Take(limit)
				.List();
		}

		#endregion

	}
}
