using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class LandRegistryDetailModel {

		public int Id { get; set; }

		public CadastralParticleDetailModel CadastralParticle { get; set; }
		public CadastreDetailModel Cadastre { get; set; }

		[Display(Name = "Ukupna porvršina vlasničkih etaža")]
		public decimal TotalSurfaceOfOwnedPartitions { get; set; }

		[Display(Name = "Ukupna površina zajedničkih etaža")]
		public decimal TotalSurfaceOfJointOwnershipPartitions { get; set; }

		[Display(Name = "Ukupna površina etaža")]
		public decimal TotalSurfaceOfPartitionSpaces { get; set; }

		public bool Locked { get; set; }

		public IList<PartitionSpaceListModel> OwnedPartitionSpaces { get; set; }
		public IList<PartitionSpaceListModel> JointOwnershipPartitionSpaces { get; set; }

	}
}