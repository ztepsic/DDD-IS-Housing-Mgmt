using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using ISHousingMgmt.Web.Models;
using ISHousingMgmt.Web.Models.BuildingMaintenance;
using ISHousingMgmt.Web.Models.BuildingManagement;
using ISHousingMgmt.Web.Models.Legislature;
using ISHousingMgmt.Web.Models.Owner;
using IndexModel = ISHousingMgmt.Web.Models.Owner.IndexModel;

namespace ISHousingMgmt.Web.Controllers {
	[Authorize]
	public class DashboardController : Controller {

		#region Members

		private readonly IBuildingsRepository buildingsRepository;
		private readonly IPartitionSpacesRepository partitionSpacesRepository;
		private readonly IPersonsRepository personsRepository;
		private readonly IMaintenancesRepository maintenancesRepository;
		

		#endregion

		#region Constructors and Init

		public DashboardController(IBuildingsRepository buildingsRepository, IPartitionSpacesRepository partitionSpacesRepository,
			IPersonsRepository personsRepository, IMaintenancesRepository maintenancesRepository) {
			this.buildingsRepository = buildingsRepository;
			this.partitionSpacesRepository = partitionSpacesRepository;
			this.personsRepository = personsRepository;
			this.maintenancesRepository = maintenancesRepository;
		}

		#endregion

		#region Actions

		[NHibernateTransaction]
		public ActionResult Index() {

			var person = personsRepository.GetPersonByUsername(User.Identity.Name);
			var model = new Models.Dashboard.IndexModel() { Roles = Roles.GetRolesForUser() };

			if (User.IsInRole("contractor")) {
				var maintenances = maintenancesRepository.GetAllMaintenancesByContractor(person as LegalPerson, 5);
				model.Maintenances = Mapper.Map<IList<Maintenance>, IList<MaintenanceDetailModel>>(maintenances);
			}

			if(User.IsInRole("owner")) {
				var partitionSpaces = partitionSpacesRepository.GetPartitionSpaces(person, 5);
				var apartments = new List<ApartmentListModel>();
				foreach (var partitionSpace in partitionSpaces) {
					var building = buildingsRepository.GetByPartitionSpace(partitionSpace);
					var apartment = new ApartmentListModel {
						PartitionSpace = Mapper.Map<IPartitionSpace, PartitionSpaceListModel>(partitionSpace),
						Building = Mapper.Map<Building, BuildingListModel>(building)
					};
					apartments.Add(apartment);
				}

				model.Apartments = apartments;
			}

			if(User.IsInRole("representative")) {
				IList<Building> representativeBuildings = buildingsRepository.GetBuildingsByRepresentative(person, 5);
				var representativeBuildingListModel = Mapper.Map<IList<Building>, IList<BuildingListModel>>(representativeBuildings);
				model.RepresentativeBuildings = representativeBuildingListModel;

			}


			if(User.IsInRole("buildingmanager")) {
				IList<Building> managerBuildings = buildingsRepository.GetBuildingsByManager(person as LegalPerson, 5);
				var representativeBuildingListModel = Mapper.Map<IList<Building>, IList<BuildingListModel>>(managerBuildings);
				model.ManagerBuildings = representativeBuildingListModel;
			}



			return View(model);

		}

		#endregion
	}
}
