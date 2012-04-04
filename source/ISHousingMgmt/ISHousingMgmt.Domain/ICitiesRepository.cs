using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain {
	/// <summary>
	/// Repozitorij gradova
	/// </summary>
	public interface ICitiesRepository : IReadOnlyRepository<City> {
		/// <summary>
		/// Dohvaca grad preko postanskog broja
		/// </summary>
		/// <param name="postalCode">postanski broj grada</param>
		/// <returns></returns>
		City GetCityByPostalCode(int postalCode);

		/// <summary>
		/// Dohvaca gradove za koje postoje katastarske opcine
		/// </summary>
		/// <returns>gradovi za koje postoje katastarske opcine</returns>
		IList<City> GetCitiesWithCadastres();
	}
}
