using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BuildingMaintenance;

namespace ISHousingMgmt.Domain.PersonsAndRoles {
	/// <summary>
	/// Repozitorij izvodaca radova
	/// </summary>
	public interface IContractorsRepository : ICrudRepository<Contractor> {

		/// <summary>
		/// Dohvaca sve izvodace radova koji ne suraduju sa upraviteljem
		/// </summary>
		/// <param name="buildingManagerId">identifikator upravitelja</param>
		/// <returns></returns>
		IList<Contractor> GetNonBuildingMgmtContractors(int buildingManagerId);

		/// <summary>
		/// Dohvaca izvodace radova za upravitelja i zadanu vrstu popravka
		/// </summary>
		/// <param name="repairService">vrsta popravka</param>
		/// <param name="buildingManager">upravitelj</param>
		/// <returns>izvodaci radova koji obavaljaju zadanu vrstu popravka i u partnerstvu su sa zadanim upraviteljem</returns>
		IList<Contractor> GetContractorsByRepairService(RepairService repairService, BuildingManager buildingManager);

		/// <summary>
		/// Dohvaca izvodaca na temelju osobe
		/// </summary>
		/// <param name="legalPerson">osoba</param>
		/// <returns>izvodac radova</returns>
		Contractor GetContractorByPerson(LegalPerson legalPerson);

	}
}
