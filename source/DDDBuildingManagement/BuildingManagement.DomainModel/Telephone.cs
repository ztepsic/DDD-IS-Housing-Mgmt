using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
	/// <summary>
	/// Razred(Value object) koji predstavlja telefonski broj.
	/// </summary>
	public class Telephone {

		#region Members

		/// <summary>
		/// Naziv telefonskog broja
		/// </summary>
		private readonly string nameOfTelephoneNumber;

		/// <summary>
		/// Dohvaca naziv telefonskog broja
		/// </summary>
		public string NameOfTelephoneNumber {
			get { return nameOfTelephoneNumber; }
		}

		/// <summary>
		/// Telefonski broj
		/// </summary>
		private readonly string telephoneNumber;

		/// <summary>
		/// Dohvaca telefonski broj
		/// </summary>
		public string TelephoneNumber {
			get { return telephoneNumber; }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="nameOfTelephoneNumber">naziv telefonskog broja</param>
		/// <param name="telephoneNumber">broj telefona</param>
		public Telephone(string nameOfTelephoneNumber, string telephoneNumber) {
			this.nameOfTelephoneNumber = nameOfTelephoneNumber;
			this.telephoneNumber = telephoneNumber;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Provjerava da li su dva tipa jednaka
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>True ako su objekti jednaki, false inace.</returns>
		public override bool Equals(object obj) {
			if(obj == null || GetType() != obj.GetType()) {
				return false;
			}

			Telephone telephone = (Telephone) obj;
			return nameOfTelephoneNumber.Equals(telephone.nameOfTelephoneNumber) &&
			       telephoneNumber.Equals(telephone.telephoneNumber);
		}

		/// <summary>
		/// Hash funkcija ovog tipa
		/// </summary>
		/// <returns>hash code</returns>
		public override int GetHashCode() {
			return nameOfTelephoneNumber.GetHashCode() ^ telephoneNumber.GetHashCode();
		}

		#endregion

	}
}
