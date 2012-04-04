using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class CreateModel : RolesModel {

		public CreateLandRegistryModel LandRegistry { get; set; }
		public LinksModel Links { get; set; }
	}
}