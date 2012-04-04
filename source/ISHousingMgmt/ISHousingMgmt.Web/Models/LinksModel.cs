using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models {
	public class LinksModel {

		public int Id { get; set; }
		public IList<NavLink> Links { get; set; }

	}
}