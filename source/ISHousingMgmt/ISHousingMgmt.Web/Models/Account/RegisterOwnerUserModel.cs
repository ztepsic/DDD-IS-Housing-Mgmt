using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISHousingMgmt.Web.Models.Account {
	public class RegisterOwnerUserModel : RegisterUserModel {

		[Required]
		[RegularExpression("^[a-zšđčćžA-ZŠĐČĆŽ]{2,}$", ErrorMessage = "Unjeli ste neispravno ime.")]
		[Display(Name = "Ime")]
		public string Name { get; set; }

		[Display(Name = "Prezime")]
		[RegularExpression("^[a-zšđčćžA-ZŠĐČĆŽ]{2,}$", ErrorMessage = "Unjeli ste neispravno prezime.")]
		public string Surname { get; set; }

		[Required]
		[StringLength(11, ErrorMessage = "OIB treba biti dugačak točno {2} znamenki.", MinimumLength = 11)]
		[Display(Name = "OIB")]
		public string Oib { get; set; }

		[Display(Name = "Broj bankovnog računa")]
		public string NumberOfBankAccount { get; set; }

		[Required]
		[Display(Name = "Adresa")]
		public AddressModel Address { get; set; }

		[RegularExpression("^[0-9-]{6,}$", ErrorMessage = "Unjeli ste neispravan broj telefona.")]
		[Display(Name = "Telefon")]
		public string TelephoneNumber { get; set; }

		[RegularExpression("^[0-9-]{6,}$", ErrorMessage = "Unjeli ste neispravan broj mobitela.")]
		[Display(Name = "Mobitel")]
		public string MobileNumber { get; set; }

	}
}