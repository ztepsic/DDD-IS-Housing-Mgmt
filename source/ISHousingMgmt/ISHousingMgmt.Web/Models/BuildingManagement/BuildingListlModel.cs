using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class BuildingListModel {

		public int Id { get; set; }
		public AddressModel Address { get; set; }
		public decimal SurfaceArea { get; set; }
		public string Description { get; set; }

	}
}