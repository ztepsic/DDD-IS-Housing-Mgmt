using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.BuildingMaintenance;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
	/// <summary>
	/// Razred(Entity) koji predstavlja izvodaca radova - kooperanta
	/// </summary>
	public class Contractor : EntityBase {

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
		/// Poslove koje obavlja izvodac radova
		/// </summary>
		private readonly IList<RepairService> repairServices;

		/// <summary>
		/// Dohvaca poslove koje obavlja izvodac radova
		/// </summary>
		public IList<RepairService> RepairServices {
			get { return new ReadOnlyCollection<RepairService>(repairServices); }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="legalPerson">pravna osoba</param>
		public Contractor(LegalPerson legalPerson) :this(legalPerson, new List<RepairService>()) {
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="legalPerson">pravna osoba</param>
		/// <param name="repairServices">usluge koje obavlja izovdac radova</param>
		public Contractor(LegalPerson legalPerson, IEnumerable<RepairService> repairServices) {
			this.legalPerson = legalPerson;
			this.repairServices = repairServices.ToList();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Dodaje uslugu koje obavlja izvodac radova
		/// </summary>
		/// <param name="repairService"></param>
		public Contractor AddRepairService(RepairService repairService) {
			repairServices.Add(repairService);
			return this;
		}

		/// <summary>
		/// Brise uslugu koju obavlja izvodac radova
		/// </summary>
		/// <param name="repairService"></param>
		/// <returns></returns>
		public bool RemoveRepairService(RepairService repairService) {
			return repairServices.Remove(repairService);
		}

		#endregion
	}
}
