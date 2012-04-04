using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Finances {
	public class ReservePeriodsModel : RolesModel {
		public LinksModel Links { get; set; }
		public IList<DateTime> Periods { get; set; }
		public ReserveModel Reserve { get; set; }
	}
}