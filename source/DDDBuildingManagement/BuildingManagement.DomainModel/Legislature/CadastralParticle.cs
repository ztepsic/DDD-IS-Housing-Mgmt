using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.Legislature {
	/// <summary>
	/// Razred(Entity) koji predstavlja katastarsku cesticu
	/// </summary>
	public class CadastralParticle : AbstractCadastralParticle {

		#region Members


		/// <summary>
		/// Povrsina katastarske cestice
		/// </summary>
		private decimal surfaceArea;

		/// <summary>
		/// Dohvaca povrsinu katastarske cestice
		/// </summary>
		public override decimal SurfaceArea { get { return surfaceArea; } }

		/// <summary>
		/// Postavlja povrsinu katastarske cestice
		/// </summary>
		/// <param name="surfaceArea">povrsina katastarske cestice</param>
		public void SetSurfaceArea(decimal surfaceArea) {
			this.surfaceArea = surfaceArea;
		}

		/// <summary>
		/// Opis katastarske cestice
		/// </summary>
		private string description;

		/// <summary>
		/// Dohvaca ili postavlja opis katastaske cestice
		/// </summary>
		public override string Description { get { return description; } }

		/// <summary>
		/// Postavlja opis katastarske cestice
		/// </summary>
		/// <param name="description">opis katastarske cestice</param>
		public void SetDescription(string description) {
			if (!String.IsNullOrEmpty(description)) {
				this.description = description;
			} else {
				throw new ArgumentException("Description must be not null or empty.");
			}
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="cadastre">katastar kojemu pripada katastarska cestica</param>
		/// <param name="numberOfCadastralParticle">broj katastarske cestice</param>
		/// <param name="surfaceArea">povrsina katastarske cestice</param>
		/// <param name="description">opis katastarske cestice</param>
		public CadastralParticle(Cadastre cadastre, string numberOfCadastralParticle, decimal surfaceArea, string description) : base(cadastre) {
			NumberOfCadastralParticle = numberOfCadastralParticle;
			SetSurfaceArea(surfaceArea);
			SetDescription(description);
		}

		#endregion

		#region Methods

		#endregion

	}
}
