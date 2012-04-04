using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain {
	/// <summary>
	/// Entitet koji predstavlja grad
	/// </summary>
	public class City : NHibernateEntity {

		#region Members

		/// <summary>
		/// Dohvaca ili postavlja postanski broj grada
		/// </summary>
		[BusinessKeyOfEntity]
		public virtual int PostalCode { get; set; }

		/// <summary>
		/// Dohvaca ili postavlja naziv grada
		/// </summary>
		public virtual string Name { get; set; }

		#endregion

		#region Constructors and Init


		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected City() { }		

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="postalCode">postanski broj grada</param>
		/// <param name="name">naziv grada</param>
		public City(int postalCode, string name) {
			PostalCode = postalCode;
			Name = name;
		}

		#endregion

		#region Methods
		#endregion

	}
}
