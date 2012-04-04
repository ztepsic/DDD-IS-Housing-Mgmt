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
using ISHousingMgmt.Web.Helpers;
using ISHousingMgmt.Web.Models;
using ISHousingMgmt.Web.Models.Finances;

namespace ISHousingMgmt.Web.Controllers {
	[Authorize]
	public class FinancesController : Controller {

		#region Members

		private readonly IBillsRepository billsRepository;
		private readonly IReservesRepository reservesRepository;
		private readonly IPersonsRepository personsRepository;
		private readonly IBuildingsRepository buildingsRepository;
		private readonly IEmailNotifier emailNotifier;

		#endregion

		#region Constructors and Init

		public FinancesController(IBillsRepository billsRepository, IReservesRepository reservesRepository,
			IPersonsRepository personsRepository, IBuildingsRepository buildingsRepository,
			IEmailNotifier emailNotifier) {
			this.billsRepository = billsRepository;
			this.reservesRepository = reservesRepository;
			this.personsRepository = personsRepository;
			this.buildingsRepository = buildingsRepository;
			this.emailNotifier = emailNotifier;
		}

		#endregion

		#region Actions

		[NHibernateTransaction]
		public ActionResult Reserve(int id) {
			if (User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);
			if (building == null) { return HttpNotFound(); }

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			var person = personsRepository.GetPersonByUsername(User.Identity.Name);
			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				if(!building.RepresentativeOfPartOwners.Equals(person)) {
					return new HttpUnauthorizedResult();	
				}

				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Pričuva");
			} else if (role == "buildingmanager") {
				if (!building.BuildingManager.LegalPerson.Equals(person as LegalPerson)) {
					return new HttpUnauthorizedResult();
				}

				links.Links = NavLinksGenerator.GetManagerLinks(building, "Pričuva");
			} else if( role == "owner") {
				if (!building.GetOwners().Any(o => o.Oib == person.Oib)) {
					return new HttpUnauthorizedResult();
				}

				links.Links = NavLinksGenerator.GetOwnerLinks(building, "Pričuva");
			}

			var reserve = building.Reserve;
			
			// pokusaj izdati racune
			if(ReserveBilling.IssueMonthlyReserveBills(reserve, billsRepository)) {
				var bills = billsRepository.GetIssuedReserveBillsFor(reserve, DateTime.Now.Month, DateTime.Now.Year);
				foreach (var bill in bills) {
					var url = Url.Action("bill", "owner", new {Id = bill.Id}, "http");
					emailNotifier.NotifyOfBilling(bill, url);
				}
			}

