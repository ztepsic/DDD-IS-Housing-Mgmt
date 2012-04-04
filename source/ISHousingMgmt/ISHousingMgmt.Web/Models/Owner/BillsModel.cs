using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.Finances;

namespace ISHousingMgmt.Web.Models.Owner {
	public class BillsModel : RolesModel {
		public IList<BillModel> PaidBills { get; set; }
		public IList<BillModel> UnpaidBills { get; set; }
		public LinksModel Links { get; set; }
	}
}