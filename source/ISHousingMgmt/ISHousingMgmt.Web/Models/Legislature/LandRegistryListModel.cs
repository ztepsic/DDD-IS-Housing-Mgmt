using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISHousingMgmt.Domain.Legislature;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class LandRegistryListModel {

		public int Id { get; set; }
		public string CadastralParticleNumberOfCadastralParticle { get; set; }
		public string CadastralParticleDescription { get; set; }
		public decimal CadastralParticleSurfaceArea { get; set; }
		public int NumberOfPartitionSpaces { get; set; }
		public bool Locked { get; set; }

	}
}