using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class VotingVoteModel {

		[Display(Name = "Tema rada uprave")]
		public string Subject { get; set; }

		[Display(Name = "Opis rada uprave")]
		public string Description { get; set; }

		[Display(Name = "Glas")]
		public bool Vote { get; set; }

	}
}