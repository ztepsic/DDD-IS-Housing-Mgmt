using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class PartitionSpaceCreateModel {

		public CadastralParticleDetailModel CadastralParticle { get; set; }

		[Required]
		[RegularExpression("^[0-9/]+$", ErrorMessage = "Neispravan broj uloška.")]
		[Display(Name = "Broj uloška")]
		public string RegistryNumber { get; set; }

		[Required]
		[Display(Name = "Broj poduloška")]
		public int OrdinalNumber { get; set; }

		[Required]
		[Display(Name = "Površina")]
		public decimal SurfaceArea { get; set; }

		[Required]
		[Display(Name = "Opis")]
		public string Description { get; set; }

		public PhysicalPersonModel Owner { get; set; }

	}
}