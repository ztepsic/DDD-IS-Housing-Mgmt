using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.BuildingMaintenance;

namespace ISHousingMgmt.Web.Models.Dashboard {
	public class IndexModel : RolesModel {
		public IList<MaintenanceDetailModel> Maintenances { get; set; }

		public List<Owner.ApartmentListModel> Apartments { get; set; }

		public IList<BuildingManagement.BuildingListModel> RepresentativeBuildings { get; set; }

		public IList<BuildingManagement.BuildingListModel> ManagerBuildings { get; set; }
	}
}