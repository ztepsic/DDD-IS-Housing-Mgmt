using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.BuildingManagement {
	public interface IAdminJobsVotingsRepository : ICrudRepository<AdministrationJobsVoting> {

		/// <summary>
		/// Dohvaca sve poslove uprave za zadanu zgradu
		/// </summary>
		/// <param name="building">zgrada</param>
		/// <returns>poslovi uprave</returns>
		IList<AdministrationJobsVoting> GetByBuilding(Building building);

		/// <summary>
		/// Dohvaca odreden broj poslova (najnovije dodane) uprave za zadanu zgradu
		/// </summary>
		/// <param name="building">zgrada</param>
		/// <param name="limit">broj poslova koji se dohvaca</param>
		/// <returns>poslovi uprave</returns>
		IList<AdministrationJobsVoting> GetByBuilding(Building building, int limit);

	}
}
