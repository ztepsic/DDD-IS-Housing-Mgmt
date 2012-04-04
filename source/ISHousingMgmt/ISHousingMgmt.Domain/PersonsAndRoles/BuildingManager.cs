using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;

namespace ISHousingMgmt.Domain.PersonsAndRoles {
	/// <summary>
	/// Razred(Entity) koji predstavlja upravitelja zgrade
	/// </summary>
	public class BuildingManager : NHibernateEntity {

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
		/// Izvodaci radova
		/// </summary>
		private Iesi.Collections.Generic.ISet<Contractor> contractors;

		/// <summary>
		/// Dohvaca izvodace radova
		/// </summary>
		public virtual Iesi.Collections.Generic.ISet<Contractor> Contractors {
			get { return new ImmutableSet<Contractor>(contractors); }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected BuildingManager() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="legalPerson">pravna osoba</param>
		/// <param name="contractors">izvodaci radova</param>
		public BuildingManager(LegalPerson legalPerson, ICollection<Contractor> contractors) {
			this.legalPerson = legalPerson;
			this.contractors = new HashedSet<Contractor>(contractors);
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="legalPerson">pravna osoba</param>
		public BuildingManager(LegalPerson legalPerson) : this(legalPerson, new HashedSet<Contractor>()) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dodaje izvodaca radova
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		public virtual void AddContractor(Contractor contractor) {
			contractors.Add(contractor);
		}

		/// <summary>
		/// Brise izvodaca radoba
		/// </summary>
		/// <param name="contractor"></param>
		/// <returns>true ako je brisanje uspjelo, inace false</returns>
		public virtual bool RemoveContractor(Contractor contractor) {
			return contractors.Remove(contractor);
		}

		#endregion
	}
}
