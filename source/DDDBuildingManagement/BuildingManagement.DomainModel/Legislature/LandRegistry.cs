using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.Legislature {
	/// <summary>
	/// Razred koji predstavlja zemljisnu knjigu
	/// </summary>
	public class LandRegistry : EntityBase {

		#region Members

		/// <summary>
		/// Katastarska cestica
		/// </summary>
		private readonly AbstractCadastralParticle cadastralParticle;

		/// <summary>
		/// Dohvaca katastarske cestice
		/// </summary>
		public AbstractCadastralParticle CadastralParticle {
			get { return cadastralParticle; }
		}

		/// <summary>
		/// Katastar
		/// </summary>
		public Cadastre Cadastre {
			get { return cadastralParticle.Cadastre; }
		}

		/// <summary>
		/// Etazirani prostori
		/// </summary>
		private readonly IList<PartitionSpace> partitionSpaces;

		/// <summary>
		/// Dohvaca sve etazirane prostore
		/// </summary>
		public IList<PartitionSpace> PartitionSpaces {
			get { return new ReadOnlyCollection<PartitionSpace>(partitionSpaces); }
		}

		/// <summary>
		/// Dohvaca zajednicke etazirane prostore
		/// </summary>
		public IList<PartitionSpace> JointOwnershipPartitionSpaces {
			get { return new ReadOnlyCollection<PartitionSpace>(partitionSpaces.Where(x => !x.IsOwnedPartitionSpace).ToList()); }
		}

		/// <summary>
		/// Dohvaca vlasnicke etazirane prostore
		/// </summary>
		public IList<PartitionSpace> OwnedPartitionSpaces {
			get { return new ReadOnlyCollection<PartitionSpace>(partitionSpaces.Where(x => x.IsOwnedPartitionSpace).ToList()); }
		}


		/// <summary>
		/// Dohvaca totalnu povrsinu svih vlasnickih etaziranih prostora
		/// </summary>
		public decimal TotalSurfaceOfOwnedPartitions {
			get { return partitionSpaces.Where(x => x.IsOwnedPartitionSpace).Sum(x => x.SurfaceArea); }
		}


		/// <summary>
		/// Dohvaca ukupnu povrsinu svih zajednickih etaziranih prostora.
		/// </summary>
		public decimal TotalSurfaceOfJointOwnershipPartitios {
			get { return partitionSpaces.Where(x => !x.IsOwnedPartitionSpace).Sum(x => x.SurfaceArea); }
		}

		/// <summary>
		/// Dohvaca ukupnu povrsinu svih etaziranih prostora.
		/// </summary>
		public decimal TotalSurfaceOfPartitionSpaces {
			get { return partitionSpaces.Sum(x => x.SurfaceArea); }
		}

		

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konsturktor
		/// </summary>
		/// <param name="cadastralParticle">katastarska cestica</param>
		public LandRegistry(AbstractCadastralParticle cadastralParticle) {
			this.cadastralParticle = cadastralParticle;
			partitionSpaces = new List<PartitionSpace>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Stvara i dodaje etazu zajednickog vlasnistva u zamljisnu knjigu
		/// </summary>
		/// <param name="surfaceArea">povrsina etaze</param>
		/// <param name="description">opis etaze</param>
		/// <returns>novostvorena etaza zajednickog vlasnistva</returns>
		public IPartitionSpace CreatePartitionSpace(decimal surfaceArea, string description) {
			return CreatePartitionSpace(surfaceArea, description, null, 0);
		}

		/// <summary>
		/// Stvara i dodaje etazu zajednickog vlasnistva u zamljisnu knjigu
		/// </summary>
		/// <param name="surfaceArea">povrsina etaze</param>
		/// <param name="description">opis etaze</param>
		/// <param name="owner">vlasnici</param>
		/// <param name="shareOfTotalOwnershihp">Udio ukupnog vlasnistva temeljem ove etaze</param>
		/// <returns>novostvorena etaza zajednickog vlasnistva</returns>
		public IPartitionSpace CreatePartitionSpace(decimal surfaceArea, string description, Person owner, decimal shareOfTotalOwnershihp) {
			var ordinalNumber = partitionSpaces.Any() ? partitionSpaces.Last().OrdinalNumber + 1 : 1;
			PartitionSpace partitionSpace = new PartitionSpace(CadastralParticle, ordinalNumber, surfaceArea, description) {
				Owner = owner,
				ShareOfTotalOwnershihp = shareOfTotalOwnershihp
			};
			partitionSpaces.Add(partitionSpace);

			return partitionSpace;
		}


		#endregion

	}
}
