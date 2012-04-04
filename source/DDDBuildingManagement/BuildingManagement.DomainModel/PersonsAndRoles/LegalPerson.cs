using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
	/// <summary>
	/// Razred(Entity) koji predstavlja pravnu osobu.
	/// </summary>
	public class LegalPerson : Person {

		#region Members

		/// <summary>
		/// Naziv/Ime pravne osobe
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Dohvaca puni naziv/ime pravne osobe
		/// </summary>
		public override string FullName {
			get { return Name; }
		}

		/// <summary>
		/// Broj bankovnog racuna
		/// </summary>
		public string NumberOfBankAccount { get; set; }
		
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="oib">oib</param>
		/// <param name="name">ime pravne osobe</param>
		public LegalPerson(string oib, string name) : base(oib) {
			Name = name;
		}

		#endregion

		#region Methods

		#endregion
	}
}
