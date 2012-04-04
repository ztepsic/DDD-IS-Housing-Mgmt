using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.Legislature {
	/// <summary>
	/// Razred koji predstavlja etazu
	/// </summary>
	public class PartitionSpace : EntityBase, IPartitionSpace {

		#region Members

		/// <summary>
		/// Katastarska cestica
		/// </summary>
		private readonly AbstractCadastralParticle cadastralParticle;

		/// <summary>
		/// Dohvaca katastarsku cesticu
		/// </summary>
		public AbstractCadastralParticle CadastralParticle {
			get { return cadastralParticle; }
		}

		/// <summary>
		/// Redni broj etaze
		/// </summary>
		private int ordinalNumber;

		/// <summary>
		/// Dohbaca ili postavlja redni broj etaze
		/// </summary>
		public int OrdinalNumber {
			get { return ordinalNumber; }
			set {
				if(value != 0) {
					ordinalNumber = value;
				} else {
					throw new ArgumentException("Oridnal number can't be 0.");
				}
			}
		}

		/// <summary>
		/// Povrsina etaze
		/// </summary>
		public decimal SurfaceArea { get; set; }

		/// <summary>
		/// Opis etaziranog prostora
		/// </summary>
		public string Description { get; set; }


		/// <summary>
		/// Dohvaca vlasnika etaze
		/// </summary>
		public Person Owner { get; set; }

		/// <summary>
		/// Da li je etaza pod vlasnistvom
		/// </summary>
		public bool IsOwnedPartitionSpace {
			get { return Owner == null ? false : true; }
		}

		/// <summary>
		/// Udio ukupnog vlasnistva temeljem ove etaze
		/// </summary>
		private decimal shareOfTotalOwnership;

		/// <summary>
		/// Dohvaca udio ukupnog vlasnistva temeljem ove etaze
		/// </summary>
		public decimal ShareOfTotalOwnershihp { get; set; }


		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="cadastralParticle">katastarska cestica</param>
		/// <param name="ordinalNumber">redni broj etaze</param>
		/// <param name="surfaceArea">povrsina etaziranog prostora</param>
		/// <param name="description">opis etaziranog prostra</param>
		internal PartitionSpace(AbstractCadastralParticle cadastralParticle, int ordinalNumber, decimal surfaceArea, string description) {
			this.cadastralParticle = cadastralParticle;
			this.ordinalNumber = ordinalNumber;
			SurfaceArea = surfaceArea;
			Description = description;
			Owner = null;
			shareOfTotalOwnership = 0;
		}

		#endregion

		#region Methods

		#endregion

	}
}
