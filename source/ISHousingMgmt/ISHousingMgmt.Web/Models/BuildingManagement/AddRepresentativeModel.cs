using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class AddRepresentativeModel : RolesModel {

		public IEnumerable<PersonModel> Owners { get; set; }
		public int BuildingId { get; set; }
		public string Building { get; set; }
		public LinksModel Links { get; set; }
	}
}