using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.Legislature {
	/// <summary>
	/// Razred(Entity), composite pattern, koji predstavlja katastarsku cesticu koja se sastoji
	/// od vise podkatastarskih cestica.
	/// </summary>
	public class CadastralParticleComposite : AbstractCadastralParticle {

		#region Members

		/// <summary>
		/// Niz znakova za konkatenaciju dva zapisa
		/// </summary>
		private const string CONCATENATION_STRING = " i ";

		/// <summary>
		/// Lista koja sadrzi listu katastarskih cestica
		/// </summary>
		private readonly IList<CadastralParticle> cadastralParticles;

		/// <summary>
		/// Dohvaca listu katastarskih cestica
		/// </summary>
		public IList<CadastralParticle> CadastralParticles {
			get { return new ReadOnlyCollection<CadastralParticle>(cadastralParticles); }
		}

		/// <summary>
		/// Dohvaca povrsinu katastarskih cestica
		/// </summary>
		public override decimal SurfaceArea {
			get {
				return cadastralParticles.Sum(cadastralParicle => cadastralParicle.SurfaceArea);
			}
		}

		/// <summary>
		/// Dohvaca opis katastarske cestice
		/// </summary>
		public override string Description {
			get {
				StringBuilder stringBuilder = new StringBuilder();
				for(int i = 0; i < cadastralParticles.Count; i++) {
					if (i == cadastralParticles.Count - 1) {
						stringBuilder.AppendFormat("{0}", cadastralParticles[i].Description);	
					} else {
						stringBuilder.AppendFormat("{0}{1}", cadastralParticles[i].Description, CONCATENATION_STRING);
					}
					
				}

				return stringBuilder.ToString();
			}
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="cadastre">katastar kojemu pripada katastarska cestica</param>
		/// <param name="numberOfCadastralParticle">broj katastarske cestice</param>
		public CadastralParticleComposite(Cadastre cadastre, string numberOfCadastralParticle) : base(cadastre) {
			NumberOfCadastralParticle = numberOfCadastralParticle;

			cadastralParticles = new List<CadastralParticle>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Dodaje katastarske cestice
		/// </summary>
		/// <param name="cadastralParticle">katastarska cestica</param>
		public void Add(CadastralParticle cadastralParticle) {
			cadastralParticles.Add(cadastralParticle);
		}

		#endregion

	}
}
