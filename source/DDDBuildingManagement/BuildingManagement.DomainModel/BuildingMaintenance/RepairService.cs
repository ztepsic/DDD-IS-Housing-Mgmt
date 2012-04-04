using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.BuildingMaintenance {
	/// <summary>
	/// Razred koji predstavlja uslugu popravka
	/// </summary>
	public class RepairService : EntityBase {

		#region Members

		/// <summary>
		/// Naziv usluge popravka
		/// </summary>
		public string Name { get; set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="name">naziv usluge popravka</param>
		public RepairService(string name) {
			Name = name;
		}

		#endregion

		#region Methods
		#endregion

	}
}
