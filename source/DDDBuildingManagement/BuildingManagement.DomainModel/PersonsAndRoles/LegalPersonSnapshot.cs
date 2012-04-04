using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
	/// <summary>
	/// Razred(Value Object) koji predstavlja povijesnu snimku pravne osobe
	/// </summary>
	public class LegalPersonSnapshot : PersonSnapshot {

		#region Members
		/// <summary>
		/// Broj bankovnog racuna
		/// </summary>
		private readonly string numberOfBankAccount;

		/// <summary>
		/// Broj bankovnog racuna
		/// </summary>
		public string NumberOfBankAccount { get { return numberOfBankAccount; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="legalPerson">pravna osoba</param>
		public LegalPersonSnapshot(LegalPerson legalPerson) : base(legalPerson) {
			this.numberOfBankAccount = legalPerson.NumberOfBankAccount;
		}

		#endregion

		#region Methods
		#endregion

	}
}
