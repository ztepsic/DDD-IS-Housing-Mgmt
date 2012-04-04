using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using ISHousingMgmt.Infrastructure.Services;
using ISHousingMgmt.Web.Exstensions;
using ISHousingMgmt.Web.Helpers;
using ISHousingMgmt.Web.Models;
using ISHousingMgmt.Web.Models.BuildingMaintenance;
using ISHousingMgmt.Web.Models.BuildingManagement;
using ISHousingMgmt.Web.Models.PersonsAndRoles;
using CreateModel = ISHousingMgmt.Web.Models.BuildingMaintenance.CreateModel;
using IndexModel = ISHousingMgmt.Web.Models.BuildingMaintenance.IndexModel;

namespace ISHousingMgmt.Web.Controllers {
	[Authorize]
	public class MaintenanceController : Controller {

		#region Members

		private readonly IMaintenancesRepository maintenancesRepository;
		private readonly IBuildingsRepository buildingsRepository;
		private readonly IPersonsRepository personsRepository;
		private readonly IRepairServicesRepository repairServicesRepository;
		private readonly IContractorsRepository contractorsRepository;
		private readonly IEmailNotifier emailNotifier;
		

		#endregion

		#region Constructors and Init

		public MaintenanceController(IMaintenancesRepository maintenancesRepository, IBuildingsRepository buildingsRepository,
			IPersonsRepository personsRepository, IRepairServicesRepository repairServicesRepository,
			IContractorsRepository contractorsRepository, IEmailNotifier emailNotifier) {
			this.maintenancesRepository = maintenancesRepository;
			this.buildingsRepository = buildingsRepository;
			this.personsRepository = personsRepository;
			this.repairServicesRepository = repairServicesRepository;
			this.contractorsRepository = contractorsRepository;
			this.emailNotifier = emailNotifier;
		}

		#endregion

		#region Actions

