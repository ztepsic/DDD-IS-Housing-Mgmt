using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ISHousingMgmt.Web.Models {
	public class AddressModel {

		[Required]
		[Display(Name = "Grad")]
		public CityModel City { get; set; }
		public SelectList Cities { get; set; }

		[Required]
		[RegularExpression("^[a-zšđčć žA-ZŠĐČĆŽ]{2,}$", ErrorMessage = "Unjeli ste neispravno ime ulice.")]
		[Display(Name = "Ulica")]
		public string StreetAddress { get; set; }

		[Required]
		[Display(Name = "Broj")]
		public string StreetAddressNumber { get; set; }

		public override string ToString() {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(StreetAddress);
			stringBuilder.Append(" ");
			stringBuilder.Append(StreetAddressNumber);
			stringBuilder.Append(", ");
			stringBuilder.Append(City.PostalCode);
			stringBuilder.Append(" ");
			stringBuilder.Append(City.Name);

			return stringBuilder.ToString();
		}


	}
}