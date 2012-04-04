using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ISHousingMgmt.Domain.Abstractions {
	/// <summary>
	/// Razred koji osigurava standardne metode za usporedbu objekata.
	/// </summary>
	public abstract class DomainObject {

		#region Members

		/// <summary>
		/// Da bi se osigurala jedinstvenost hashcodea, odabire se pazljivo odabrani slucajan
		/// mnozitelj koji se koristi pri izracunu hashcodea.
		/// Goodrich and Tamassia's Data Structures an Algorithms in Java tvrde da brojevi
		/// 31, 33, 37, 39 i 41 proizvode najmanju koliziju brojeva.
		/// Pogledaj: http://computinglife.wordpress.com/2008/11/20/why-do-hash-functions-use-prime-numbers/
		/// </summary>
		protected const int HASH_MULTIPLIER = 31;

		/// <summary>
		/// Staticka clanska varijabla koja cacheira svojstva objekta kako ih se nebi pretrazivalo
		/// za svaku instancu istog tipa ponovno.
		/// </summary>
		[ThreadStatic]
		private static Dictionary<Type, IEnumerable<PropertyInfo>> signaturePropertiesDictionary;

		#endregion

		#region Constructors and Init
		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca originalni tip objekta.
		/// NHibernate zbog lazy evaluacije radi proxy objekt koji onda skriva originalni tip.
		/// </summary>
		/// <returns>Originalni tip objekta (ne proxy tip)</returns>
		public virtual Type GetUnproxiedType() {
			return GetType();
		}

		/// <summary>
		/// Dohvaca definirana svojstva instance objekta
		/// </summary>
		/// <returns>Definirana svojstva instance objekta</returns>
		protected virtual IEnumerable<PropertyInfo> getSignatureProperties() {
			IEnumerable<PropertyInfo> properties;

			// Inicijalizacija signaturePropertiesDictionary ukoliko nije vec inicijaliziran
			if (signaturePropertiesDictionary == null) {
				signaturePropertiesDictionary = new Dictionary<Type, IEnumerable<PropertyInfo>>();
			}

			if (signaturePropertiesDictionary.TryGetValue(GetType(), out properties)) {
				return properties;
			} else {
				return (signaturePropertiesDictionary[GetType()] = getTypeSpecificSignatureProperties());	
			}
			
		}

		/// <summary>
		/// Metoda pomocu koje se definiraju svojstva koja definiraju identitet nekog objekta.
		/// </summary>
		protected abstract IEnumerable<PropertyInfo> getTypeSpecificSignatureProperties();

		/// <summary>
		/// Metoda koja provjerava da sva svojstva imaju jednake vrijednosti kao i svojstva objekta s kojim se usporeduje.
		/// </summary>
		protected virtual bool hasSameObjectSignatureAs(DomainObject compareTo) {
			IEnumerable<PropertyInfo> signatureProperties = getSignatureProperties();

			// True ako postoji barem jedno svojstvo cije vrijednosti dvaju objekata nisu jednake, false inace
			// Objasnjenje za where djelove:
			//	- ako su vrijednosti svojstava obaju objekta null, trazi dalje (prvi where)
			//	- ako je jedna vrijednost svojstva !null, a druga null, vrijednosti svojstava nisu jednaka
			//	- ako vrijednosti svojstava nisu null, provjeriti jednakost metodom Equals
			bool conditionOfInequality = (from property in signatureProperties
			                            let valueOfThisObject = property.GetValue(this, null)
			                            let valueToCompareTo = property.GetValue(compareTo, null)
			                            where valueOfThisObject != null || valueToCompareTo != null
			                            where
			                            	(valueOfThisObject == null ^ valueToCompareTo == null) ||
			                            	(!valueOfThisObject.Equals(valueToCompareTo))
			                            select valueOfThisObject).Any();

			// Ako je uvjet nejednakosti true vrati false, inace 
			// ako postoje svojstva onda znaci da su objekti jednaki,
			// ako nema svojstava za usporedbu vrati defaultnu vrijednost Equals metode
			return conditionOfInequality
			       	? false
			       	: signatureProperties.Any() || base.Equals(compareTo);
		}


		/// <summary>
		/// Metoda koja provjerava da li je zadani objekt jednak trenutnoj instanci
		/// </summary>
		/// <param name="obj">Objekt koji se usporeduje sa trenutnom instancom</param>
		/// <returns>True ukoliko su objekti jednaki, false inace.</returns>
		public override bool Equals(object obj) {
			DomainObject compareTo = obj as DomainObject;

			if (ReferenceEquals(this, compareTo)) {
				return true;
			}

			return compareTo != null && GetUnproxiedType().Equals(compareTo.GetUnproxiedType()) &&
				hasSameObjectSignatureAs(compareTo);
		}

		/// <summary>
		/// Stvara hashcode objekta koristeci svojstva objekta koja definiranju njegov identitet.
		/// Posto je preporuceno da se hashcode ne mijenja cesto, ili ikad u svome zivotnome ciklusu,
		/// vazno je pazljivo odabrati svojstva koja definiraju identitet objekta.
		/// </summary>
		public override int GetHashCode() {
			IEnumerable<PropertyInfo> signatureProperties = getSignatureProperties();

			// Moguce je da dva objekta vrate isti hash code baziran na jednakim svojstvima, cak i
			// ako ne predstavljaju isti tip. Stoga ukljucujemo ovaj hashcode u izracun hashcodea objekta.
			int hashCode = GetType().GetHashCode();

			hashCode = signatureProperties
				.Select(property => property.GetValue(this, null))
				.Where(value => value != null)
				.Aggregate(hashCode, (current, value) => (current * HASH_MULTIPLIER) ^ value.GetHashCode());

			return signatureProperties.Any()
			       	? hashCode
					: base.GetHashCode();
		}

		public static bool operator ==(DomainObject domainObject1, DomainObject domainObject2) {
			// castanje na object inace rekurzivna petlja
			if ((object) domainObject1 == null) {
				return (object) domainObject2 == null;
			}

			return domainObject1.Equals(domainObject2);
		}

		public static bool operator !=(DomainObject domainObject1, DomainObject domainObject2) {
			return !(domainObject1 == domainObject2);
		}

		#endregion

	}
}
