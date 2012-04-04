using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Iesi.Collections.Generic;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Web.Models.Legislature;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class BuildingDetailModel {

		public int Id { get; set; }
		public AddressModel Address { get; set; }

		[Display(Name = "Upravitelj zgrade")]
		public PersonModel BuildingManagerLegalPerson { get; set; }

		[Display(Name = "Predstavnik suvlasnika")]
		public PersonModel RepresentativeOfPartOwners { get; set; }

		[Display(Name = "Zemljišna knjiga")]
		public LandRegistryDetailModel LandRegistry { get; set; }

		[Display(Name = "Koeficijent pričuve")]
		public decimal ReserveCoefficient { get; set; }

		[Display(Name = "Pričuva")]
		public decimal ReserveMoney { get; set; }

	}
}