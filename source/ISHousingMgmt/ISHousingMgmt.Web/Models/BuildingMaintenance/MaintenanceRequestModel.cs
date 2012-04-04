using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.BuildingMaintenance {
	public class MaintenanceRequestModel {

		[Display(Name = "Prijavio")]
		public PersonModel Submitter { get; set; }
		
		[Required]
		[Display(Name = "Kvar")]
		public string Subject { get; set; }

		[Required]
		[Display(Name = "Opis kvara")]
		public string Description { get; set; }

		[Required]
		[Display(Name = "Lokacija")]
		public string Location { get; set; }

		[Display(Name = "Datum prijave")]
		public DateTime DateTimeOfRequest { get; set; }

	}
}