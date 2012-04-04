using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.BuildingManagement {
	public interface IBuildingsRepository : ICrudRepository<Building> {
		/// <summary>
		/// Dohvaca sve zgrade za zadanog upravitelja
		/// </summary>
		/// <param name="buildingManager">upravitelj</param>
		/// <returns>sve zgrade kojima upravlja zadani upravitelj</returns>
		IList<Building> GetBuildingsByManager(BuildingManager buildingManager);

		/// <summary>
		/// Dohvaca sve zgrade za zadanog upravitelja
		/// </summary>
		/// <param name="buildingManager">upravitelj</param>
		/// <returns>sve zgrade kojima upravlja zadani upravitelj</returns>
		IList<Building> GetBuildingsByManager(LegalPerson buildingManager);

		/// <summary>
		/// Dohvaca sve zgrade za zadanog upravitelja
		/// </summary>
		/// <param name="buildingManager">upravitelj</param>
		/// <param name="limit">broj zgrada koji se dohvaca</param>
		/// <returns>sve zgrade kojima upravlja zadani upravitelj</returns>
		IList<Building> GetBuildingsByManager(LegalPerson buildingManager, int limit);

		/// <summary>
		/// Dohvaca sve zgrade za zadanog predstavnika suvlasnika
		/// </summary>
		/// <param name="representative">predstavnik suvlasnika</param>
		/// <returns>sve zgrade koji imaju zadanog predstavnika suvlasnika</returns>
		IList<Building> GetBuildingsByRepresentative(Person representative);

		/// <summary>
		/// Dohvaca sve zgrade za zadanog predstavnika suvlasnika
		/// </summary>
		/// <param name="representative">predstavnik suvlasnika</param>
		/// <param name="limit">broj zgrada koji se dohvaca</param>
		/// <returns>sve zgrade koji imaju zadanog predstavnika suvlasnika</returns>
		IList<Building> GetBuildingsByRepresentative(Person representative, int limit);

		/// <summary>
		/// Dohvaca sve zgrade za zadanog svulasnika
		/// </summary>
		/// <param name="owner">suvlasnik</param>
		/// <returns>zgrade u kojima zadani suvlasnik ima stan</returns>
		IList<Building> GetBuildingsByOwner(Person owner);

		/// <summary>
		/// Dohvaca zgradu za etazu
		/// </summary>
		/// <param name="partitionSpace">etaza</param>
		/// <returns>zgrada</returns>
		Building GetByPartitionSpace(IPartitionSpace partitionSpace);

		/// <summary>
		/// Dohvaca zgradu za zemljisnu knjigu
		/// </summary>
		/// <param name="landRegistry">zemljisna knjiga</param>
		/// <returns>zgrada</returns>
		Building GetByLandRegistry(LandRegistry landRegistry);

	}
}
