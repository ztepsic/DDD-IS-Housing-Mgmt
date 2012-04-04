using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.BuildingManagement;

namespace ISHousingMgmt.Web.Models.Finances {
	public class ReserveModel {

		[Display(Name = "Zgrada")]
		public BuildingListModel Building { get; set; }

		[Display(Name = "Stanje novca")]
		public decimal Money { get; set; }

		[Display(Name = "Plaćeni računi zgrade")]
		public IList<BillModel> PaidBills { get; set; }

		[Display(Name = "Neplaćeni računi zgrade")]
		public virtual IList<BillModel> UnpaidBills { get; set; }

	}
}