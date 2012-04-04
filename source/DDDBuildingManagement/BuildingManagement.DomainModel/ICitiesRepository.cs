using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel {
	/// <summary>
	/// Repozitorij grada
	/// </summary>
	public interface ICitiesRepository : ICrudRepository<City> {
		/// <summary>
		/// Dohvaca grad preko postanskog broja
		/// </summary>
		/// <param name="postalCode">postanski broj grada</param>
		/// <returns></returns>
		City GetCityByPostalCode(int postalCode);
	}
}
