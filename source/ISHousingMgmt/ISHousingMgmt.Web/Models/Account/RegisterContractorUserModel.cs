using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.Account {
	public class RegisterContractorUserModel : RegisterUserModel {

		[Required]
		[RegularExpression("^[a-zšđčćž A-ZŠĐČĆŽ\\.]{2,}$", ErrorMessage = "Unjeli ste neispravno ime.")]
		[Display(Name = "Ime pravne osobe")]
		public string Name { get; set; }

		[Required]
		[StringLength(11, ErrorMessage = "OIB treba biti dugačak točno {2} znamenki.", MinimumLength = 11)]
		[Display(Name = "OIB")]
		public string Oib { get; set; }

		[Required]
		[Display(Name = "Broj bankovnog računa")]
		public string NumberOfBankAccount { get; set; }

		[Required]
		[Display(Name = "Ulica")]
		public string StreetAddress { get; set; }

		[Required]
		[Display(Name = "Broj")]
		public string StreetAddressNumber { get; set; }

		[Required]
		[Display(Name = "Grad")]
		public int City { get; set; }

		public SelectList Cities { get; set; }

		[RegularExpression("^[0-9-]{6,}$", ErrorMessage = "Unjeli ste neispravan broj telefona.")]
		[Display(Name = "Telefon")]
		public string TelephoneNumber { get; set; }

		[RegularExpression("^[0-9-]{6,}$", ErrorMessage = "Unjeli ste neispravan broj mobitela.")]
		[Display(Name = "Mobitel")]
		public string MobileNumber { get; set; }

		[Required]
		[Display(Name = "Usluge")]
		public int[] SelectedRepairServices { get; set; }

		public SelectList RepairServices { get; set; }

	}
}