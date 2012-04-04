using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.Finances {
	public class BillModel {

		#region Members

		public int Id { get; set; }

		[Display(Name = "Pričuva")]
		public ReserveModel Reserve { get; set; }

		[Display(Name = "Naziv zgrade")]
		public string BuildingName { get; set; }

		[Display(Name = "Datum izdavanja računa")]
		public DateTime DateTimeIssued { get; set; }

		[Display(Name = "Datum plaćanja računa")]
		public DateTime? PaidDateTime { get; set; }

		[Display(Name = "PDV")]
		public short Tax { get; set; }

		[Display(Name = "Izdao")]
		public PersonModel From { get; set; }

		[Display(Name = "Za")]
		public PersonModel To { get; set; }

		[Display(Name = "Poziv na broj")]
		public string ReferenceNumber { get; set; }

		[Display(Name = "Stavke računa")]
		public IEnumerable<BillItemModel> BillItems { get; set; }

		[Display(Name = "Osnovica za PDV")]
		public decimal TotalAmount { get; set; }

		[Display(Name = "Platiti PDV")]
		public decimal TaxAmount { get; set; }

		[Display(Name = "Ukupno")]
		public decimal TotalAmountWithTax { get; set; }

		[Display(Name = "Plaćeno")]
		public bool IsPaid { get; set; }

		[Required]
		[Display(Name = "Opis plaćanja")]
		public string PaymentDescription { get; set; }

		#endregion

		#region Constructors and Init

		#endregion



	}
}