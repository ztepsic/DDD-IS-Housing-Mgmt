using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.BuildingMaintenance {
	public class DelegateRepairModel : RolesModel {

		public LinksModel Links { get; set; }

		public IList<ContractorModel> Contractors { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int Contractor { get; set; }

		public MaintenanceDetailModel Maintenance { get; set; }
	}
}