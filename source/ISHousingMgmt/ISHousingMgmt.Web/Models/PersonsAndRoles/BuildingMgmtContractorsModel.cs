using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.PersonsAndRoles {
	public class BuildingMgmtContractorsModel : RolesModel {

		public ICollection<ContractorModel> Contractors { get; set; }

	}
}