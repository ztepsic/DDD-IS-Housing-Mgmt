using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Iesi.Collections.Generic;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;

namespace ISHousingMgmt.Domain.PersonsAndRoles {
	/// <summary>
	/// Apstraktni razred(Entity) koji predstavlja osobu
	/// </summary>
	public abstract class Person : NHibernateEntity {

		#region Members

		/// <summary>
		/// Broj znamenaka oib-a
		/// </summary>
		private const int NUMBER_OF_OIB_DIGITS = 11;

		/// <summary>
		/// Naziv osobe
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// Puni naziv osobe
		/// </summary>
		public virtual string FullName { get { return Name; } }

		/// <summary>
		/// Osobni identifikacijski broj
		/// </summary>
		[BusinessKeyOfEntity]
		public virtual string Oib { get; set; }

		/// <summary>
		/// Adresa osobe
		/// </summary>
		public virtual Address Address { get; set; }

		/// <summary>
		/// Lista telefonskih brojeva
		/// </summary>
		private Iesi.Collections.Generic.ISet<Telephone> telephones;

		/// <summary>
		/// Dohvaca listu telefonskih brojeva
		/// </summary>
		public virtual Iesi.Collections.Generic.ISet<Telephone> Telephones {
			get { return new ImmutableSet<Telephone>(telephones); }
			// get { return telephones; }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Defaultni kontruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected Person() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		protected Person(string oib, string name) {
			Oib = oib;
			Name = name;
			telephones = new HashedSet<Telephone>();

			IsValid();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Dodaje novi broj telefona osobi.
		/// Ukoliko već postoji telefosnki broj s istim imenom, postojeći 
		/// se zamjenjuje novim.
		/// </summary>
		/// <param name="telephone">broj telefona</param>
		public virtual void AddTelephone(Telephone telephone) {
			var isSucceded = telephones.Add(telephone);
			if(!isSucceded) {
				var existingTelephone =
					telephones.Where(t => t.NameOfTelephoneNumber == telephone.NameOfTelephoneNumber).SingleOrDefault();
				RemoveTelephone(existingTelephone);
				telephones.Add(telephone);
			}
			
		}

		/// <summary>
		/// Brise telefonski broj
		/// </summary>
		/// <param name="telephone">telefonski broj</param>
		/// <returns>True ako je brisanje uspjelo, inace false</returns>
		public virtual bool RemoveTelephone(Telephone telephone) {
			return telephones.Remove(telephone);
		}

		/// <summary>
		/// Brise sve telefonske brojeve
		/// </summary>
		public virtual void ClearAllTelephones() {
			telephones.Clear();
		}

		/// <summary>
		/// Provjerava da li je oib ispravan
		/// </summary>
		/// <returns>true ukoliko je oib ispravan, inace false</returns>
		private bool isOibValid() {
			if (Oib.Length != NUMBER_OF_OIB_DIGITS) {
				return false;
			}

			Regex regex = new Regex(@"\d");
			var isNumber = regex.IsMatch(Oib);

			if (!isNumber) {
				return false;
			}

			return true;
		}

		/// <summary>
		/// Provjerava da li je trenutni objekt u ispravnom stanju
		/// </summary>
		public virtual void IsValid() {
			var errors = new BusinessRulesException<Person>();

			if (!isOibValid()) {
				errors.AddErrorFor(x => x.Oib, "Oib is not valid.");
			}

			if (errors.Errors.Any()) {
				throw errors;
			}
		}

		#endregion

	}
}
