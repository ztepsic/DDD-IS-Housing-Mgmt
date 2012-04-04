using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Finances {
	public class BillMethodModel : RolesModel {
		public Finances.BillModel Bill { get; set; }
		public LinksModel Links { get; set; }
	}
}