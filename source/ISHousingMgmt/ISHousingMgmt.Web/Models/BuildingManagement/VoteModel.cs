using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class VoteModel : RolesModel {

		public VotingVoteModel Vote { get; set; }
		public LinksModel Links { get; set; }
	}
}