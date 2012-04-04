using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.BuildingMaintenance {
	/// <summary>
	/// Razred(Entity) koji predstavlja proces odrzavanja
	/// </summary>
	public class Maintenance : NHibernateEntity {

		#region Members

		/// <summary>
		/// Vrsta usluge
		/// </summary>
		private RepairService serviceType;

		/// <summary>
		/// Dohvaca vrstu usluge
		/// </summary>
		public virtual RepairService ServiceType { get { return serviceType; } }

		/// <summary>
		/// Dohvaca upravitelja zgrade
		/// </summary>
		public virtual BuildingManager BuildingManager { get { return building.BuildingManager; } }

		/// <summary>
		/// Dohvaca predstavnika suvlasnika zgrade
		/// </summary>
		public virtual Person Representative { get { return building.RepresentativeOfPartOwners; } }

		/// <summary>
		/// Hitnost
		/// </summary>
		private Urgency urgency;

		/// <summary>
		/// Dohvaca hitnost
		/// </summary>
		public virtual Urgency Urgency { get { return urgency; } }

		/// <summary>
		/// Status popravka
		/// </summary>
		private StatusOfMaintenance statusOfMaintenance;

		/// <summary>
		/// Dohvaca status popravka
		/// </summary>
		public virtual StatusOfMaintenance StatusOfMaintenance { get { return statusOfMaintenance; } }

		/// <summary>
		/// Zgrada u kojoj se popravak obavlja
		/// </summary>
		private Building building;

		/// <summary>
		/// Dohvaca zgradu u kojoj se obavlja poparavak
		/// </summary>
		public virtual Building Building { get { return building; } }

		/// <summary>
		/// Zahtjev za popravkom
		/// </summary>
		private MaintenanceRequest maintenanceRequest;

		/// <summary>
		/// Dohvaca zahtjev za popravkom
		/// </summary>
		public virtual MaintenanceRequest MaintenanceRequest { get { return maintenanceRequest; } }

		/// <summary>
		/// Izvodac radova
		/// </summary>
		private PersonSnapshot contractor;

		/// <summary>
		/// Dohvaca izvodaca radova
		/// </summary>
		public virtual PersonSnapshot Contractor { get { return contractor; } }

		/// <summary>
		/// Dodatne instrukcije
		/// </summary>
		public virtual string Instructions { get; set; }

		/// <summary>
		/// Datum i vrijeme zavrsetka
		/// </summary>
		private DateTime? completitionDateTime;

		/// <summary>
		/// Dohvaca datum i vrijeme zavrsetka
		/// </summary>
		public virtual DateTime? CompletitionDateTime { get { return completitionDateTime; } }

		/// <summary>
		/// Zaključak izvođača radova na rješenje kvara
		/// </summary>
		public virtual string ContractorsConclusion { get; set; }

		/// <summary>
		/// Racun za popravak
		/// </summary>
		private Bill bill;

		/// <summary>
		/// Dohvaca racun za popravak
		/// </summary>
		public virtual Bill Bill { get { return bill; } }

		/// <summary>
		/// Napomene predstavnika suvlasnika
		/// </summary>
		private IList<MaintenanceRemark> maintenanceRemarks;

		/// <summary>
		/// Dohvaca napomene predstavnika suvlasnika
		/// </summary>
		public virtual IList<MaintenanceRemark> MaintenanceRemarks {
			get { return new ReadOnlyCollection<MaintenanceRemark>(maintenanceRemarks); }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected Maintenance() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="maintenanceRequest">zahtjev za popravkom</param>
		/// <param name="urgency">hitnost</param>
		/// <param name="serviceType">tip servisa/usluge</param>
		/// <param name="building">zgrada za koju se prijavljuje kvar</param>
		public Maintenance(MaintenanceRequest maintenanceRequest, Urgency urgency, RepairService serviceType, Building building) {
			this.maintenanceRequest = maintenanceRequest;
			this.building = building;
			this.urgency = urgency;
			this.serviceType = serviceType;
			statusOfMaintenance = StatusOfMaintenance.NotStarted;
			maintenanceRemarks = new List<MaintenanceRemark>();
		}

		#endregion

		#region Methods

		public virtual void SetBill(Bill bill) {
			var errors = new BusinessRulesException<Maintenance>();

			if(bill.From.Oib != contractor.Oib) {
				errors.AddErrorFor(m => m.Bill,
					"Constractor who issued bill isn't the same as contractor responsible for the maintenance.");
			}

			if(errors.Errors.Any()) {
				throw errors;
			}

			this.bill = bill;
		}

		/// <summary>
		/// Postavlja stanje za obavljeni posao na obavljeno
		/// </summary>
		public virtual void SetWorkAsDone() {
			statusOfMaintenance = StatusOfMaintenance.InConfirmation;
			completitionDateTime = DateTime.Now;
		}

		/// <summary>
		/// Postavlja stanje za obavljeni posao kao nije obavljeno
		/// </summary>
		/// <param name="remark">pojasnjenje zasto posao nije obavljen te danje instrukcije za poboljsanje posla</param>
		public virtual void SetWorkAsNotDone(string remark) {
			statusOfMaintenance = StatusOfMaintenance.InProgress;
			completitionDateTime = null;
			maintenanceRemarks.Add(new MaintenanceRemark(remark));
		}

		/// <summary>
		/// Zaključava održavanje/popravak kvara
		/// </summary>
		public virtual void SetMaitenanceCompleted() {
			statusOfMaintenance = StatusOfMaintenance.Completed;
		}

		/// <summary>
		/// Zaključava održavanje/popravak kvara
		/// </summary>
		/// <param name="remark">napomena predstavnika suvlasnika</param>
		public virtual void SetMaitenanceCompleted(string remark) {
			statusOfMaintenance = StatusOfMaintenance.Completed;
			maintenanceRemarks.Add(new MaintenanceRemark(remark));
		}

		/// <summary>
		/// Dodjeljuje izvodaca radova na popravak ili odrzavanje
		/// </summary>
		/// <param name="contractor">izvodac radova</param>
		public virtual void SetContractor(Contractor contractor) {
			var errors = new BusinessRulesException<Maintenance>();

			if (!BuildingManager.Contractors.Contains(contractor)) {
				errors.AddErrorForModel("Contractor is not working for BuildingManager, so it's invalid.");
			}

			if (!contractor.RepairServices.Contains(ServiceType)) {
				errors.AddErrorForModel("This Contractor can't do needed service.");
			}

			if (errors.Errors.Any()) {
				throw errors;
			}

			this.contractor = new PersonSnapshot(contractor.LegalPerson);
			statusOfMaintenance = StatusOfMaintenance.InProgress;
		}

		/// <summary>
		/// Mijenja vrstu usluge
		/// </summary>
		/// <param name="serviceType">nova vrsta usluge</param>
		public virtual void ChangeServiceType(RepairService serviceType) {
			this.serviceType = serviceType;
		}

		/// <summary>
		/// Postavlja hitnost
		/// </summary>
		/// <param name="urgency"></param>
		protected internal virtual void ChangeUrgency(Urgency urgency) {
			this.urgency = urgency;
		}

		#endregion

	}
}
