using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.PersonsAndRoles {
	/// <summary>
	/// Razred(Value object) koji predstavlja telefonski broj
	/// </summary>
	public class Telephone : ValueObject {

		#region Members

		/// <summary>
		/// Naziv telefonskog broja
		/// </summary>
		private readonly string nameOfTelephoneNumber;

		/// <summary>
		/// Dohvaca naziv telefonskog broja
		/// </summary>
		public string NameOfTelephoneNumber { get { return nameOfTelephoneNumber; } }

		/// <summary>
		/// Telefonski broj
		/// </summary>
		private readonly string telephoneNumber;

		/// <summary>
		/// Dohvaca telefonski broj
		/// </summary>
		public string TelephoneNumber { get { return telephoneNumber; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Defaultni kontruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		private Telephone() : this(string.Empty, string.Empty) { }

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
		#endregion

	}
}
