using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.Legislature {
	/// <summary>
	/// Razred(Entity) koji predstavlja katastarsku cesticu
	/// </summary>
	public class CadastralParticle : ValueObject {

		#region Members

		/// <summary>
		/// Katastar
		/// </summary>
		private readonly Cadastre cadastre;

		/// <summary>
		/// Dohvaca katastar
		/// </summary>
		public virtual Cadastre Cadastre { get { return cadastre; } }


		/// <summary>
		/// Broj katastarske cestice
		/// </summary>
		private readonly string numberOfCadastralParticle;

		/// <summary>
		/// Dohvaca katastarske cestice
		/// </summary>
		public virtual string NumberOfCadastralParticle { get { return numberOfCadastralParticle; } }


		/// <summary>
		/// Povrsina katastarske cestice
		/// </summary>
		private readonly decimal surfaceArea;

		/// <summary>
		/// Dohvaca povrsinu katastarske cestice
		/// </summary>
		public virtual decimal SurfaceArea { get { return surfaceArea; } }

		/// <summary>
		/// Opis katastarske cestice
		/// </summary>
		private readonly string description;

		/// <summary>
		/// Dohvaca opis katastaske cestice
		/// </summary>
		public virtual string Description { get { return description; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected CadastralParticle() : this(null, string.Empty, 0m, string.Empty) { }		


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="cadastre">katastar kojemu pripada katastarska cestica</param>
		/// <param name="numberOfCadastralParticle">broj katastarske cestice</param>
		/// <param name="surfaceArea">povrsina katastarske cestice</param>
		/// <param name="description">opis katastarske cestice</param>
		public CadastralParticle(Cadastre cadastre, string numberOfCadastralParticle, decimal surfaceArea, string description) {
			this.cadastre = cadastre;
			this.numberOfCadastralParticle = numberOfCadastralParticle;
			this.surfaceArea = surfaceArea;
			this.description = description;
		}

		#endregion

		#region Methods

		#endregion

	}
}
