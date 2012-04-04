using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISHousingMgmt.Web.Models.BuildingMaintenance;
using ISHousingMgmt.Web.Models.Finances;

namespace ISHousingMgmt.Web.Models.Contractor {
	public class IssueBillModel : RolesModel {

		#region Members

		public SelectList UnbilledMaintances { get; set; }

		[Required]
		[Display(Name = "Popravak/održavanje za koji se izdaje račun")]
		public int UnbilledMaintance { get; set; }

		[Required]
		[Display(Name = "Stavke računa")]
		public BillItemModel[] BillItems { get; set; }

		[Required]
		[Display(Name = "Opis plaćanja")]
		public string PaymentDescription { get; set; }

		#endregion

		#region Constructors and Init

		#endregion

	}
}