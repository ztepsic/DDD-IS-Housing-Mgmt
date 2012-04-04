using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class CreateVotingModel : RolesModel {

		public VotingCreateModel Voting { get; set; }
		public LinksModel Links { get; set; }
	}
}