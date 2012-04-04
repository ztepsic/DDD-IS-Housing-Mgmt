using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using ISHousingMgmt.Web.Helpers;
using ISHousingMgmt.Web.Models;
using ISHousingMgmt.Web.Models.BuildingManagement;
using ISHousingMgmt.Web.Models.Finances;
using ISHousingMgmt.Web.Models.Legislature;
using ISHousingMgmt.Web.Models.Owner;
using IndexModel = ISHousingMgmt.Web.Models.Owner.IndexModel;

namespace ISHousingMgmt.Web.Controllers {
	[Authorize]
	public class OwnerController : Controller {

		#region Members

		private readonly IBuildingsRepository buildingsRepository;
		private readonly IPartitionSpacesRepository partitionSpacesRepository;
		private readonly IPersonsRepository personsRepository;
		private readonly IAdminJobsVotingsRepository adminJobsVotingsRepository;
		private readonly IBillsRepository billsRepository;

		#endregion

		#region Constructros and Init

		public OwnerController(IBuildingsRepository buildingsRepository, IPartitionSpacesRepository partitionSpacesRepository,
			IPersonsRepository personsRepository, IAdminJobsVotingsRepository adminJobsVotingsRepository, IBillsRepository billsRepository) {
			this.buildingsRepository = buildingsRepository;
			this.partitionSpacesRepository = partitionSpacesRepository;
			this.personsRepository = personsRepository;
			this.adminJobsVotingsRepository = adminJobsVotingsRepository;
			this.billsRepository = billsRepository;
		}

		#endregion

		#region Actions

		[Authorize]
		[NHibernateTransaction]
		public ActionResult Index() {
			if (!User.IsInRole("owner")) { return new HttpUnauthorizedResult(); }

			var owner = personsRepository.GetPersonByUsername(User.Identity.Name);
			var partitionSpaces = partitionSpacesRepository.GetPartitionSpaces(owner);
			var apartments = new List<ApartmentListModel>();
			foreach (var partitionSpace in partitionSpaces) {
				var building = buildingsRepository.GetByPartitionSpace(partitionSpace);
				var apartment = new ApartmentListModel {
					PartitionSpace = Mapper.Map<IPartitionSpace, PartitionSpaceListModel>(partitionSpace),
					Building = Mapper.Map<Building, BuildingListModel>(building)
				};
				apartments.Add(apartment);
			}


			var model = new IndexModel() {
				Apartments = apartments,
				Roles = Roles.GetRolesForUser()
			};


			return View(model);
		}

		[Authorize]
		[NHibernateTransaction]
		public ActionResult Apartment(int id) {
			if (!User.IsInRole("owner")) { return new HttpUnauthorizedResult(); }

			var partitionSpace = partitionSpacesRepository.GetById(id);
			if(partitionSpace == null) {
				return HttpNotFound();
			}

			var owner = personsRepository.GetPersonByUsername(User.Identity.Name);
			if(partitionSpace.Owner == null || !partitionSpace.Owner.Equals(owner)) {
				return new HttpUnauthorizedResult();
			}

			var building = buildingsRepository.GetByPartitionSpace(partitionSpace);
			var votings = adminJobsVotingsRepository.GetByBuilding(building, 5);

			var model = new ApartmentModel() {
				Apartment = new ApartmentDetailModel() {
					Building = Mapper.Map<Building, BuildingDetailModel>(building),
					PartitionSpace = Mapper.Map<IPartitionSpace, PartitionSpaceDetailModel>(partitionSpace)
				},
				Votings = Mapper.Map<IList<AdministrationJobsVoting>, IList<AdminJobsVotingListModel>>(votings),
				Roles = Roles.GetRolesForUser(),
				Links = new LinksModel() { Id = partitionSpace.Id, Links = NavLinksGenerator.GetOwnerLinks(building, string.Empty)}
			};

			Session.Add("role", "owner");
			Session.Add("lastPageId", partitionSpace.Id);

			return View(model);
		}

		[NHibernateTransaction]
		public ActionResult Bills(int id = 0) {
			if (!User.IsInRole("owner")) { return new HttpUnauthorizedResult(); }

			var person = personsRepository.GetPersonByUsername(User.Identity.Name);

			IList<Bill> bills = null;
			string role = string.Empty;
			LinksModel links = null;
			if (id > 0) {
				var building = buildingsRepository.GetById(id);
				bills = billsRepository.GetBillsIssuedFor(person, building.Reserve);
				role = "owner";
				links = new LinksModel {
					Id = building.Id,
					Links = NavLinksGenerator.GetOwnerLinks(building, "Moji računi")
				};
			} else {
				bills = billsRepository.GetBillsIssuedFor(person);
			}

			var paidBills = bills.Where(b => b.IsPaid).ToList();
			var unpaidBills = bills.Where(b => b.IsPaid == false).ToList();

			var model = new BillsModel {
				PaidBills = Mapper.Map<IList<Bill>, IList<BillModel>>(paidBills),
				UnpaidBills = Mapper.Map<IList<Bill>, IList<BillModel>>(unpaidBills),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult Bill(int id) {
			if (!User.IsInRole("owner")) { return new HttpUnauthorizedResult(); }

			var bill = billsRepository.GetById(id);
			if (bill == null) { return HttpNotFound(); }

			var building = bill.Reserve.Building;
			
			var person = personsRepository.GetPersonByUsername(User.Identity.Name);
			if (!building.GetOwners().Any(o => o.Oib == person.Oib)) {
				return new HttpUnauthorizedResult();
			}

			var model = new BillMethodModel {
				Bill = Mapper.Map<Bill, BillModel>(bill),
				Roles = Roles.GetRolesForUser()
			};

			return View("~/Views/Finances/Bill.cshtml", model);

		}

		[NHibernateTransaction]
		public ActionResult PrintBill(int id) {
			if (!User.IsInRole("owner")) { return new HttpUnauthorizedResult(); }

			var bill = billsRepository.GetById(id);
			if (bill == null) { return HttpNotFound(); }

			var building = bill.Reserve.Building;

			var person = personsRepository.GetPersonByUsername(User.Identity.Name);
			if (!building.GetOwners().Any(o => o.Oib == person.Oib)) {
				return new HttpUnauthorizedResult();
			}

			var model = new BillMethodModel {
				Bill = Mapper.Map<Bill, BillModel>(bill),
				Roles = Roles.GetRolesForUser()
			};

			return View("~/Views/Finances/PrintBill.cshtml", model);

		}

		#endregion


		public IList<BillModel> PaidBills { get; set; }

		public IList<BillModel> UnpaidBills { get; set; }
	}
}
