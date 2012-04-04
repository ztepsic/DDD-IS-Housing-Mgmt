using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using ISHousingMgmt.Infrastructure.Services;
using ISHousingMgmt.Web.Exstensions;
using ISHousingMgmt.Web.Models;
using ISHousingMgmt.Web.Models.BuildingMaintenance;
using ISHousingMgmt.Web.Models.Contractor;
using ISHousingMgmt.Web.Models.PersonsAndRoles;
using IndexModel = ISHousingMgmt.Web.Models.Contractor.IndexModel;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Web.Models.Finances;

namespace ISHousingMgmt.Web.Controllers {
	[Authorize]
	public class ContractorController : Controller {

		#region Members

		private readonly IContractorsRepository contractorsRepository;
		private readonly IMaintenancesRepository maintenancesRepository;
		private readonly IPersonsRepository personsRepository;
		private readonly IBillsRepository billsRepository;
		private readonly IRepairServicesRepository repairServicesRepository;
		private readonly IEmailNotifier emailNotifier;

		#endregion

		#region Constructors and Init

		public ContractorController(IContractorsRepository contractorsRepository, IMaintenancesRepository maintenancesRepository,
			IPersonsRepository personsRepository, IBillsRepository billsRepository, IRepairServicesRepository repairServicesRepository,
			IEmailNotifier emailNotifier) {
			this.contractorsRepository = contractorsRepository;
			this.maintenancesRepository = maintenancesRepository;
			this.personsRepository = personsRepository;
			this.billsRepository = billsRepository;
			this.repairServicesRepository = repairServicesRepository;
			this.emailNotifier = emailNotifier;
		}

		#endregion

		#region Actions

		[NHibernateTransaction]
		public ActionResult Index() {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var maintenances = maintenancesRepository.GetAllMaintenancesByContractor(legalPerson);
			var activeMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.InProgress);
			var inConfirmationMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.InConfirmation);
			var completedMaintenances = maintenances.Where(m => m.StatusOfMaintenance == StatusOfMaintenance.Completed);

			var model = new IndexModel() {
				ActiveMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(activeMaintenances),
				InConfirmationMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(inConfirmationMaintenances),
				CompletedMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(completedMaintenances),
				Roles = Roles.GetRolesForUser()
			};

