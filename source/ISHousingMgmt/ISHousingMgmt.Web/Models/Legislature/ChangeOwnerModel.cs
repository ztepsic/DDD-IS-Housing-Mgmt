using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class ChangeOwnerModel : RolesModel {
		public PartitionSpaceDetailModel PartitionSpace { get; set; }
		public LinksModel Links { get; set; }

		[Required]
		public PhysicalPersonModel NewOwner { get; set; }
	}
}