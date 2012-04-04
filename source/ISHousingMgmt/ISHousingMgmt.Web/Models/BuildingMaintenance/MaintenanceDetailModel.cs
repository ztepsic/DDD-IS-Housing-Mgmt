using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Web.Models.BuildingManagement;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.BuildingMaintenance {
	public class MaintenanceDetailModel {

		public int Id { get; set; }

		[Display(Name = "Vrsta kvara")]
		public RepairServiceModel ServiceType { get; set; }

		[Display(Name = "Upravitelj")]
		public PersonModel BuildingManager { get; set; }

		[Display(Name = "Predstavnik")]
		public PersonModel Representative { get; set; }

		[Display(Name = "Hitnost")]
		public Urgency Urgency { get; set; }
		public string UrgencyName() {
			switch (Urgency) {
				case Domain.BuildingMaintenance.Urgency.Low:
					return "Niska";
				case Domain.BuildingMaintenance.Urgency.Normal:
					return "Normalna";
				case Domain.BuildingMaintenance.Urgency.High:
					return "Visoka";
				default:
					return string.Empty;
			}
		}

		[Display(Name = "Status")]
		public StatusOfMaintenance StatusOfMaintenance { get; set; }
		public string GetStatusOfMaintenanceName() {
			switch (StatusOfMaintenance) {
				case Domain.BuildingMaintenance.StatusOfMaintenance.NotStarted:
					return "Nije započeto";
				case Domain.BuildingMaintenance.StatusOfMaintenance.InProgress:
					return "Započeto";
				case Domain.BuildingMaintenance.StatusOfMaintenance.InConfirmation:
					return "Čeka na potvrdu";
				case Domain.BuildingMaintenance.StatusOfMaintenance.Completed:
					return "Završeno";
				default:
					return string.Empty;
			}
		}

		public BuildingListModel Building { get; set; }

		public virtual MaintenanceRequestModel MaintenanceRequest { get; set; }

		[Display(Name = "Izvođač radova")]
		public PersonModel Contractor { get; set; }

		[Display(Name = "Upraviteljeve instrukcije izvođaču radova")]
		public string Instructions { get; set; }

		[Display(Name = "Datum završetka")]
		public DateTime? CompletitionDateTime { get; set; }

		[Display(Name = "Zaključak izvođača radova")]
		public string ContractorsConclusion { get; set; }

		[Display(Name = "Račun")]
		public int BillId { get; set; }

		[Display(Name = "Napomene predstavnika suvlasnika")]
		public IEnumerable<MaintenanceRemarkModel> MaintenanceRemarks { get; set; }

		public class MaintenanceRemarkModel {
			public string Remark { get; set; }
			public DateTime RemarkDateTime { get; set; }
		}

	}
}