		[NHibernateTransaction]
		public ActionResult Index(int id) {
			if (User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);
			if(building == null) {
				return HttpNotFound();
			}

			var maintenances = maintenancesRepository.GetAllMaintenancesByBuilding(building.Id);
			var newMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.NotStarted);
			var activeMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.InProgress);
			var inConfirmationMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.InConfirmation);
			var completedMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.Completed);

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Kvarovi");
			} else if (role == "buildingmanager") {
				links.Links = NavLinksGenerator.GetManagerLinks(building, "Kvarovi");
			} else if (role == "owner") {
				links.Links = NavLinksGenerator.GetOwnerLinks(building, "Kvarovi");
			}

			var model = new IndexModel() {
				NewMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(newMaintenances),
				ActiveMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(activeMaintenances),
				InConfirmationMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(inConfirmationMaintenances),
				CompletedMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(completedMaintenances),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links,
				Building = Mapper.Map<Building, BuildingListModel>(building)
			};

			return View(model);
		}

		[NHibernateTransaction]
		public ActionResult My(int id) {
			var building = buildingsRepository.GetById(id);
			if (building == null) {
				return HttpNotFound();
			}

			var submitter = personsRepository.GetPersonByUsername(User.Identity.Name);
			var maintenances = maintenancesRepository.GetAllMaintenancesBySubmitter(submitter, building.Id);
			var newMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.NotStarted);
			var activeMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.InProgress);
			var inConfirmationMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.InConfirmation);
			var completedMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.Completed);

			
			LinksModel links = new LinksModel();
			if(Session["lastPageId"] != null) {
				links.Id = (int) Session["lastPageId"];
				links.Links = NavLinksGenerator.GetOwnerLinks(building, "Moji kvarovi");
			}

			var model = new IndexModel() {
				NewMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(newMaintenances),
				ActiveMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(activeMaintenances),
				InConfirmationMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(inConfirmationMaintenances),
				CompletedMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(completedMaintenances),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = "owner",
				Links = links,
				Building = Mapper.Map<Building, BuildingListModel>(building)
			};

			return View("Index", model);
		}


		[NHibernateTransaction]
		public ActionResult Create(int id) {
			var building = buildingsRepository.GetById(id);
			if(building == null) {
				return HttpNotFound();
			}

			var repairServicesModel = Mapper.Map<IEnumerable<RepairService>, IEnumerable<RepairServiceModel>>(repairServicesRepository.GetAll());

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Kvarovi");
			} else if (role == "buildingmanager") {
				links.Links = NavLinksGenerator.GetManagerLinks(building, "Kvarovi");
			} else if (role == "owner") {
				links.Links = NavLinksGenerator.GetOwnerLinks(building, "Kvarovi");
			}

			var model = new Models.BuildingMaintenance.CreateModel() {
				RepairServices = new SelectList(repairServicesModel, "Id", "Name"),
				Urgencies = new SelectList(new[] {
					new { Id = (int)Urgency.Low, Name = "Niska" },
					new { Id = (int)Urgency.Normal, Name = "Normalna"},
					new { Id = (int)Urgency.High, Name = "Visoka"}
				}, "Id", "Name"),
				MaintenanceRequest = new MaintenanceRequestModel(),
				Roles = Roles.GetRolesForUser(),
				Links = links,
				CurrentRole = role,
				Building = building.Address.ToString()
			};

			return View(model);
		}

		
		[NHibernateTransaction]
		[HttpPost]
		public ActionResult Create(int id, CreateModel createModel) {
			Person submitter = null;
			if(ModelState.IsValid) {
				submitter = personsRepository.GetPersonByUsername(User.Identity.Name);

				MaintenanceRequest maintenanceRequest = new MaintenanceRequest(
					submitter,
					createModel.MaintenanceRequest.Subject,
					createModel.MaintenanceRequest.Description,
					createModel.MaintenanceRequest.Location);

				RepairService repairService = repairServicesRepository.GetById(createModel.RepairService);
				Building building = buildingsRepository.GetById(id);

				Maintenance maintenance = new Maintenance(
					maintenanceRequest,
					createModel.Urgency,
					repairService,
					building);

				maintenancesRepository.SaveOrUpdate(maintenance);
				var url = Url.Action("details", "maintenance", new {Id = maintenance.Id}, "http");
				emailNotifier.NotifyOfMaintenanceCreation(maintenance, url);
				return RedirectToAction("index", new { Id = id });

			} 

			IList<Building> buildings = null;
			submitter = personsRepository.GetPersonByUsername(User.Identity.Name);
			if (User.IsInRole("owner")) {
				buildings = buildingsRepository.GetBuildingsByOwner(submitter);
			} else if (User.IsInRole("buildingmanager")) {
				buildings = buildingsRepository.GetBuildingsByManager(submitter as LegalPerson);
			} else {
				return new HttpUnauthorizedResult();
			}

			var buildingsModel = Mapper.Map<IList<Building>, IList<BuildingListModel>>(buildings);
			var repairServicesModel = Mapper.Map<IEnumerable<RepairService>, IEnumerable<RepairServiceModel>>(repairServicesRepository.GetAll());
			createModel.RepairServices = new SelectList(repairServicesModel, "Id", "Name");
			createModel.Urgencies = new SelectList(new[] {
				new {Id = (int) Urgency.Low, Name = "Niska"},
				new {Id = (int) Urgency.Normal, Name = "Normalna"},
				new {Id = (int) Urgency.High, Name = "Visoka"}
				}, "Id", "Name");
			createModel.Roles = Roles.GetRolesForUser();

			return View(createModel);
			
		}

		[Authorize]
		[NHibernateTransaction]
		public ActionResult Details(int id) {
			var maintenance = maintenancesRepository.GetById(id);

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				links.Links = NavLinksGenerator.GetRepresentativeLinks(maintenance.Building, "Kvarovi");
			} else if (role == "buildingmanager") {
				links.Links = NavLinksGenerator.GetManagerLinks(maintenance.Building, "Kvarovi");
			} else if (role == "owner") {
				links.Links = NavLinksGenerator.GetOwnerLinks(maintenance.Building, "Kvarovi");
			}

			var model = new DetailsModel() {
				Maintenance = Mapper.Map<Maintenance, MaintenanceDetailModel>(maintenance),
				Roles = Roles.GetRolesForUser(),
				Links = links,
				CurrentRole = role
			};

			return View(model);
		}

		[NHibernateTransaction]
		public ActionResult DelegateRepair(int id) {
			if(!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var maintenance = maintenancesRepository.GetById(id);
			if (maintenance == null) { return HttpNotFound(); }

			BuildingManager buildingManager = maintenance.BuildingManager;
			var contractors = contractorsRepository.GetContractorsByRepairService(maintenance.ServiceType, buildingManager);

			var model = new DelegateRepairModel() {
				Maintenance = Mapper.Map<Maintenance, MaintenanceDetailModel>(maintenance),
				Contractors = Mapper.Map<IList<Contractor>, IList<ContractorModel>>(contractors),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = "buildingmanager",
				Links = new LinksModel() { Id = maintenance.Building.Id, Links = NavLinksGenerator.GetManagerLinks(maintenance.Building, "Kvarovi")}
			};

			return View(model);
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult DelegateRepair(int id, DelegateRepairModel delegateRepairModel) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var maintenance = maintenancesRepository.GetById(id);
			if (maintenance == null) { return HttpNotFound(); }
			if(maintenance.StatusOfMaintenance == StatusOfMaintenance.InProgress) {
				return new HttpStatusCodeResult(405);
			}

			BuildingManager buildingManager = maintenance.BuildingManager;

			if(ModelState.IsValid) {
				var contractor = contractorsRepository.GetById(delegateRepairModel.Contractor);
				if(contractor != null) {
					try {
						maintenance.SetContractor(contractor);
						if (!string.IsNullOrEmpty(delegateRepairModel.Maintenance.Instructions)) {
							maintenance.Instructions = delegateRepairModel.Maintenance.Instructions;
						}

						var url = Url.Action("maintenance", "contractor", new {Id = maintenance.Id}, "http");
						emailNotifier.NotifyOfMaintenanceDelegation(maintenance, url);
						return RedirectToAction("details", new { Id = maintenance.Id });

					} catch (BusinessRulesException ex) {
						ex.CopyTo(ModelState);
					}	
				} else {
					ModelState.AddModelError("Contractor", "Niste odabrali izvođača radova.");
				}
				
			}

			
			var contractors = contractorsRepository.GetContractorsByRepairService(maintenance.ServiceType, buildingManager);
			delegateRepairModel.Maintenance = Mapper.Map<Maintenance, MaintenanceDetailModel>(maintenance);
			delegateRepairModel.Roles = Roles.GetRolesForUser();
			delegateRepairModel.CurrentRole = "buildingmanager";
			delegateRepairModel.Links = new LinksModel() {
				Id = maintenance.Building.Id,
				Links = NavLinksGenerator.GetManagerLinks(maintenance.Building, "Kvarovi")
			};
			delegateRepairModel.Contractors = Mapper.Map<IList<Contractor>, IList<ContractorModel>>(contractors);

			return View(delegateRepairModel);

		}

		[NHibernateTransaction]
		public ActionResult Confirm(int id) {
			if (!User.IsInRole("representative")) { return new HttpUnauthorizedResult(); }

			var maintenance = maintenancesRepository.GetById(id);
			if (maintenance == null) { return HttpNotFound(); }

			Person person = personsRepository.GetPersonByUsername(User.Identity.Name);
			if(person.Oib != maintenance.Representative.Oib) {
				return new HttpUnauthorizedResult();
			}

			var model = new ConfirmModel {
				Maintenance = Mapper.Map<Maintenance, MaintenanceDetailModel>(maintenance),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = "representative",
				Links = new LinksModel() { Id = maintenance.Building.Id, Links = NavLinksGenerator.GetRepresentativeLinks(maintenance.Building, "Kvarovi") }
			};

			return View(model);
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult Confirm(int id, ConfirmModel confirmModel) {
			if (!User.IsInRole("representative")) { return new HttpUnauthorizedResult(); }

			var maintenance = maintenancesRepository.GetById(id);
			if (maintenance == null) { return HttpNotFound(); }

			Person person = personsRepository.GetPersonByUsername(User.Identity.Name);
			if (person.Oib != maintenance.Representative.Oib) {
				return new HttpUnauthorizedResult();
			}

			if(confirmModel.IsConfirmed == false && string.IsNullOrEmpty(confirmModel.Remark)) {
				ModelState.AddModelError("Remark", "Ukoliko obavljanje posla nije pozitivno potvrđeno, obavezno je potrebno upisati napomene izvođačima radova.");
			}

			if(ModelState.IsValid) {
				if(confirmModel.IsConfirmed) {
					if(!string.IsNullOrEmpty(confirmModel.Remark)) {
						maintenance.SetMaitenanceCompleted(confirmModel.Remark);
					} else {
						maintenance.SetMaitenanceCompleted();	
					}
					
				} else {
					maintenance.SetWorkAsNotDone(confirmModel.Remark);
				}

				var submitterUrl = Url.Action("details", "maintenance", new { Id = maintenance.Id }, "http");
				var contractorUrl = Url.Action("maintenance", "contractor", new {Id = maintenance.Id}, "http");
				emailNotifier.NotifyOfMaintenanceAcception(maintenance, submitterUrl, contractorUrl);
				return RedirectToAction("details", new {Id = maintenance.Id});
			}

			confirmModel.Maintenance = Mapper.Map<Maintenance, MaintenanceDetailModel>(maintenance);
			confirmModel.Roles = Roles.GetRolesForUser();
			confirmModel.CurrentRole = "representative";
			confirmModel.Links = new LinksModel() {
				Id = maintenance.Building.Id,
				Links = NavLinksGenerator.GetRepresentativeLinks(maintenance.Building, "Kvarovi")
			};

			return View(confirmModel);

		}

		#endregion

	}
}
