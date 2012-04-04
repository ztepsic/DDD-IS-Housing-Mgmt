using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.BuildingManagement;
using ISHousingMgmt.Web.Models.Legislature;

namespace ISHousingMgmt.Web.Models.Owner {
	public class ApartmentDetailModel {

		public PartitionSpaceDetailModel PartitionSpace { get; set; }
		public BuildingDetailModel Building { get; set; }

	}
}