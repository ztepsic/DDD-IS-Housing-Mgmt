using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.Legislature {
	/// <summary>
	/// Razred koji predstavlja katastar
	/// </summary>
	public class Cadastre : NHibernateEntity {

		#region Members

		/// <summary>
		/// Grad katastra
		/// </summary>
		public virtual City City { get; set; }

		/// <summary>
		/// Maticni broj katastarske opcine
		/// </summary>
		[BusinessKeyOfEntity]
		public virtual string Mbr { get; set; }

		/// <summary>
		/// Katastarska opcina
		/// </summary>
		public virtual string CadastralDistrict { get; set; }

		#endregion

		#region Constructors and Init

		
		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected Cadastre() { }	

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
