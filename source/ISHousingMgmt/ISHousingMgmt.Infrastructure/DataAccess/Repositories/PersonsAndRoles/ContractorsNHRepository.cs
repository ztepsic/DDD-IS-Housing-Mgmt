using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.PersonsAndRoles {
	public class ContractorsNHRepository : NHibernateCrudRepository<Contractor>, IContractorsRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public ContractorsNHRepository() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public ContractorsNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca sve izvodace radova koji ne suraduju sa upraviteljem
		/// </summary>
		/// <param name="buildingManagerId">identifikator upravitelja</param>
		/// <returns></returns>
		public IList<Contractor> GetNonBuildingMgmtContractors(int buildingManagerId) {
			var contractors = GetAll();
			var buildingManagerContractors = Session.Get<BuildingManager>(buildingManagerId).Contractors;

			return contractors.Except(buildingManagerContractors).ToList();

		}

		/// <summary>
		/// Dohvaca izvodace radova za upravitelja i zadanu vrstu popravka
		/// </summary>
		/// <param name="repairService">vrsta popravka</param>
		/// <param name="buildingManager">upravitelj</param>
		/// <returns>izvodaci radova koji obavaljaju zadanu vrstu popravka i u partnerstvu su sa zadanim upraviteljem</returns>
		public IList<Contractor> GetContractorsByRepairService(RepairService repairService, BuildingManager buildingManager) {
			var hql = @"select c from BuildingManager bm
				join bm.Contractors c
				join c.RepairServices rs
				where bm = :buildingManager and
				rs = :repairService";

			return Session.CreateQuery(hql)
				.SetEntity("buildingManager", buildingManager)
				.SetEntity("repairService", repairService)
				.List<Contractor>();

		}

		/// <summary>
		/// Dohvaca izvodaca na temelju osobe
		/// </summary>
		/// <param name="legalPerson">osoba</param>
		/// <returns>izvodac radova</returns>
		public Contractor GetContractorByPerson(LegalPerson legalPerson) {
			return Session.QueryOver<Contractor>()
				.Where(c => c.LegalPerson == legalPerson)
				.SingleOrDefault();
		}

		#endregion

	}
}
