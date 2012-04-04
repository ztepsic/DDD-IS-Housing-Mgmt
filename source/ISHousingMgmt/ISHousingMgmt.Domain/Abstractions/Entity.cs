using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ISHousingMgmt.Domain.Abstractions {

	/// <summary>
	/// Dekorira svojstva Entiteta koja jedinstveno identificiraju entitet kada
	/// identifikator nije definiran.
	/// Navedeni atribut provjerava se u getTypeSpecificSignatureProperties
	/// </summary>
	/// <remarks>
	/// Koristi se iskljucivo za <see cref="Entity" />.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class BusinessKeyOfEntityAttribute : Attribute { }

	/// <summary>
	/// Razred predstavlja bazni razred za objekte (enitete) koji ce se perzistirati u bazi podataka.
	/// Razred ukljucuje svojstvo identiteta tipa int te mogucnosti usporedivanja entiteta
	/// metodama Equals() i GetHashCode().
	/// 
	/// Za koristenje nekog drugog tipa za identitet koristiti razred <see cref="Entity{TId}"/>
	/// </summary>
	public abstract class Entity : Entity<int> { }

	/// <summary>
	/// Razred predstavlja bazni razred za objekte (enitete) koji ce se perzistirati u bazi podataka.
	/// Razred ukljucuje svojstvo identiteta te mogucnosti usporedivanja entiteta
	/// metodama Equals() i GetHashCode().
	/// </summary>
	public abstract class Entity<TId> : DomainObject {

		#region Members

		/// <summary>
		/// Identifikator entiteta.
		/// Setter je protected da omogući unit testovima postavljanje tog svojstva putem reflectiona i da
		/// omoguci vise fleksibilnosti za postavljanje rucno dodjeljenog identifikatora.
		/// </summary>
		public virtual TId Id { get; protected set; }

		private int? cachedHashcode;

		#endregion

		#region Constructors and Init
		#endregion

		#region Methods

		/// <summary>
		/// Da li je objekt tranzijentan. Tanzijentni objekti nemaju dodjeljen identifikator - npr.
		/// vrijednost nedodjeljenog identifikatora moze biti, zavisno o tipu, null ili 0.
		/// </summary>
		private static bool isTransient(Entity<TId> obj) {
			return obj != null && Equals(obj.Id, default(TId));
		}

		/// <summary>
		/// Metoda pomocu koje se definiraju svojstva koja definiraju identitet nekog objekta.
		/// </summary>
		protected override IEnumerable<PropertyInfo> getTypeSpecificSignatureProperties() {
			return GetType().GetProperties()
				.Where(p => Attribute.IsDefined(p, typeof(BusinessKeyOfEntityAttribute), true));
		}

		/// <summary>
		/// Metoda koja provjerava da li je zadani entitet jednak trenutnoj instanci
		/// </summary>
		/// <param name="obj">Entitet koji se usporeduje sa trenutnom instancom</param>
		/// <returns>True ukoliko su entiteti jednaki, false inace.</returns>
		public override bool Equals(object obj) {
			return Equals(obj as Entity<TId>);
		}

		public virtual bool Equals(Entity<TId> other) {
			if(other == null) {
				return false;
			}

			if(ReferenceEquals(this, other)) {
				return true;
			}

			// Ako nisu tranzijentni i imaju jednaki identifikator,
			// moraju biti jednakog tipa, odnosno podtipa.
			if(!isTransient(this) &&
				!isTransient(other) &&
				Equals(Id, other.Id)) {
				//var otherType = other.getUnproxiedType().UnderlyingSystemType;
				//var thisType = getUnproxiedType().UnderlyingSystemType;
				var otherType = other.GetUnproxiedType();
				var thisType = GetUnproxiedType();

				return thisType.Equals(otherType);

			} else {
				// Ako su oba objekta tranzijentna provjeri po poslovnome kljucu,
				// inace vrati false
				return isTransient(this) &&
				       isTransient(other) &&
				       hasSameObjectSignatureAs(other);
			}


		}

		/// <summary>
		/// Stvara hashcode objekta koristeci svojstva objekta koja definiranju njegov identitet.
		/// </summary>
		public override int GetHashCode() {
			if (cachedHashcode.HasValue)
				return cachedHashcode.Value;

			if (isTransient(this)) {
				cachedHashcode = base.GetHashCode();
			} else {
				int hashCode = GetType().GetHashCode();
				cachedHashcode = (hashCode * HASH_MULTIPLIER) ^ Id.GetHashCode();
			}

			return cachedHashcode.Value;

		}

		#endregion
	}

}
