using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class CreateLandRegistryModel {

		[Display(Name = "Grad katastarske općine")]
		public int City { get; set; }

		public SelectList Cities { get; set; }

		[Required]
		[Display(Name = "Katastar")]
		public int Cadastre { get; set; }

		[Required]
		[RegularExpression("^[0-9/]+$", ErrorMessage = "Neispravan broj katastarske čestice")]
		[Display(Name = "Broj katastarske čestice")]
		public string NumberOfCadastralParticle { get; set; }


		[Required]
		[Display(Name = "Površina")]
		public decimal SurfaceArea { get; set; }

		[Required]
		[Display(Name = "Opis")]
		public string Description { get; set; }



	}
}