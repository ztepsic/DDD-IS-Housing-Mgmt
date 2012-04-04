using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Domain.BuildingManagement;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class AdminJobsVotingListModel {

		public int Id { get; set; }
		public AdministrationJobsType AdministrationJobsType { get; set; }
		public string GetAdministrationJobsTypeName() {
			switch (AdministrationJobsType) {
				case AdministrationJobsType.Regular:
					return "Regularna";
				case AdministrationJobsType.Extraordinary:
					return "Izvanredna";
				default:
					return string.Empty;
			}
		}

		public string Subject { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public int NumberOfOwners { get; set; }
		public bool IsFinished { get; set; }
		public bool IsAccepted { get; set; }
		public int Voted { get; set; }

	}
}