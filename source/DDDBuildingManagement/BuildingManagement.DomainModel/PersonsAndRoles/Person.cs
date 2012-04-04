using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.BusinessRulesAndValidation;
using System.Text.RegularExpressions;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
	/// <summary>
	/// Apstraktni razred(Entity) koji predstavlja osobu
	/// </summary>
	public abstract class Person : EntityBase {

		#region Members

		/// <summary>
		/// Broj znamenaka oib-a
		/// </summary>
		private const int NUMBER_OF_OIB_DIGITS = 11;

		/// <summary>
		/// Puni naziv osobe
		/// </summary>
		public abstract string FullName { get; }

		/// <summary>
		/// Osobni identifikacijski broj
		/// </summary>
		public string Oib { get; set; }

		/// <summary>
		/// Adresa pravne osobe
		/// </summary>
		public Address Address { get; set; }

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
		protected Person(string oib) {
			Oib = oib;
			telephoneList = new List<Telephone>();

			IsValid();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Dodaje novi broj telefona osobi
		/// </summary>
		/// <param name="telephone">broj telefona</param>
		public void AddTelephone(Telephone telephone) {
			telephoneList.Add(telephone);
		}

		/// <summary>
		/// Brise telefonski broj
		/// </summary>
		/// <param name="telephone">telefonski broj</param>
		/// <returns>True ako je brisanje uspjelo, inace false</returns>
		public bool RemoveTelephone(Telephone telephone) {
			return telephoneList.Remove(telephone);
		}

		/// <summary>
		/// Brise sve telefonske brojeve
		/// </summary>
		public void ClearAllTelephones() {
			telephoneList.Clear();
		}

		/// <summary>
		/// Provjerava da li je oib ispravan
		/// </summary>
		/// <returns>true ukoliko je oib ispravan, inace false</returns>
		private bool isOibValid() {
			if(Oib.Length != NUMBER_OF_OIB_DIGITS) {
				return false;
			}

			Regex regex = new Regex(@"\d");
			var isNumber = regex.IsMatch(Oib);

			if(!isNumber) {
				return false;
			}

			return true;
		}

		/// <summary>
		/// Provjerava da li je trenutni objekt u ispravnom stanju
		/// </summary>
		public void IsValid() {
			var errors = new RulesException<Person>();

			if(!isOibValid()) {
				errors.ErrorFor(x => x.Oib, "Oib is not valid.");
			}

			if(errors.Errors.Any()) {
				throw errors;
			}
		}

		/// <summary>
		/// Usporeduje dva Person objekta da li su ista.
		/// Da Person objekta su ista ukoliko imaju isti OIB
		/// </summary>
		/// <param name="obj">person objekt koji se usporeduje sa trenutnim objektnom</param>
		/// <returns>True ako su isti, inace false.</returns>
		public override bool Equals(object obj) {
			if(obj == null || GetType() != obj.GetType()) {
				return false;
			}

			Person person = (Person)obj;
			return Oib.Equals(person.Oib);
		}

		/// <summary>
		/// Dohvaca hash kod trenutnog objekta
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return Oib.GetHashCode();
		}

		#endregion

	}
}
