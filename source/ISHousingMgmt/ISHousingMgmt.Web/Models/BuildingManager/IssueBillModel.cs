using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISHousingMgmt.Web.Models.BuildingManagement;
using ISHousingMgmt.Web.Models.Finances;

namespace ISHousingMgmt.Web.Models.BuildingManager {
	public class IssueBillModel : RolesModel {

		public LinksModel Links { get; set; }

		[Display(Name = "Zgrada za koju se izdaje račun")]
		public BuildingListModel Building { get; set; }

		[Required]
		[Display(Name = "Stavke računa")]
		public BillItemModel[] BillItems { get; set; }

		[Required]
		[Display(Name = "Opis plaćanja")]
		public string PaymentDescription { get; set; }

	}
}