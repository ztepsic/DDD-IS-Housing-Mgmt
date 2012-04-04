using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using Iesi.Collections.Generic;

namespace ISHousingMgmt.Domain.PersonsAndRoles {
	/// <summary>
	/// Razred(Value Object) koji predstavlja povijesnu snimku neke osobe
	/// </summary>
	public class PersonSnapshot : ValueObject {

		#region Members

		/// <summary>
		/// Puni naziv osobe
		/// </summary>
		private readonly string fullName;

		/// <summary>
		/// Puni naziv osobe
		/// </summary>
		public virtual string FullName { get { return fullName; } }

		/// <summary>
		/// Osobni identifikacijski broj
		/// </summary>
		private readonly string oib;

		/// <summary>
		/// Osobni identifikacijski broj
		/// </summary>
		public virtual string Oib { get { return oib; } }

		/// <summary>
		/// Adresa pravne osobe
		/// </summary>
		private readonly Address address;

		/// <summary>
		/// Adresa pravne osobe
		/// </summary>
		public virtual Address Address { get { return address; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Defaultni kontruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected PersonSnapshot() {
			fullName = string.Empty;
			oib = string.Empty;
			address = null;
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="person">osoba</param>
		public PersonSnapshot(Person person) {
			fullName = person.FullName;
			oib = person.Oib;
			address = person.Address;
		}

		#endregion

		#region Methods

		#endregion

	}
}