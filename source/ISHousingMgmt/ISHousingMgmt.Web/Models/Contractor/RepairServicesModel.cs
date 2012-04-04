using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Contractor {
	public class RepairServicesModel : RolesModel {
		public PersonsAndRoles.ContractorModel Contractor { get; set; }
		public IEnumerable<BuildingMaintenance.RepairServiceModel> AvaliableServices { get; set; }
	}
}