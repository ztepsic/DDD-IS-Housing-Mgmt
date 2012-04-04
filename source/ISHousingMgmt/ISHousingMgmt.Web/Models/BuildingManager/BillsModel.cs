using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingManager {
	public class BillsModel : RolesModel {

		public int BuildingId { get; set; }
		public IEnumerable<Finances.BillModel> UnpaidBills { get; set; }
		public IEnumerable<Finances.BillModel> PaidBills { get; set; }
		public LinksModel Links { get; set; }

	}
}