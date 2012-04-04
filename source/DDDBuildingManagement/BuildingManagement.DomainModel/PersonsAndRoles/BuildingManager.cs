using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.BuildingMaintenance;
using BuildingManagement.DomainModel.BusinessRulesAndValidation;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
	/// <summary>
	/// Razred(Entity) koji predstavlja upravitelja zgrade
	/// </summary>
	public class BuildingManager : EntityBase {

		#region Members

		/// <summary>
		/// Pravna osoba
		/// </summary>
		private readonly LegalPerson legalPerson;

		/// <summary>
		/// Dohvaca pravnu osobu
		/// </summary>
		public LegalPerson LegalPerson {
			get { return legalPerson; }
		}

		/// <summary>
		/// Izvodaci radova
		/// </summary>
		private readonly IList<Contractor> contractors;

		/// <summary>
		/// Dohvaca izvodace radova
		/// </summary>
		public IList<Contractor> Contractors {
			get { return new ReadOnlyCollection<Contractor>(contractors); }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="legalPerson">pravna osoba</param>
		/// <param name="contractors">izvodaci radova</param>
		public BuildingManager(LegalPerson legalPerson, IEnumerable<Contractor> contractors) {
			this.legalPerson = legalPerson;
			this.contractors = contractors.ToList();
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="legalPerson">pravna osoba</param>
		public BuildingManager(LegalPerson legalPerson) : this(legalPerson, new List<Contractor>()) {}

		#endregion

		#region Methods

		/// <summary>
		/// Dodaje izvodaca radova
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		public void AddContractor(Contractor contractor) {
			contractors.Add(contractor);
		}

		/// <summary>
		/// Brise izvodaca radoba
		/// </summary>
		/// <param name="contractor"></param>
		/// <returns>true ako je brisanje uspjelo, inace false</returns>
		public bool RemoveContractor(Contractor contractor) {
			return contractors.Remove(contractor);
		}

		/// <summary>
		/// Dodjeljuje izvodaca radova na popravak ili odrzavanje
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		/// <param name="maintenance">popravak ili odrzavanje</param>
		public void SetContractorForMaintenance(Contractor contractor, Maintenance maintenance) {
			var errors = new RulesException<BuildingManager>();

			if(!contractors.Contains(contractor)) {
				errors.ErrorForModel("Contractor is not working for BuildingManager, so it's invalid.");
			}

			if (!Equals(maintenance.BuildingManager)) {
				errors.ErrorForModel("This BuildingManager is not in charge of a builing in given maintenance.");
			}

			if (!contractor.RepairServices.Contains(maintenance.ServiceType)) {
				errors.ErrorForModel("This Contractor can't do needed service.");
			}

			if(errors.Errors.Any()) {
				throw errors;
			}

			maintenance.SetContractor(contractor);
		}




		#endregion
	}
}
