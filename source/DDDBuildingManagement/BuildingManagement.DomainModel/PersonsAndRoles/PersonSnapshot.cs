using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
	/// <summary>
	/// Razred(Value Object) koji predstavlja povijesnu snimku neke osobe
	/// </summary>
	public class PersonSnapshot {

		#region Members

		/// <summary>
		/// Puni naziv osobe
		/// </summary>
		private readonly string fullName;

		/// <summary>
		/// Puni naziv osobe
		/// </summary>
		public string FullName { get { return fullName; } }

		/// <summary>
		/// Osobni identifikacijski broj
		/// </summary>
		private readonly string oib;

		/// <summary>
		/// Osobni identifikacijski broj
		/// </summary>
		public string Oib { get { return oib; } }

		/// <summary>
		/// Adresa pravne osobe
		/// </summary>
		private readonly Address address;

		/// <summary>
		/// Adresa pravne osobe
		/// </summary>
		public Address Address { get { return address; } }

		/// <summary>
		/// Lista telefonskih brojeva
		/// </summary>
		private readonly IList<Telephone> telephoneList;

		/// <summary>
		/// Dohvaca listu telefonskih brojeva
		/// </summary>
		public IList<Telephone> Telephones {
			get { return new ReadOnlyCollection<Telephone>(telephoneList); }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="person">osoba</param>
		public PersonSnapshot(Person person) {
			this.fullName = person.FullName;
			this.oib = person.Oib;
			this.address = person.Address;
			this.telephoneList = person.Telephones;
		}

		#endregion

		#region Methdos

		/// <summary>
		/// Usporeduje da li su dva objekta jednaka.
		/// </summary>
		/// <param name="obj">objekt s kojim se usporeduje</param>
		/// <returns>True ako jesu, false inace.</returns>
		public override bool Equals(object obj) {
			if(obj == null || GetType() != obj.GetType()) {
				return false;
			}

			PersonSnapshot personSnapshot = (PersonSnapshot) obj;
			return oib == personSnapshot.Oib;
		}

		/// <summary>
		/// HashCode ovog objekta
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return oib.GetHashCode();
		}

		#endregion

	}
}
