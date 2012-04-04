using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class PartitionSpaceDetailModel {

		public int Id { get; set; }

		public int LandRegistryId { get; set; }

		[Display(Name = "Broj uloška")]
		public string RegistryNumber { get; set; }

		public CadastralParticleDetailModel CadastralParticle { get; set; }

		[Display(Name = "Broj poduloška")]
		public int OrdinalNumber { get; set; }

		[Display(Name = "Površina")]
		public decimal SurfaceArea { get; set; }

		[Display(Name = "Opis")]
		public string Description { get; set; }

		[Display(Name = "Vlasnik")]
		public PersonModel Owner { get; set; }

		[Display(Name = "Udio vlasništva")]
		public decimal ShareOfTotalOwnership { get; set; }

		public bool IsOwnedPartitionSpace { get; set; }

	}
}