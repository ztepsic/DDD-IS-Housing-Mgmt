using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingMaintenance {
	public class DetailsModel : RolesModel {

		public MaintenanceDetailModel Maintenance { get; set; }
		public LinksModel Links { get; set; }


	}
}