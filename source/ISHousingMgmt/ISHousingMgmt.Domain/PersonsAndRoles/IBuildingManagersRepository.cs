using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.PersonsAndRoles {
	public interface IBuildingManagersRepository : ICrudRepository<BuildingManager> {

		/// <summary>
		/// Dohvaca upravitelja na temelju pravne osobe
		/// </summary>
		/// <param name="legalPerson">pravna osoba za koju se pretpostavlja da je upravitelj</param>
		/// <returns>upravitelj</returns>
		BuildingManager GetByLegalPerson(LegalPerson legalPerson);

	}
}
