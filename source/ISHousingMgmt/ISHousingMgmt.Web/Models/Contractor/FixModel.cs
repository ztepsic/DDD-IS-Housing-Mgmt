using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.BuildingMaintenance;

namespace ISHousingMgmt.Web.Models.Contractor {
	public class FixModel : RolesModel {
		public MaintenanceDetailModel Maintenance { get; set; }
	}
}