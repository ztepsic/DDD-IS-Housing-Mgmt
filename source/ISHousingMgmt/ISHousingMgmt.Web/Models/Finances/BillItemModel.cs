using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Finances {
	public class BillItemModel {

		[Required]
		[Display(Name = "Količina")]
		public int Quantity { get; set; }

		[Required]
		[Display(Name = "Cijena")]
		public decimal Price { get; set; }

		[Required]
		[Display(Name = "Opis")]
		public string Description { get; set; }

		[Display(Name = "Ukupna cijena")]
		public decimal TotalAmount { get; set; }

	}
}