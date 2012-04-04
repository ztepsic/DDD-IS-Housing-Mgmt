using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Account {
	public class LogInModel {

		[Required]
		[Display(Name = "Korisničko ime")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Lozinka")]
		public string Password { get; set; }

		[Display(Name = "Zapamti me?")]
		public bool RememberMe { get; set; }


	}
}