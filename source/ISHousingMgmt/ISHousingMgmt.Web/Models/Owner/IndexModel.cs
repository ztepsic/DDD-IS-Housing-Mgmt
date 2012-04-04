using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Owner {
	public class IndexModel : RolesModel {

		public IList<ApartmentListModel> Apartments { get; set; }

	}
}