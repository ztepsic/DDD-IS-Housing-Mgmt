using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace ISHousingMgmt.Web.Models {
	public class NavLink {

		#region Members

		public string Text { get; set; }
		public RouteValueDictionary RouteValues { get; set; }
		public bool IsSelected { get; set; }

		#endregion

		#region Constructors and Init

		public NavLink() {
			IsSelected = false;
		}

		#endregion

		#region Methods
		#endregion

	}
}