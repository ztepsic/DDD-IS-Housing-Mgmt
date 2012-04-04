using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.BuildingMaintenance {
	/// <summary>
	/// Razred(Value Object) koji predstavlja zahtjev za popravkom
	/// </summary>
	public class MaintenanceRequest {

		#region Members

		/// <summary>
		/// Osoba koja je izdala zahtjev za popravkom
		/// </summary>
		private readonly PersonSnapshot submitter;

		/// <summary>
		/// Osoba koja je izdala zahtjev za popravkom
		/// </summary>
		public PersonSnapshot Submitter {
			get { return submitter; }
		}

		/// <summary>
		/// Naslov incidenta
		/// </summary>
		private readonly string subject;

		/// <summary>
		/// Naslov incidenta
		/// </summary>
		public string Subject {
			get { return subject; }
		}

		/// <summary>
		/// Opis incidenta
		/// </summary>
		private readonly string description;

		/// <summary>
		/// Opis incidenta
		/// </summary>
		public string Descritpion {
			get { return description; }
		}

		/// <summary>
		/// Lokacija incidenta
		/// </summary>
		private readonly string location;

		/// <summary>
		/// Lokacija incidenta
		/// </summary>
		public string Location {
			get { return location; }
		}

		/// <summary>
		/// Datum zahtjeva
		/// </summary>
		private readonly DateTime dateTimeOfRequest;

		/// <summary>
		/// Dohvaca datum zahtjeva
		/// </summary>
		public DateTime DateTimeOfRequest {
			get { return dateTimeOfRequest; }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
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
