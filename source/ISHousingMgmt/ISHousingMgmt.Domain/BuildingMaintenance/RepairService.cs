using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.BuildingMaintenance {
	/// <summary>
	/// Razred koji predstavlja uslugu popravka
	/// </summary>
	public class RepairService : NHibernateEntity {

		#region Members

		/// <summary>
		/// Naziv usluge popravka
		/// </summary>
		public virtual string Name { get; set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected RepairService() { }

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
