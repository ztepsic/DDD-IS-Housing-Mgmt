using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Finances {
	public class ReserveMModel : RolesModel {
		public ReserveModel Reserve { get; set; }
		public LinksModel Links { get; set; }
	}
}