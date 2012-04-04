using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.Legislature {
	/// <summary>
	/// Razred koji predstavlja katastar
	/// </summary>
	public class Cadastre : EntityBase {

		#region Members

		/// <summary>
		/// Grad katastra
		/// </summary>
		public City City { get; set; }

		/// <summary>
		/// Maticni broj katastarske opcine
		/// </summary>
		public string Mbr { get; set; }

		/// <summary>
		/// Katastarska opcina
		/// </summary>
		public string CadastralDistrict { get; set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="cadastalDistrict">katastarska opcina</param>
		/// <param name="mbr">maticni broj katastarske opcine</param>
		/// <param name="city">grad katastra</param>
		public Cadastre(string cadastalDistrict, string mbr, City city) {
			CadastralDistrict = cadastalDistrict;
			Mbr = mbr;
			City = city;
		}

		#endregion

		#region Methods

		#endregion

	}
}
