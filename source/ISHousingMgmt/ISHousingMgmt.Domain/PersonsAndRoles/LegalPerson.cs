using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISHousingMgmt.Domain.PersonsAndRoles {
	/// <summary>
	/// Razred(Entity) koji predstavlja pravnu osobu.
	/// </summary>
	public class LegalPerson : Person {

		#region Members

		/// <summary>
		/// Broj bankovnog racuna
		/// </summary>
		public virtual string NumberOfBankAccount { get; set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Defaultni kontruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected LegalPerson() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="oib">oib</param>
		/// <param name="name">ime pravne osobe</param>
		public LegalPerson(string oib, string name)
			: base(oib, name) {
		}

		#endregion

		#region Methods

		#endregion
	}
}
