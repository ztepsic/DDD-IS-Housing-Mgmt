using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BuildingMaintenance;

namespace ISHousingMgmt.Domain.PersonsAndRoles {
	/// <summary>
	/// Razred(Entity) koji predstavlja izvodaca radova - kooperanta
	/// </summary>
	public class Contractor : NHibernateEntity {

		#region Members

		/// <summary>
		/// Pravna osoba
		/// </summary>
		[BusinessKeyOfEntity]
		private LegalPerson legalPerson;

		/// <summary>
		/// Dohvaca pravnu osobu
		/// </summary>
		public virtual LegalPerson LegalPerson {
			get { return legalPerson; }
		}

		/// <summary>
		/// Poslove koje obavlja izvodac radova
		/// </summary>
		private Iesi.Collections.Generic.ISet<RepairService> repairServices;

		/// <summary>
		/// Dohvaca poslove koje obavlja izvodac radova
		/// </summary>
		public virtual Iesi.Collections.Generic.ISet<RepairService> RepairServices {
			get { return new ImmutableSet<RepairService>(repairServices); }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected Contractor() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="legalPerson">pravna osoba</param>
		public Contractor(LegalPerson legalPerson)
			: this(legalPerson, new HashedSet<RepairService>()) {
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="legalPerson">pravna osoba</param>
		/// <param name="repairServices">usluge koje obavlja izovdac radova</param>
		public Contractor(LegalPerson legalPerson, ICollection<RepairService> repairServices) {
			this.legalPerson = legalPerson;
			this.repairServices = new HashedSet<RepairService>(repairServices);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Dodaje uslugu koje obavlja izvodac radova
		/// </summary>
		/// <param name="repairService"></param>
		public virtual Contractor AddRepairService(RepairService repairService) {
			repairServices.Add(repairService);
			return this;
		}

		/// <summary>
		/// Brise uslugu koju obavlja izvodac radova
		/// </summary>
		/// <param name="repairService"></param>
		/// <returns></returns>
		public virtual bool RemoveRepairService(RepairService repairService) {
			return repairServices.Remove(repairService);
		}

		#endregion

	}
}
