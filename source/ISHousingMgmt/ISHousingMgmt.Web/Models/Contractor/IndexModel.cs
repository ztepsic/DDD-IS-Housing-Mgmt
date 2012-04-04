using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Contractor {
	public class IndexModel : RolesModel {
		public IEnumerable<BuildingMaintenance.MaintenanceDetailModel> ActiveMaintenances { get; set; }
		public IEnumerable<BuildingMaintenance.MaintenanceDetailModel> CompletedMaintenances { get; set; }
		public IEnumerable<BuildingMaintenance.MaintenanceDetailModel> InConfirmationMaintenances { get; set; }
	}
}