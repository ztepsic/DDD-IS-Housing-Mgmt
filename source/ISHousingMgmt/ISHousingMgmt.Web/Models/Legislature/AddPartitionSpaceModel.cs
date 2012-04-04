﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class AddPartitionSpaceModel : RolesModel {
		public PartitionSpaceCreateModel PartitionSpace { get; set; }
		public LinksModel Links { get; set; }
	}
}