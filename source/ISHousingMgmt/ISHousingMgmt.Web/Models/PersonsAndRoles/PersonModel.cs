using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Iesi.Collections.Generic;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Domain;

namespace ISHousingMgmt.Web.Models.PersonsAndRoles {
	public class PersonModel {

		public int Id { get; set; }

		[Required]
		[Display(Name = "Ime")]
		public string Name { get; set; }

		[Display(Name = "Puno ime")]
		public string FullName { get; set; }

		[Required]
		[StringLength(11, ErrorMessage = "OIB treba biti dugačak točno {2} znamenki.", MinimumLength = 11)]
		[Display(Name = "OIB")]
		public string Oib { get; set; }

		[Required]
		[Display(Name = "Adresa")]
		public AddressModel Address { get; set; }

		[Display(Name = "Telefoni")]
		public IList<Telephone> Telephones { get; set; }

	}
}