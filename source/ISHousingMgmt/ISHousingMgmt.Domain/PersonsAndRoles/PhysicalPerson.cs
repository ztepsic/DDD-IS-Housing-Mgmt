using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISHousingMgmt.Domain.PersonsAndRoles {
	/// <summary>
	/// Razred(Entity) koji predstavlja fizicku osobu
	/// </summary>
	public class PhysicalPerson : Person {

		#region Members

		/// <summary>
		/// Prezime osobe
		/// </summary>
		public virtual string Surname { get; set; }

		/// <summary>
		/// Puno ime - ime + prezime
		/// </summary>
		public override string FullName { get { return Name + " " + Surname; } }

		#endregion

		#region Consructors and Init

		/// <summary>
		/// Defaultni kontruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected PhysicalPerson() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="oib">oib</param>
		/// <param name="name">ime fizicke osobe</param>
		/// <param name="surname">prezime fizicke osobe</param>
		public PhysicalPerson(string oib, string name, string surname)
			: base(oib, name) {
			Surname = surname;
		}

		#endregion

		#region Methods
		#endregion

	}
}
