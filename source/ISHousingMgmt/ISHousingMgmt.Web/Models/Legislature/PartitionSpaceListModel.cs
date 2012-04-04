using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class PartitionSpaceListModel {

		public int Id { get; set; }
		public int OrdinalNumber { get; set; }
		public decimal SurfaceArea { get; set; }
		public decimal ShareOfTotalOwnership { get; set; }
		public string Description { get; set; }
		public bool IsOwnedPartitionSpace { get; set; }
		public PersonModel Owner { get; set; }

	}
}