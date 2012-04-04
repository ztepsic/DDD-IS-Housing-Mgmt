using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.BuildingManagement;

namespace ISHousingMgmt.Web.Models.BuildingMaintenance {
	public class IndexModel : RolesModel {

		#region Members

		public BuildingListModel Building { get; set; }
		public IEnumerable<MaintenanceDetailModel> NewMaintenances { get; set; }
		public IEnumerable<MaintenanceDetailModel> ActiveMaintenances { get; set; }
		public IEnumerable<MaintenanceDetailModel> InConfirmationMaintenances { get; set; }
		public IEnumerable<MaintenanceDetailModel> CompletedMaintenances { get; set; }
		public LinksModel Links { get; set; }

		#endregion

		#region Constructors and Init

		#endregion
	}
}