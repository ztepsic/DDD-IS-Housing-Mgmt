using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.BuildingManagement {
	public interface IBuildingsRepository : ICrudRepository<Building> {
		/// <summary>
		/// Dohvaca sve zgrade s kojima je osoba povezana (stanar, suvlasnik, predstavnik suvlasnika, upravitelj)
		/// </summary>
		/// <param name="oib"></param>
		/// <returns></returns>
		IList<Building> GetBuildings(string oib);
	}
}
