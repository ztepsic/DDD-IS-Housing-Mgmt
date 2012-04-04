using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class VotingModel : RolesModel {

		public AdminJobsVotingDetailModel Voting { get; set; }
		public bool IsUserVoted { get; set; }
		public LinksModel Links { get; set; }

	}
}