using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class IndexModel : RolesModel {

		public IList<BuildingListModel> Buildings { get; set; }

	}
}