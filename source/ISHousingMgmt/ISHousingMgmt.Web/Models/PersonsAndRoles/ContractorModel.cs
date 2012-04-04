using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.BuildingMaintenance;

namespace ISHousingMgmt.Web.Models.PersonsAndRoles {
	public class ContractorModel {

		public int Id { get; set; }
		public PersonModel LegalPerson { get; set; }
		public IList<RepairServiceModel> RepairServices { get; set; }

	}
}