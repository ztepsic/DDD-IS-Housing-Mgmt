using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
	/// <summary>
	/// Razred(Entitet) koji predstavlja fizicku osobu
	/// </summary>
	public class PhysicalPerson : Person {

		#region Members

		/// <summary>
		/// Ime osobe
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Prezime osobe
		/// </summary>
		public string Surname { get; set; }

		/// <summary>
		/// Puno ime - ime + prezime
		/// </summary>
		public override string FullName { get { return Name + " " + Surname; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="oib">oib</param>
		/// <param name="name">ime fizicke osobe</param>
		/// <param name="surname">prezime fizicke osobe</param>
		public PhysicalPerson(string oib, string name, string surname) : base(oib) {
			Name = name;
			Surname = surname;
		}

		#endregion

		#region Methods

		#endregion

	}
}
