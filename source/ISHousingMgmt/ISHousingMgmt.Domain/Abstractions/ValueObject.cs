using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ISHousingMgmt.Domain.Abstractions {
	/// <summary>
	/// Razred koji osigurava standardne metode za usporedbu vrijednosnih objekata.
	/// Dva vrijednosna objekta su jednaka kada imaju sva svojstva jednaka.
	/// </summary>
	public abstract class ValueObject : DomainObject {

		#region Members
		#endregion

		#region Constructors and Init
		#endregion

		#region Methods

		/// <summary>
		/// Metoda dohvaca sva svojstva koja predstavljaju vrijednosni objekt.
		/// </summary>
		protected override IEnumerable<PropertyInfo> getTypeSpecificSignatureProperties() {
			return GetType().GetProperties();
		}

		#endregion
	}
}
