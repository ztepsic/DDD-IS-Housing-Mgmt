using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.BuildingManagement;

namespace ISHousingMgmt.Web.Models.Owner {
	public class ApartmentModel : RolesModel {

		public ApartmentDetailModel Apartment { get; set; }
		public IList<AdminJobsVotingListModel> Votings { get; set; }
		public LinksModel Links { get; set; }

	}
}