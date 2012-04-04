using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ISHousingMgmt.Domain.BuildingManagement;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class AdminJobsVotingDetailModel {

		public int Id { get; set; }

		[Display(Name = "Vrsta uprave")]
		public AdministrationJobsType AdministrationJobsType { get; set; }
		public string GetAdministrationJobsTypeName() {
			switch (AdministrationJobsType) {
				case AdministrationJobsType.Regular:
					return "Redovna";
				case AdministrationJobsType.Extraordinary:
					return "Izvanredna";
				default:
					return string.Empty;
			}
		}

		[Display(Name = "Tema rada uprave")]
		public string Subject { get; set; }

		[Display(Name = "Opis rada uprave")]
		public string Description { get; set; }

		[Display(Name = "Početak")]
		public DateTime StartDateTime { get; set; }

		[Display(Name = "Kraj")]
		public DateTime EndDateTime { get; set; }

		[Display(Name = "Broj suvlasnika")]
		public int NumberOfOwners { get; set; }

		[Display(Name = "Završeno")]
		public bool IsFinished { get; set; }

		[Display(Name = "Prihvaćeno")]
		public bool IsAccepted { get; set; }

		[Display(Name = "Broj glasova")]
		public int OwnerVotesCount { get; set; }

		[Display(Name = "Broj pozitivnih glasnova")]
		public int NumberOfPositiveVotes { get; set; }

		[Display(Name = "Broj negativnih glasova")]
		public int NumberOfNegativeVotes { get; set; }

		public BuildingListModel Building { get; set; }
	}
}