using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.Account {
	public class EditModel : RolesModel {

		[Required]
		[Display(Name = "Ime")]
		public string Name { get; set; }

		[Display(Name = "Prezime")]
		public string Surname { get; set; }

		[Required]
		[Display(Name = "Adresa")]
		public AddressModel Address { get; set; }

		[Display(Name = "Korisničko ime")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email adresa")]
		public string Email { get; set; }

		[StringLength(100, ErrorMessage = "Lozinka {0} treba biti dugačka barem {2} znakova.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Stara Lozinka")]
		public string OldPassword { get; set; }

		[StringLength(100, ErrorMessage = "Lozinka {0} treba biti dugačka barem {2} znakova.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Lozinka")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Ponovi lozinku")]
		[Compare("Password", ErrorMessage = "Lozinka i ponovljena lozinka nisu jednake.")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Telefon")]
		public string TelephoneNumber { get; set; }

		[Display(Name = "Mobitel")]
		public string MobileNumber { get; set; }

	}
}