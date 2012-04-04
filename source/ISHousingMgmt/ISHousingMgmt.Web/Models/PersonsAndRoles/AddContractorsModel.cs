using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.PersonsAndRoles {
	public class AddContractorsModel : RolesModel {

		public ICollection<ContractorModel> Contractors { get; set; }
		[Required]
		public int[] SelectedContractors { get; set; }

	}
}