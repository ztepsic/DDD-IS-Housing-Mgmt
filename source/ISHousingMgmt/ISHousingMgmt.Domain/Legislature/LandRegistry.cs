using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.Legislature {
	/// <summary>
	/// Razred koji predstavlja zemljisnu knjigu
	/// </summary>
	public class LandRegistry : NHibernateEntity {

		#region Members

		/// <summary>
		/// Katastarska cestica
		/// </summary>
		[BusinessKeyOfEntity]
		private CadastralParticle cadastralParticle;

		/// <summary>
		/// Dohvaca katastarsku cesticu
		/// </summary>
		public virtual CadastralParticle CadastralParticle {
			get { return cadastralParticle; }
		}

		/// <summary>
		/// Katastar
		/// </summary>
		public virtual Cadastre Cadastre {
			get { return cadastralParticle.Cadastre; }
		}

		/// <summary>
		/// Etazirani prostori
		/// </summary>
		private Iesi.Collections.Generic.ISet<IPartitionSpace> partitionSpaces;

		/// <summary>
		/// Dohvaca sve etazirane prostore
		/// </summary>
		public virtual Iesi.Collections.Generic.ISet<IPartitionSpace> PartitionSpaces {
			get { return new ImmutableSet<IPartitionSpace>(partitionSpaces); }
		}

		/// <summary>
		/// Dohvaca zajednicke etazirane prostore
		/// </summary>
		public virtual IList<IPartitionSpace> JointOwnershipPartitionSpaces {
			get { return new ReadOnlyCollection<IPartitionSpace>(partitionSpaces.Where(x => !x.IsOwnedPartitionSpace).ToList()); }
		}

		/// <summary>
		/// Dohvaca vlasnicke etazirane prostore
		/// </summary>
		public virtual IList<IPartitionSpace> OwnedPartitionSpaces {
			get { return new ReadOnlyCollection<IPartitionSpace>(partitionSpaces.Where(x => x.IsOwnedPartitionSpace).ToList()); }
		}


		/// <summary>
		/// Dohvaca totalnu povrsinu svih vlasnickih etaziranih prostora
		/// </summary>
		public virtual decimal TotalSurfaceOfOwnedPartitions {
			get { return partitionSpaces.Where(x => x.IsOwnedPartitionSpace).Sum(x => x.SurfaceArea); }
		}


		/// <summary>
		/// Dohvaca ukupnu povrsinu svih zajednickih etaziranih prostora.
		/// </summary>
		public virtual decimal TotalSurfaceOfJointOwnershipPartitions {
			get { return partitionSpaces.Where(x => !x.IsOwnedPartitionSpace).Sum(x => x.SurfaceArea); }
		}

		/// <summary>
		/// Dohvaca ukupnu povrsinu svih etaziranih prostora.
		/// </summary>
		public virtual decimal TotalSurfaceOfPartitionSpaces {
			get { return partitionSpaces.Sum(x => x.SurfaceArea); }
		}

		/// <summary>
		/// Indikacija da li je zemljisna knjiga zakljucana
		/// </summary>
		public virtual bool Locked { get; set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected LandRegistry() { }	

		/// <summary>
		/// Konsturktor
		/// </summary>
		/// <param name="cadastralParticle">katastarska cestica</param>
		public LandRegistry(CadastralParticle cadastralParticle) {
			this.cadastralParticle = cadastralParticle;
			partitionSpaces = new HashedSet<IPartitionSpace>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Stvara i dodaje etazu zajednickog vlasnistva u zamljisnu knjigu
		/// </summary>
		/// <param name="registryNumber">broj uloška</param>
		/// <param name="surfaceArea">povrsina etaze</param>
		/// <param name="description">opis etaze</param>
		/// <returns>novostvorena etaza zajednickog vlasnistva</returns>
		public virtual IPartitionSpace CreatePartitionSpace(string registryNumber, decimal surfaceArea, string description) {
			return CreatePartitionSpace(registryNumber, surfaceArea, description, null);
		}

		/// <summary>
		/// Stvara i dodaje etazu zajednickog vlasnistva u zamljisnu knjigu
		/// </summary>
		/// <param name="registryNumber">broj uloška</param>
		/// <param name="surfaceArea">povrsina etaze</param>
		/// <param name="description">opis etaze</param>
		/// <param name="owner">vlasnici</param>
		/// <returns>novostvorena etaza zajednickog vlasnistva</returns>
		public virtual IPartitionSpace CreatePartitionSpace(string registryNumber, decimal surfaceArea, string description, Person owner) {
			if (TotalSurfaceOfPartitionSpaces + surfaceArea > cadastralParticle.SurfaceArea) {
				throw new BusinessRulesException("Ukupna površina svi etaža zajedno sa ovom etažom bila bi veća od ukupne površine katastarske čestice.");
			}

			var ordinalNumber = partitionSpaces.Any() ? partitionSpaces.Last().OrdinalNumber + 1 : 1;
			PartitionSpace partitionSpace = new PartitionSpace(this, registryNumber, ordinalNumber, surfaceArea, description) {
				Owner = owner
			};

			partitionSpaces.Add(partitionSpace);

			return partitionSpace;
		}


		#endregion

	}
}
