using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.Legislature {
	/// <summary>
	/// Apstraktni Razred(Entity) koji predstavlja katastarsku cesticu
	/// </summary>
	public abstract class AbstractCadastralParticle : EntityBase {

		#region Members

		/// <summary>
		/// Katastar
		/// </summary>
		private readonly Cadastre cadastre;

		/// <summary>
		/// Dohvaca katastar
		/// </summary>
		public Cadastre Cadastre {
			get { return cadastre; }
		}

		/// <summary>
		/// Broj katastarske cestice
		/// </summary>
		private string numberOfCadastralParticle;

		/// <summary>
		/// Dohvaca ili postavlja broj katastarske cestice
		/// </summary>
		public string NumberOfCadastralParticle {
			get { return numberOfCadastralParticle; }
			set {
				if(!String.IsNullOrEmpty(value)) {
					numberOfCadastralParticle = value;
				} else {
					throw new ArgumentException("Number of cadastral particle can not be null or empty.");
				}
			}
		}

		/// <summary>
		/// Dohvaca povrsinu katastarske cestice
		/// </summary>
		public abstract decimal SurfaceArea { get; }


		/// <summary>
		/// Dohvaca opis katastaske cestice
		/// </summary>
		public abstract string Description { get; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="cadastre">katastar</param>
		protected AbstractCadastralParticle(Cadastre cadastre) {
			this.cadastre = cadastre;
		}

		#endregion

		#region Methods

		#endregion

	}
}