			var model = new ReserveMModel {
				Reserve = Mapper.Map<Reserve, ReserveModel>(reserve),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult Bill(int id) {
			if (User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var bill = billsRepository.GetById(id);
			if (bill == null) { return HttpNotFound(); }

			var building = bill.Reserve.Building;

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			var person = personsRepository.GetPersonByUsername(User.Identity.Name);
			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				if (!building.RepresentativeOfPartOwners.Equals(person)) {
					return new HttpUnauthorizedResult();
				}

				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Pričuva");
			} else if (role == "buildingmanager") {
				if (!building.BuildingManager.LegalPerson.Equals(person as LegalPerson)) {
					return new HttpUnauthorizedResult();
				}

				links.Links = NavLinksGenerator.GetManagerLinks(building, "Pričuva");
			} else if (role == "owner") {
				if (!building.GetOwners().Any(o => o.Oib == person.Oib)) {
					return new HttpUnauthorizedResult();
				}

				links.Links = NavLinksGenerator.GetOwnerLinks(building, "Pričuva");
			}

			var model = new BillMethodModel {
				Bill = Mapper.Map<Bill, BillModel>(bill),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult PrintBill(int id) {
			if (User.IsInRole("contractor")) { return new HttpUnauthorizedResult(); }

			var bill = billsRepository.GetById(id);
			if (bill == null) { return HttpNotFound(); }

			var building = bill.Reserve.Building;

			var person = personsRepository.GetPersonByUsername(User.Identity.Name);
			if (!(building.RepresentativeOfPartOwners.Equals(person) ||
				building.BuildingManager.LegalPerson.Equals(person as LegalPerson) ||
				building.GetOwners().Any(o => o.Oib == person.Oib))) {
				return new HttpUnauthorizedResult();
			} 

			var model = new BillMethodModel {
				Bill = Mapper.Map<Bill, BillModel>(bill),
				Roles = Roles.GetRolesForUser(),
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult PayBill(int id) {
			if (!User.IsInRole("representative") && !User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var bill = billsRepository.GetById(id);
			var reserve = bill.Reserve;

			var buildig = reserve.Building;
			var person = personsRepository.GetPersonByUsername(User.Identity.Name);
			if(!buildig.RepresentativeOfPartOwners.Equals(person) && !buildig.BuildingManager.LegalPerson.Equals(person as LegalPerson)) {
				return new HttpUnauthorizedResult();
			}

			try {
				string url = string.Empty;
				// racun je izdala zgrada
				if(bill.From == null) {
					if (User.IsInRole("buildingmanager")) {
						reserve.ReceivePaymentFor(bill);
						url = Url.Action("bill", "finances", new {Id = bill.Id}, "http");
						emailNotifier.NotifyOfPaying(bill, url);
					} else {
						TempData["error"] = "Niste ovlašteni za potvrđivanje naplate računa.";
					}
					
				} else {
					// racun mora platiti zgrada
					if (User.IsInRole("representative")) {
						var isPaid = reserve.PayBill(bill);
						if (!isPaid) {
							TempData["error"] = "Nema dovoljno novaca u pričuvi za plaćanje računa..";
						} else {
							url = Url.Action("bill", "contractor", new { Id = bill.Id}, "http");
							emailNotifier.NotifyOfPaying(bill, url);
						}
					} else {
						TempData["error"] = "Niste ovlašteni za obavljanje plaćanja računa novcem iz pričuve.";
					}
					
				}

			} catch(BusinessRulesException ex) {
				TempData["error"] = ex.Message;
			}

			

			return RedirectToAction("bill", new { Id = bill.Id });

		}

		[NHibernateTransaction]
		public ActionResult ReservePeriods(int id) {
			if (!User.IsInRole("buildingmanager") && !User.IsInRole("representative")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);
			if (building == null) { return HttpNotFound(); }

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			var person = personsRepository.GetPersonByUsername(User.Identity.Name);
			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				if (!building.RepresentativeOfPartOwners.Equals(person)) {
					return new HttpUnauthorizedResult();
				}

				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Izdani računi pričuve");
			} else if (role == "buildingmanager") {
				if (!building.BuildingManager.LegalPerson.Equals(person as LegalPerson)) {
					return new HttpUnauthorizedResult();
				}

				links.Links = NavLinksGenerator.GetManagerLinks(building, "Izdani računi pričuve");
			} 

			var reserve = building.Reserve;

			var periods = billsRepository.GetReservePeriods(reserve);

			var model = new ReservePeriodsModel {
				Reserve = Mapper.Map<Reserve, ReserveModel>(reserve),
				Periods = periods,
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links
			};

			return View(model);
		}

		[NHibernateTransaction]
		public ActionResult ReserveBills(int id, DateTime date) {
			if (!User.IsInRole("buildingmanager") && !User.IsInRole("representative")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);
			if (building == null) { return HttpNotFound(); }

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			var person = personsRepository.GetPersonByUsername(User.Identity.Name);
			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				if (!building.RepresentativeOfPartOwners.Equals(person)) {
					return new HttpUnauthorizedResult();
				}

				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Izdani računi pričuve");
			} else if (role == "buildingmanager") {
				if (!building.BuildingManager.LegalPerson.Equals(person as LegalPerson)) {
					return new HttpUnauthorizedResult();
				}

				links.Links = NavLinksGenerator.GetManagerLinks(building, "Izdani računi pričuve");
			}

			var reserve = building.Reserve;
			var reserveBills = billsRepository.GetIssuedReserveBillsFor(reserve, date.Month, date.Year);
			if(reserveBills == null) {
				return HttpNotFound();
			}

			var unpaidBills = reserveBills.Where(b => b.IsPaid == false).ToList();
			var paidBills = reserveBills.Where(b => b.IsPaid).ToList();

			var model = new ReserveBillsModel {
				Date = date,
				Reserve = Mapper.Map<Reserve, ReserveModel>(reserve),
				UnpaidBills = Mapper.Map<IList<Bill>, IList<BillModel>>(unpaidBills),
				PaidBills = Mapper.Map<IList<Bill>, IList<BillModel>>(paidBills),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links
			};

			return View(model);
		}

		#endregion

	}
}
