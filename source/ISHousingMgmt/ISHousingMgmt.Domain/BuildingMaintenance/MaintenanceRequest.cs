using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.BuildingMaintenance {
	/// <summary>
	/// Razred(Value Object) koji predstavlja zahtjev za popravkom
	/// </summary>
	public class MaintenanceRequest : ValueObject {

		#region Members

		/// <summary>
		/// Osoba koja je izdala zahtjev za popravkom
		/// </summary>
		private readonly PersonSnapshot submitter;

		/// <summary>
		/// Osoba koja je izdala zahtjev za popravkom
		/// </summary>
		public virtual PersonSnapshot Submitter { get { return submitter; } }

		/// <summary>
		/// Naslov incidenta
		/// </summary>
		private readonly string subject;

		/// <summary>
		/// Naslov incidenta
		/// </summary>
		public virtual string Subject { get { return subject; } }

		/// <summary>
		/// Opis incidenta
		/// </summary>
		private readonly string description;

		/// <summary>
		/// Opis incidenta
		/// </summary>
		public virtual string Description { get { return description; } }

		/// <summary>
		/// Lokacija incidenta
		/// </summary>
		private readonly string location;

		/// <summary>
		/// Lokacija incidenta
		/// </summary>
		public virtual string Location { get { return location; } }

		/// <summary>
		/// Datum zahtjeva
		/// </summary>
		private readonly DateTime dateTimeOfRequest;

		/// <summary>
		/// Dohvaca datum zahtjeva
		/// </summary>
		public virtual DateTime DateTimeOfRequest { get { return dateTimeOfRequest; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		private MaintenanceRequest() {
			submitter = null;
			subject = null;
			description = null;
			location = null;
			dateTimeOfRequest = DateTime.MinValue;
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="submitter">osoba koja prijavljuje kvar</param>
		/// <param name="subject">naziv kvara</param>
		/// <param name="description">opis kvara</param>
		/// <param name="location">lokacija gdje se kvar desio</param>
		public MaintenanceRequest(Person submitter, string subject, string description, string location) {
			this.submitter = new PersonSnapshot(submitter);
			this.subject = subject;
			this.description = description;
			this.location = location;
			dateTimeOfRequest = DateTime.Now;
		}

		#endregion

		#region Methods
		#endregion
	}
}
