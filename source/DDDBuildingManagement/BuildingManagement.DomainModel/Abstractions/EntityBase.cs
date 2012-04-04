using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.Abstractions {
	/// <summary>
	/// Apstraktni razred koji predstavlja enitet kao takav
	/// </summary>
	public abstract class EntityBase {

		#region Members

		/// <summary>
		/// Predstavlja primarni identifikator entiteta
		/// </summary>
		public object Id { get; private set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		protected EntityBase() : this(null) {}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="id">Podatak koji predstavlja identifikator, odnosno identitet entiteta.</param>
		protected EntityBase(object id) {
			Id = id;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Metoda koja provjerava da li je zadani entitet jednak trenutnoj instanci
		/// </summary>
		/// <param name="obj">Entitet koji se usporeduje sa trenutnom instancom</param>
		/// <returns>True ukoliko su entiteti jednaki, false inace.</returns>
		public override bool Equals(object obj) {
			if(obj == null || GetType() != obj.GetType()) {
				return false;
			}

			EntityBase entityBase = (EntityBase) obj;
			return (this == entityBase);
		}

		/// <summary>
		/// Nadjacavanje operatora jednakosti
		/// </summary>
		/// <param name="base1">Prva instanca eniteta s lijeve strane jednakosti.</param>
		/// <param name="base2">Druga instanca entiteta s dense strane jednakosti.</param>
		/// <returns>True ukoliko su entiteti jednaki, false inace.</returns>
		public static bool operator ==(EntityBase base1, EntityBase base2) {
			// provjera da li su oba objekta null (castanje na object inace rekurzivna petlja)
			if ((object)base1 == null && (object)base2 == null) {
				return true;
			}

			// provjera da li je jedan od entiteta null
			if ((object)base1 == null || (object)base2 == null) {
				return false;
			}

			return base1.Id == base2.Id;
		}

		/// <summary>
		/// Nadjacavanje operatora nejednakosti
		/// </summary>
		/// <param name="base1">Prva instanca eniteta s lijeve strane jednakosti.</param>
		/// <param name="base2">Druga instanca entiteta s dense strane jednakosti.</param>
		/// <returns>True ukoliko entiteti NISU jednaki, false inace.</returns>
		public static bool operator !=(EntityBase base1, EntityBase base2) {
			return !(base1 == base2);
		}

		/// <summary>
		/// Hash funkcija ovog tipa
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return Id.GetHashCode();
		}

		#endregion

	}
}
