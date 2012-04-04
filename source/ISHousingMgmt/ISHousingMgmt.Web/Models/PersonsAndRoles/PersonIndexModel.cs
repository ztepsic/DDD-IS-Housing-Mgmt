using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.PersonsAndRoles {
	public class PersonIndexModel : RolesModel {

		public PhysicalPersonModel Person { get; set; }
		public LinksModel Links { get; set; }

	}
}