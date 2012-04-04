using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Finances {
	public class ReserveBillsModel : RolesModel {
		public ReserveModel Reserve { get; set; }
		public LinksModel Links { get; set; }
		public IList<BillModel> PaidBills { get; set; }

		public IList<BillModel> UnpaidBills { get; set; }

		public DateTime Date { get; set; }
	}
}