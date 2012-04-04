using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.BuildingManagement {
	public class ChangeReservCoefModel : RolesModel {

		[Required]
		[Range(1.53, 100)]
		[Display(Name = "Koeficijent pričuve")]
		public decimal ReserveCoefficient { get; set; }
		public LinksModel Links { get; set; }
	}
}