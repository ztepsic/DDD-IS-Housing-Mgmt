using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISHousingMgmt.Web.Models.Account {
	public class RegisterUserModel {

		[Required]
		[Display(Name = "Korisničko ime")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Neispravna email adresa")]
		[Display(Name = "Email adresa")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "Lozinka {0} treba biti dugačka barem {2} znakova.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Lozinka")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Ponovi lozinku")]
		[Compare("Password", ErrorMessage = "Lozinka i ponovljena lozinka nisu jednake.")]
		public string ConfirmPassword { get; set; }

	}
}
