using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.Finances;

namespace ISHousingMgmt.Web.Models.Contractor {
	public class BillsModel : RolesModel {

		public IEnumerable<Finances.BillModel> UnpaidBills { get; set; }
		public IEnumerable<Finances.BillModel> PaidBills { get; set; }

	}
}