using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class VotingsModel : RolesModel {

		#region Members

		public bool IsRepresentative { get; set; }
		public int BuidlingId { get; set; }
		public string Building { get; set; }
		public IList<AdminJobsVotingListModel> Votings { get; set; }
		public LinksModel Links { get; set; }

		#endregion

		#region Constructors and Init

		public VotingsModel() {
			IsRepresentative = false;
		}

		#endregion

	}
}