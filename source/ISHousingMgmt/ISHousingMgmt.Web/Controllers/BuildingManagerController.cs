using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using ISHousingMgmt.Infrastructure.Services;
using ISHousingMgmt.Web.Exstensions;
using ISHousingMgmt.Web.Helpers;
using ISHousingMgmt.Web.Models;
using ISHousingMgmt.Web.Models.BuildingManagement;
using ISHousingMgmt.Web.Models.BuildingManager;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Controllers {
	[Authorize]
	public class BuildingManagerController : Controller {

		#region Members

		private readonly IBuildingManagersRepository buildingManagersRepository;
		private readonly IPersonsRepository personsRepository;
		private readonly IContractorsRepository contractorsRepository;
		private readonly IBillsRepository billsRepository;
		private readonly IBuildingsRepository buildingsRepository;
		private readonly IEmailNotifier emailNotifier;

		#endregion

		#region Constructors and Init

		public BuildingManagerController(IBuildingManagersRepository buildingManagersRepository, IPersonsRepository personsRepository,
			IContractorsRepository contractorsRepository, IBillsRepository billsRepository, IBuildingsRepository buildingsRepository,
			IEmailNotifier emailNotifier) {
			this.buildingManagersRepository = buildingManagersRepository;
			this.personsRepository = personsRepository;
			this.contractorsRepository = contractorsRepository;
			this.billsRepository = billsRepository;
			this.buildingsRepository = buildingsRepository;
			this.emailNotifier = emailNotifier;
		}

		#endregion

		#region Actions

		[NHibernateTransaction]
		public ActionResult Contractors() {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			LegalPerson legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var buildingManager = buildingManagersRepository.GetByLegalPerson(legalPerson);
			var constractors = buildingManager.Contractors;
			var model = new BuildingMgmtContractorsModel() {
				Contractors = Mapper.Map<ICollection<Contractor>, ICollection<ContractorModel>>(constractors),
				Roles = Roles.GetRolesForUser()
			};

			return View(model);
		}

		[NHibernateTransaction]
		public ActionResult AddContractors() {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			LegalPerson legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var buildingManager = buildingManagersRepository.GetByLegalPerson(legalPerson);
			var constractors = contractorsRepository.GetNonBuildingMgmtContractors(buildingManager.Id);
			var model = new AddContractorsModel() {
				Contractors = Mapper.Map<ICollection<Contractor>, ICollection<ContractorModel>>(constractors),
				Roles = Roles.GetRolesForUser()
			};

			return View(model);
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult AddContractors(AddContractorsModel addContractorsModel) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			LegalPerson legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var buildingManager = buildingManagersRepository.GetByLegalPerson(legalPerson);
			if(ModelState.IsValid) {
				foreach (var selectedContractorId in addContractorsModel.SelectedContractors) {
					var contractor = contractorsRepository.GetById(selectedContractorId);
					buildingManager.AddContractor(contractor);
				}

				return RedirectToAction("contractors");
				
			}

			addContractorsModel.Roles = Roles.GetRolesForUser();

			var constractors = contractorsRepository.GetNonBuildingMgmtContractors(buildingManager.Id);
			addContractorsModel.Contractors = Mapper.Map<ICollection<Contractor>, ICollection<ContractorModel>>(constractors);

			return View(addContractorsModel);

		}

		[NHibernateTransaction]
		public ActionResult RemoveContractor(int id) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			LegalPerson legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var buildingManager = buildingManagersRepository.GetByLegalPerson(legalPerson);
			var contractor = contractorsRepository.GetById(id);
			if(contractor != null) {
				buildingManager.RemoveContractor(contractor);	
			}

			return RedirectToAction("contractors");

		}

		[NHibernateTransaction]
		public ActionResult Bills(int id) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			if (legalPerson == null) { return new HttpUnauthorizedResult(); }

			IList<Bill> bills = null;
			string role = string.Empty;
			LinksModel links = null;
			if(id > 0) {
				var building = buildingsRepository.GetById(id);
				
				bills = billsRepository.GetBillsFrom(legalPerson, building.Reserve);
				role = "buildingmanager";
				links = new LinksModel{
					Id = building.Id,
					Links = NavLinksGenerator.GetManagerLinks(building, "Upraviteljevi računi")
				};
			} else {
				bills = billsRepository.GetBillsFrom(legalPerson);
			}
			
			var unpaidBills = bills.Where(b => b.IsPaid == false);
			var paidBills = bills.Where(b => b.IsPaid== true);

			var model = new Models.BuildingManager.BillsModel {
				PaidBills = Mapper.Map<IEnumerable<Bill>, IEnumerable<Models.Finances.BillModel>>(paidBills),
				UnpaidBills = Mapper.Map<IEnumerable<Bill>, IEnumerable<Models.Finances.BillModel>>(unpaidBills),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links,
				BuildingId = id
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult IssueBill(int id) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			LegalPerson legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var building = buildingsRepository.GetById(id);
			if(building == null) {
				return HttpNotFound();
			}

			if(!building.BuildingManager.LegalPerson.Equals(legalPerson)) {
				return new HttpUnauthorizedResult();
			}

			var model = new IssueBillModel {
				Building = Mapper.Map<Building, BuildingListModel>(building),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = "buildingmanager",
				Links = new LinksModel{ Id = building.Id, Links = NavLinksGenerator.GetManagerLinks(building, "Upraviteljevi računi")}
			};

			return View(model);
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult IssueBill(int id, IssueBillModel model) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			LegalPerson legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
			var building = buildingsRepository.GetById(id);
			if (building == null) {
				return HttpNotFound();
			}

			if (!building.BuildingManager.LegalPerson.Equals(legalPerson)) {
				return new HttpUnauthorizedResult();
			}

			if (ModelState.IsValid) {
				try {
					var bill = new Bill(legalPerson, building.Reserve, model.PaymentDescription, 23) {
						ReferenceNumber = string.Format("{0}-{1}-{2}", building.Id, building.BuildingManager.Id, DateTime.Now.ToString("yyyy-MM-dd"))
					};

					foreach (var billItemModel in model.BillItems) {
						var billItem = new BillItem(billItemModel.Quantity, billItemModel.Price, billItemModel.Description);
						bill.AddBillItem(billItem);
					}

					billsRepository.SaveOrUpdate(bill);
					var url = Url.Action("bill", "buildingmanager", new {Id = bill.Id}, "http");
					emailNotifier.NotifyOfBilling(bill, url);
					return RedirectToAction("bills");

				} catch (BusinessRulesException ex) {
					ex.CopyTo(ModelState);
				}
			}

			model.Roles = Roles.GetRolesForUser();
			model.Building = Mapper.Map<Building, BuildingListModel>(building);

			return View(model);
		}

		[NHibernateTransaction]
		public ActionResult Bill(int id) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var bill = billsRepository.GetById(id);

			if (bill == null) { return HttpNotFound(); }

			var model = new BillModel {
				Bill = Mapper.Map<Bill, Models.Finances.BillModel>(bill),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = "buildingmanager",
				Links = new LinksModel{ Id = bill.Reserve.Building.Id, Links = NavLinksGenerator.GetManagerLinks(bill.Reserve.Building, "Upraviteljevi računi")}
			};

			return View(model);

		}
		

		#endregion

	}
}
