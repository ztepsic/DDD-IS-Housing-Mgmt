using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class VotingCreateModel {

		[Required]
		[Display(Name = "Vrsta uprave")]
		[Range(0,1)]
		public int AdministrationJobsType { get; set; }

		public SelectList AdministrationJobsTypes {
			get {
				return new SelectList(
					new[] {
					new {Value = (int)Domain.BuildingManagement.AdministrationJobsType.Regular, Text = "Redovna"},
					new {Value = (int)Domain.BuildingManagement.AdministrationJobsType.Extraordinary, Text = "Izvanredna"}
				}, "Value", "Text");
			}
		}

		[Required]
		[Display(Name = "Tema rada uprave")]
		public string Subject { get; set; }

		[Required]
		[Display(Name = "Opis rada uprave")]
		public string Description { get; set; }

		[Required]
		[Display(Name = "Rok")]
		public DateTime EndDateTime { get; set; }

	}
}