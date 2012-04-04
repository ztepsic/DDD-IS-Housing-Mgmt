using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Contractor {
	public class BillModel : RolesModel {
		public Finances.BillModel Bill { get; set; }
	}
}