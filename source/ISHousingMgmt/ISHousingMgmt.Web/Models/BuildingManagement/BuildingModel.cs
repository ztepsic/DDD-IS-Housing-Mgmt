using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class BuildingModel : RolesModel {

		public BuildingDetailModel Building { get; set; }
		public IList<AdminJobsVotingListModel> Votings { get; set; }
		public LinksModel Links { get; set; }

	}
}