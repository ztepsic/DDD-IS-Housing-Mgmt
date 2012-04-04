using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel {
	public class City : EntityBase {

		#region Members

		/// <summary>
		/// Dohvaca ili postavlja postanski broj grada
		/// </summary>
		public int PostalCode { get; set; }

		/// <summary>
		/// Dohvaca ili postavlja naziv grada
		/// </summary>
		public string Name { get; set; }

		#endregion

		#region Constructors and Init

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
