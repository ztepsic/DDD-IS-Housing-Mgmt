using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Web.Models.BuildingManagement;

namespace ISHousingMgmt.Web.Models.BuildingMaintenance {
	public class CreateModel : RolesModel {

		public LinksModel Links { get; set; }

		[Display(Name = "Zgrada")]
		public string Building { get; set; }


		public SelectList Urgencies { get; set; }

		[Required]
		[Range(0, 2)]
		[Display(Name = "Hitnost")]
		public Urgency Urgency { get; set; }
		public string UrgencyName() {
			switch (Urgency) {
				case Urgency.Low:
					return "Niska";
				case Urgency.Normal:
					return "Normalna";
				case Urgency.High:
					return "Visoka";
				default:
					return string.Empty;
			}
		}

		public SelectList RepairServices { get; set; }
		[Required]
		[Display(Name = "Vrsta popravka")]
		public int RepairService { get; set; }

		[Required]
		public MaintenanceRequestModel MaintenanceRequest { get; set; }

	}
}