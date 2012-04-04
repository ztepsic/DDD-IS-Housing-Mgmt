using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.Legislature {
	/// <summary>
	/// Razred koji predstavlja etazu
	/// </summary>
	public class PartitionSpace : NHibernateEntity, IPartitionSpace {

		#region Members

		/// <summary>
		/// Zemljisna knjiga
		/// </summary>
		private LandRegistry landRegistry;

		/// <summary>
		/// Dohvaca zemljisnu knjigu
		/// </summary>
		public virtual LandRegistry LandRegistry {
			get { return landRegistry; }
		}

		/// <summary>
		/// Dohvaca identifikator zemljisne knjige
		/// </summary>
		public virtual int LandRegistryId { get { return landRegistry.Id; } }

		/// <summary>
		/// Broj uloška
		/// </summary>
		[BusinessKeyOfEntity]
		private string registryNumber;

		/// <summary>
		/// Dohvaca ili postavlja broj uloška
		/// </summary>
		public virtual string RegistryNumber {
			get { return registryNumber; }
			set { registryNumber = value; }
		}

		/// <summary>
		/// Dohvaca katastarsku cesticu
		/// </summary>
		public virtual CadastralParticle CadastralParticle { get { return landRegistry.CadastralParticle; } }

		/// <summary>
		/// Redni broj etaze
		/// </summary>
		[BusinessKeyOfEntity]
		private int ordinalNumber;

		/// <summary>
		/// Dohbaca ili postavlja redni broj etaze
		/// </summary>
		public virtual int OrdinalNumber {
			get { return ordinalNumber; }
			set {
				if (value != 0) {
					ordinalNumber = value;
				} else {
					throw new ArgumentException("Oridnal number can't be 0.");
				}
			}
		}

		/// <summary>
		/// Povrsina etaze
		/// </summary>
		public virtual decimal SurfaceArea { get; set; }

		/// <summary>
		/// Opis etaziranog prostora
		/// </summary>
		public virtual string Description { get; set; }


		/// <summary>
		/// Dohvaca vlasnika etaze
		/// </summary>
		public virtual Person Owner { get; set; }

		/// <summary>
		/// Da li je etaza pod vlasnistvom
		/// </summary>
		public virtual bool IsOwnedPartitionSpace {
			get { return Owner == null ? false : true; }
		}

		/// <summary>
		/// Dohvaca ili postavlja udio ukupnog vlasnistva temeljem ove etaze
		/// </summary>
		public virtual decimal ShareOfTotalOwnership {
			get {
				if(IsOwnedPartitionSpace) {
					return SurfaceArea / landRegistry.TotalSurfaceOfOwnedPartitions;	
				} else {
					return 0;
				}
				
			}
		}


		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected PartitionSpace() { }	


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="landRegistry">zemljisna knjiga</param>
		/// <param name="registryNumber">broj uloška</param>
		/// <param name="ordinalNumber">redni broj etaze</param>
		/// <param name="surfaceArea">povrsina etaziranog prostora</param>
		/// <param name="description">opis etaziranog prostra</param>
		internal PartitionSpace(LandRegistry landRegistry, string registryNumber, int ordinalNumber, decimal surfaceArea, string description) {
			this.landRegistry = landRegistry;
			this.registryNumber = registryNumber;
			this.ordinalNumber = ordinalNumber;
			SurfaceArea = surfaceArea;
			Description = description;
			Owner = null;
		}

		#endregion

		#region Methods

		#endregion

	}
}
