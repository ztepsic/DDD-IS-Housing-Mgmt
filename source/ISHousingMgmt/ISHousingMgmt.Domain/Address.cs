using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;

namespace ISHousingMgmt.Domain {
	/// <summary>
	/// Razred(Value Object) koji predstavlja adresu
	/// </summary>
	public class Address : ValueObject {

		#region Members

		/// <summary>
		/// Grad
		/// </summary>
		private readonly City city;

		/// <summary>
		/// Dohvaca grad
		/// </summary>
		public virtual City City {
			get { return city; }
		}

		/// <summary>
		/// Ulica
		/// </summary>
		private readonly string streetAddress;

		/// <summary>
		/// Dohvaca ulicu
		/// </summary>
		public virtual string StreetAddress {
			get { return streetAddress; }
		}

		/// <summary>
		/// Broj ulice
		/// </summary>
		private readonly string streetAddressNumber;

		/// <summary>
		/// Dohvaca broj ulice
		/// </summary>
		public virtual string StreetAddressNumber {
			get { return streetAddressNumber; }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Defaultni kontruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		private Address() : this(string.Empty, string.Empty, new City(0, string.Empty)) { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="streetAddress">ulica</param>
		/// <param name="streetAddressNumber">broj ulice</param>
		/// <param name="city">grad</param>
		public Address(string streetAddress, string streetAddressNumber, City city) {
			this.streetAddress = streetAddress;
			this.streetAddressNumber = streetAddressNumber;

			if(city == null) {
				throw new BusinessRulesException("City can't be null.");
			}

			this.city = city;
		}

		#endregion

		#region Methods

		public override string ToString() {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(StreetAddress);
			stringBuilder.Append(" ");
			stringBuilder.Append(StreetAddressNumber);
			stringBuilder.Append(", ");
			stringBuilder.Append(City.PostalCode);
			stringBuilder.Append(" ");
			stringBuilder.Append(City.Name);

			return stringBuilder.ToString();
		}

		#endregion

	}
}
