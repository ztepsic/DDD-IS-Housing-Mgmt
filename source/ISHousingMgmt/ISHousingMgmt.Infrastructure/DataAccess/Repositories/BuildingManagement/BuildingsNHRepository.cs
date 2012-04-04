using System;
using System.Collections.Generic;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.BuildingManagement {
	public class BuildingsNHRepository : NHibernateCrudRepository<Building>, IBuildingsRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public BuildingsNHRepository() { }


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public BuildingsNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		public override IEnumerable<Building> GetAll() {
			var hql = @"from Building b
				join fetch b.LandRegistry
				join fetch b.Address.City";

			return Session.CreateQuery(hql)
				.List<Building>();
		}

		/// <summary>
		/// Dohvaca sve zgrade za zadanog upravitelja
		/// </summary>
		/// <param name="buildingManager">upravitelj</param>
		/// <returns>sve zgrade kojima upravlja zadani upravitelj</returns>
		public IList<Building> GetBuildingsByManager(BuildingManager buildingManager) {
			// radi vanjsko spajanje jer se moze desiti scenarij gdje zemljisna knjiga nije jos kreirana
			return Session.QueryOver<Building>()
				.Fetch(b => b.LandRegistry).Eager
				.Fetch(b => b.Address.City).Eager
				.Where(b => b.BuildingManager == buildingManager)
				.List();
		}

		/// <summary>
		/// Dohvaca sve zgrade za zadanog upravitelja
		/// </summary>
		/// <param name="buildingManager">upravitelj</param>
		/// <returns>sve zgrade kojima upravlja zadani upravitelj</returns>
		public IList<Building> GetBuildingsByManager(LegalPerson buildingManager) {
			var hql = @"from Building b
				join fetch b.LandRegistry
				join fetch b.Address.City
				where b.BuildingManager.LegalPerson = :buildingManager";

			return Session.CreateQuery(hql)
				.SetEntity("buildingManager", buildingManager)
				.List<Building>();
		}

		/// <summary>
		/// Dohvaca sve zgrade za zadanog upravitelja
		/// </summary>
		/// <param name="buildingManager">upravitelj</param>
		/// <param name="limit">broj zgrada koji se dohvaca</param>
		/// <returns>sve zgrade kojima upravlja zadani upravitelj</returns>
		public IList<Building> GetBuildingsByManager(LegalPerson buildingManager, int limit) {
			var hql = @"from Building b
				join fetch b.LandRegistry
				join fetch b.Address.City
				where b.BuildingManager.LegalPerson = :buildingManager";

			return Session.CreateQuery(hql)
				.SetEntity("buildingManager", buildingManager)
				.SetMaxResults(limit)
				.List<Building>();
		}

		/// <summary>
		/// Dohvaca sve zgrade za zadanog predstavnika suvlasnika
		/// </summary>
		/// <param name="representative">predstavnik suvlasnika</param>
		/// <returns>sve zgrade koji imaju zadanog predstavnika suvlasnika</returns>
		public IList<Building> GetBuildingsByRepresentative(Person representative) {
			var hql = @"from Building b
				join fetch b.LandRegistry
				join fetch b.Address.City
				where b.RepresentativeOfPartOwners = :representative";

			return Session.CreateQuery(hql)
				.SetEntity("representative", representative)
				.List<Building>();
		}

		/// <summary>
		/// Dohvaca sve zgrade za zadanog predstavnika suvlasnika
		/// </summary>
		/// <param name="representative">predstavnik suvlasnika</param>
		/// <param name="limit">broj zgrada koji se dohvaca</param>
		/// <returns>sve zgrade koji imaju zadanog predstavnika suvlasnika</returns>
		public IList<Building> GetBuildingsByRepresentative(Person representative, int limit) {
			var hql = @"from Building b
				join fetch b.LandRegistry
				join fetch b.Address.City
				where b.RepresentativeOfPartOwners = :representative";

			return Session.CreateQuery(hql)
				.SetEntity("representative", representative)
				.SetMaxResults(5)
				.List<Building>();
		}

		/// <summary>
		/// Dohvaca sve zgrade za zadanog svulasnika
		/// </summary>
		/// <param name="owner">suvlasnik</param>
		/// <returns>zgrade u kojima zadani suvlasnik ima stan</returns>
		public IList<Building> GetBuildingsByOwner(Person owner) {
			var hql = @"from Building b
				join fetch b.LandRegistry lr
				join fetch lr.PartitionSpaces ps
				where ps.Owner = :owner";

			return Session.CreateQuery(hql)
				.SetEntity("owner", owner)
				.List<Building>();

		}

		/// <summary>
		/// Dohvaca zgradu za etazu
		/// </summary>
		/// <param name="partitionSpace">etaza</param>
		/// <returns>zgrada</returns>
		public Building GetByPartitionSpace(IPartitionSpace partitionSpace) {
			var hql = @"select b from Building b
				join b.LandRegistry lr
				join lr.PartitionSpaces ps
				where ps = :partitionSpace";
			return Session.CreateQuery(hql)
				.SetEntity("partitionSpace", partitionSpace)
				.UniqueResult<Building>();
		}

		/// <summary>
		/// Dohvaca zgradu za zemljisnu knjigu
		/// </summary>
		/// <param name="landRegistry">zemljisna knjiga</param>
		/// <returns>zgrada</returns>
		public Building GetByLandRegistry(LandRegistry landRegistry) {
			return Session.QueryOver<Building>()
				.Where(b => b.LandRegistry == landRegistry)
				.SingleOrDefault();
		}

		#endregion

	}
}
