using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingMaintenance {
	public class ConfirmModel : RolesModel {

		public MaintenanceDetailModel Maintenance { get; set; }
		public LinksModel Links { get; set; }

		[Display(Name = "Napomena predstavnika suvlasnika")]
		public string Remark { get; set; }

		[Display(Name = "Potvrdi da je posao obavljen")]
		public bool IsConfirmed { get; set; }
	}
}