			return View(model);
		}

		[NHibernateTransaction]
		public ActionResult Maintenance(int id) {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var maintenance = maintenancesRepository.GetById(id);
			if (maintenance == null) { return HttpNotFound(); }

			var legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			if(maintenance.Contractor.Oib != legalPerson.Oib) { return new HttpStatusCodeResult(405); }

			var model = new DetailsModel() {
				Maintenance = Mapper.Map<Maintenance, MaintenanceDetailModel>(maintenance),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = "contractor"
			};

			return View("~/Views/Maintenance/Details.cshtml" ,model);


		}

		[NHibernateTransaction]
		public ActionResult Fix(int id) {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var maintenance = maintenancesRepository.GetById(id);
			if (maintenance == null) { return HttpNotFound(); }

			var legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			if ((maintenance.Contractor.Oib != legalPerson.Oib) || maintenance.StatusOfMaintenance == StatusOfMaintenance.Completed) {  return new HttpStatusCodeResult(405); }

			var model = new FixModel {
				Maintenance = Mapper.Map<Maintenance, MaintenanceDetailModel>(maintenance),
				Roles = Roles.GetRolesForUser()
			};

			return View(model);
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult Fix(int id, FixModel fixModel) {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var maintenance = maintenancesRepository.GetById(id);
			if (maintenance == null) { return HttpNotFound(); }

			var legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			if (maintenance.Contractor.Oib != legalPerson.Oib) { return new HttpStatusCodeResult(405); }

			if(string.IsNullOrEmpty(fixModel.Maintenance.ContractorsConclusion)) {
				ModelState.AddModelError("Maintenance.ContractorsConclusion", "Zaključak izvođača radova ne može biti prazan.");
			}

			if(ModelState.IsValid) {
				maintenance.ContractorsConclusion = fixModel.Maintenance.ContractorsConclusion;
				maintenance.SetWorkAsDone();
				var url = Url.Action("details", "maintenance", new {Id = maintenance.Id}, "http");
				emailNotifier.NotifyOfMaintenanceCompletition(maintenance, url);
				return RedirectToAction("maintenance", new {Id = maintenance.Id});
			}

			fixModel.Roles = Roles.GetRolesForUser();
			fixModel.Maintenance = Mapper.Map<Maintenance, MaintenanceDetailModel>(maintenance);

			return View(fixModel);

		}

		[NHibernateTransaction]
		public ActionResult Bills() {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			if (legalPerson == null) { return new HttpUnauthorizedResult(); }

			var bills = billsRepository.GetBillsFrom(legalPerson);
			var unpaidBills = bills.Where(b => b.IsPaid == false);
			var paidBills = bills.Where(b => b.IsPaid== true);

			var model = new BillsModel {
				PaidBills = Mapper.Map<IEnumerable<Bill>, IEnumerable<Models.Finances.BillModel>>(paidBills),
				UnpaidBills = Mapper.Map<IEnumerable<Bill>, IEnumerable<Models.Finances.BillModel>>(unpaidBills),
				Roles = Roles.GetRolesForUser()
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult IssueBill() {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			LegalPerson legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var maintenances = maintenancesRepository.GetAllMaintenancesWithNoBillByContractor(legalPerson);
			var mappedMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(maintenances);

			var model = new IssueBillModel {
				UnbilledMaintances = new SelectList(mappedMaintenances, "Id", "MaintenanceRequest.Subject"),
				Roles = Roles.GetRolesForUser()
			};

			return View(model);
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult IssueBill(IssueBillModel model) {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			LegalPerson legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			
			if(ModelState.IsValid) {
				var maintenance = maintenancesRepository.GetById(model.UnbilledMaintance);
				if(maintenance != null) {
					try {
						var bill = new Bill(legalPerson, maintenance.Building.Reserve, model.PaymentDescription, 23) {
							ReferenceNumber = string.Format("{0}-{1}-{2}", maintenance.Building.Id, maintenance.Id, DateTime.Now.ToString("yyyy-MM-dd"))
						};
						foreach (var billItemModel in model.BillItems) {
							var billItem = new BillItem(billItemModel.Quantity, billItemModel.Price, billItemModel.Description);
							bill.AddBillItem(billItem);	
						}

						maintenance.SetBill(bill);
						billsRepository.SaveOrUpdate(bill);

						var url = Url.Action("bill", "contractor", new {Id = bill.Id}, "http");
						emailNotifier.NotifyOfBilling(bill, url);
						return RedirectToAction("bills");

					} catch(BusinessRulesException ex) {
						ex.CopyTo(ModelState);
					}

				} else {
					ModelState.AddModelError("UnbilledMaintance", "Odabrali ste ne postojeće održavanje/popravak.");	
				}
			}
			

			var maintenances = maintenancesRepository.GetAllMaintenancesWithNoBillByContractor(legalPerson);
			var mappedMaintenances = Mapper.Map<IEnumerable<Maintenance>, IEnumerable<MaintenanceDetailModel>>(maintenances);
			model.UnbilledMaintances = new SelectList(mappedMaintenances, "Id", "MaintenanceRequest.Subject");
			model.Roles = Roles.GetRolesForUser();

			return View(model);
		}

		[NHibernateTransaction]
		public ActionResult Bill(int id) {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var bill = billsRepository.GetById(id);

			if (bill == null) { return HttpNotFound(); }			

			var model = new Models.Contractor.BillModel {
				Bill = Mapper.Map<Bill, Models.Finances.BillModel>(bill),
				Roles = Roles.GetRolesForUser()
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult PrintBill(int id) {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var bill = billsRepository.GetById(id);

			if (bill == null) { return HttpNotFound(); }

			var model = new Models.Contractor.BillModel {
				Bill = Mapper.Map<Bill, Models.Finances.BillModel>(bill),
				Roles = Roles.GetRolesForUser()
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult RepairServices() {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var contractor = contractorsRepository.GetContractorByPerson(legalPerson);
			if (contractor == null) { return new HttpUnauthorizedResult(); }

			var services = repairServicesRepository.GetAll();
			var avaliableServices = services.Except(contractor.RepairServices);

			var model = new RepairServicesModel {
				Contractor = Mapper.Map<Contractor, ContractorModel>(contractor),
				AvaliableServices = Mapper.Map<IEnumerable<RepairService>, IEnumerable<RepairServiceModel>>(avaliableServices),
				Roles = Roles.GetRolesForUser()
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult AddRepairService(int id) {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var contractor = contractorsRepository.GetContractorByPerson(legalPerson);
			if (contractor == null) { return new HttpUnauthorizedResult(); }

			var repairService = repairServicesRepository.GetById(id);
			if (repairService == null) { return HttpNotFound(); }

			if(!contractor.RepairServices.Contains(repairService)) {
				contractor.AddRepairService(repairService);
			}

			return RedirectToAction("repairservices");

		}

		[NHibernateTransaction]
		public ActionResult RemoveRepairService(int id) {
			if (!User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var contractor = contractorsRepository.GetContractorByPerson(legalPerson);
			if (contractor == null) { return new HttpUnauthorizedResult(); }

			var repairService = repairServicesRepository.GetById(id);
			if (repairService == null) { return HttpNotFound(); }

			if (contractor.RepairServices.Contains(repairService)) {
				contractor.RemoveRepairService(repairService);
			}

			return RedirectToAction("repairservices");

		}

		#endregion

	}
}
