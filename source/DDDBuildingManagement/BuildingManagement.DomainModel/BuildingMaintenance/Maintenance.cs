using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.BuildingManagement;
using BuildingManagement.DomainModel.BusinessRulesAndValidation;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.BuildingMaintenance {
	/// <summary>
	/// Razred(Entity) koji predstavlja proces odrzavanja
	/// </summary>
	public class Maintenance : EntityBase {

		#region Members

		/// <summary>
		/// Vrsta usluge
		/// </summary>
		private RepairService serviceType;

		/// <summary>
		/// Dohvaca vrstu usluge
		/// </summary>
		public RepairService ServiceType { get { return serviceType; } }

		/// <summary>
		/// Dohvaca upravitelja zgrade
		/// </summary>
		public BuildingManager BuildingManager {
			get { return building.BuildingManager; }
		}

		/// <summary>
		/// Hitnost
		/// </summary>
		private Urgency urgency;

		/// <summary>
		/// Dohvaca hitnost
		/// </summary>
		public Urgency Urgency { get { return urgency; } }

		/// <summary>
		/// Status popravka
		/// </summary>
		public StatusOfMaintenance statusOfMaintenance;

		/// <summary>
		/// Dohvaca status popravka
		/// </summary>
		public StatusOfMaintenance StatusOfMaintenance { get { return statusOfMaintenance; } }

		/// <summary>
		/// Zgrada u kojoj se popravak obavlja
		/// </summary>
		private readonly Building building;

		/// <summary>
		/// Dohvaca zgradu u kojoj se obavlja poparavak
		/// </summary>
		public Building Building {
			get { return building; }
		}

		/// <summary>
		/// Zahtjev za popravkom
		/// </summary>
		private readonly MaintenanceRequest maintenanceRequest;

		/// <summary>
		/// Dohvaca zahtjev za popravkom
		/// </summary>
		public MaintenanceRequest MaintenanceRequest {
			get { return maintenanceRequest; }
		}

		/// <summary>
		/// Izvodac radova
		/// </summary>
		private PersonSnapshot contractor;

		/// <summary>
		/// Dohvaca ili postavlja izvodaca radova
		/// </summary>
		public PersonSnapshot Contractor {
			get { return contractor; }
		}

		/// <summary>
		/// Dodatne instrukcije
		/// </summary>
		public string Instructions { get; set; }

		/// <summary>
		/// Datum i vrijeme zavrsetka
		/// </summary>
		private DateTime completitionDateTime;

		/// <summary>
		/// Dohvaca datum i vrijeme zavrsetka
		/// </summary>
		public DateTime CompletitionDateTime { get { return completitionDateTime; } }

		#endregion

		#region Constructors and Init

		public Maintenance(MaintenanceRequest maintenanceRequest, Urgency urgency, RepairService serviceType, Building building) {
			this.maintenanceRequest = maintenanceRequest;
			this.building = building;
			this.urgency = urgency;
			this.serviceType = serviceType;
			statusOfMaintenance = StatusOfMaintenance.NotStarted;
		}

		#endregion

		#region Methods

		public void SetDone() {
			statusOfMaintenance = StatusOfMaintenance.Completed;
		}

		/// <summary>
		/// Postavlja izvodaca radova
		/// </summary>
		/// <param name="contractor"></param>
		internal void SetContractor(Contractor contractor) {
			this.contractor = new PersonSnapshot(contractor.LegalPerson);
			statusOfMaintenance = StatusOfMaintenance.InProgress;
		}

		/// <summary>
		/// Mijenja vrstu usluge
		/// </summary>
		/// <param name="serviceType">nova vrsta usluge</param>
		public void ChangeServiceType(RepairService serviceType) {
			this.serviceType = serviceType;
		}

		/// <summary>
		/// Postavlja hitnost
		/// </summary>
		/// <param name="urgency"></param>
		internal void ChangeUrgency(Urgency urgency) {
			this.urgency = urgency;
		}

		#endregion

	}
}
