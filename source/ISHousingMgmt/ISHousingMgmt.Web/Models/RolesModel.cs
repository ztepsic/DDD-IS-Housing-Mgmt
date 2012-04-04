using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models {
	public class RolesModel {

		#region Members

		public string CurrentRole { get; set; }
		public string[] Roles { get; set; }

		#endregion

		#region Constructors and Init

		public RolesModel() {
			CurrentRole = string.Empty;
		}

		#endregion

		#region Methods

		public bool IsInRole(string roleName) {
			return Roles.Any(role => role.ToLower() == roleName.ToLower());
		}

		#endregion
	}
}