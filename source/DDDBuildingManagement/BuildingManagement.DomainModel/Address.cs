using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel {
	/// <summary>
	/// Razred(Value Objekt) koji predstavlja adresu.
	/// </summary>
	public class Address {

		#region Members

		/// <summary>
		/// Grad
		/// </summary>
		private readonly City city;

		/// <summary>
		/// Dohvaca grad
		/// </summary>
		public City City {
			get { return city; }
		}

		/// <summary>
		/// Ulica
		/// </summary>
		private readonly string streetAddress;

		/// <summary>
		/// Dohvaca ulicu
		/// </summary>
		public string StreetAddres {
			get { return streetAddress; }
		}

		/// <summary>
		/// Broj ulice
		/// </summary>
		private readonly string streetAddressNumber;

		/// <summary>
		/// Dohvaca broj ulice
		/// </summary>
		public string StreetAddressNumber {
			get { return streetAddressNumber; }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="streetAddress">ulica</param>
		/// <param name="streetAddressNumber">broj ulice</param>
		/// <param name="city">grad</param>
		public Address(string streetAddress, string streetAddressNumber, City city) {
			this.streetAddress = streetAddress;
			this.streetAddressNumber = streetAddressNumber;
			this.city = city;
		}

		#endregion

		#region Methods

		#endregion

	}
}
