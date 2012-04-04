using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.MembershipAndRoles;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using ISHousingMgmt.Infrastructure.Services;
using ISHousingMgmt.Web.Exstensions;
using ISHousingMgmt.Web.Helpers;
using ISHousingMgmt.Web.Models;
using ISHousingMgmt.Web.Models.BuildingManagement;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Controllers {
	[Authorize]
	public class BuildingManagementController : Controller {

		#region Members

		private readonly ICitiesRepository citiesRepository;
		private readonly IBuildingsRepository buildingsRepository;
		private readonly IBuildingManagersRepository buildingManagersRepository;
		private readonly IPersonsRepository personsRepository;
		private readonly IAdminJobsVotingsRepository adminJobsVotingsRepository;
		private readonly IPartitionSpacesRepository partitionSpacesRepository;
		private readonly IRolesRepository rolesRepository;
		private readonly IHousingMgmtUsersRepository housingMgmtUsersRepository;
		private readonly IEmailNotifier emailNotifier;

		#endregion

		#region Constructors and Init

		public BuildingManagementController(ICitiesRepository citiesRepository, IBuildingsRepository buildingsRepository,
			IBuildingManagersRepository buildingManagersRepository, IPersonsRepository personsRepository,
			IAdminJobsVotingsRepository adminJobsVotingsRepository, IPartitionSpacesRepository partitionSpacesRepository,
			IRolesRepository rolesRepository, IHousingMgmtUsersRepository housingMgmtUsersRepository, 
			IEmailNotifier emailNotifier) {
			this.citiesRepository = citiesRepository;
			this.buildingsRepository = buildingsRepository;
			this.buildingManagersRepository = buildingManagersRepository;
			this.personsRepository = personsRepository;
			this.adminJobsVotingsRepository = adminJobsVotingsRepository;
			this.partitionSpacesRepository = partitionSpacesRepository;
			this.rolesRepository = rolesRepository;
			this.housingMgmtUsersRepository = housingMgmtUsersRepository;
			this.emailNotifier = emailNotifier;
		}

		#endregion

		#region Actions

		[NHibernateTransaction]
		public ActionResult Index() {
			if (!User.IsInRole("buildingmanager") && !User.IsInRole("representative")) { return new HttpUnauthorizedResult(); }

			IList<Building> buildings = null;
			if(User.IsInRole("buildingmanager")) {
				LegalPerson legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
				var buildingManager = buildingManagersRepository.GetByLegalPerson(legalPerson);
				buildings = buildingsRepository.GetBuildingsByManager(buildingManager);	
			} else {
				PhysicalPerson physicalPerson = personsRepository.GetPhysicalPersonByUsername(User.Identity.Name);
				buildings = buildingsRepository.GetBuildingsByRepresentative(physicalPerson);
			}

			
			var buildingListModel = Mapper.Map<IList<Building>, IList<BuildingListModel>>(buildings);
			var indexModel = new IndexModel() {
				Buildings = buildingListModel,
				Roles = Roles.GetRolesForUser()
			};
			

			return View(indexModel);
		}

		[NHibernateTransaction]
		public ActionResult Create() {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			AddressModel addressModel = new AddressModel() {
				Cities = new SelectList(citiesRepository.GetCitiesWithCadastres(), "Id", "Name")
			};

			var createModel = new CreateModel() {
				Address = addressModel,
				Roles = Roles.GetRolesForUser()
			};

			return View(createModel);
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult Create(CreateModel createModel) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			if (ModelState.IsValid) {
				// TODO provjeri da li vec postoji zgrada sa tom adresom

				LegalPerson legalPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);
				var buildingManager = buildingManagersRepository.GetByLegalPerson(legalPerson);

				try {
					Building building = new Building(buildingManager) {
						Address =
							new Address(createModel.Address.StreetAddress, createModel.Address.StreetAddressNumber,
							            citiesRepository.GetById(createModel.Address.City.Id))
					};

					buildingsRepository.SaveOrUpdate(building);

					return RedirectToAction("index");

				} catch (BusinessRulesException ex) {
					ex.CopyTo(ModelState);
				}

			}

			createModel.Address.Cities = new SelectList(citiesRepository.GetCitiesWithCadastres(), "Id", "Name",
													  createModel.Address.City.Id);
			createModel.Roles = Roles.GetRolesForUser();

			return View(createModel);	
		}

		[NHibernateTransaction]
		public ActionResult Building(int id) {
			if (!User.IsInRole("buildingmanager") && !User.IsInRole("representative")) { return new HttpUnauthorizedResult(); }
			string currentRole = string.Empty;
			if (User.IsInRole("representative")) {
				currentRole = "representative";
			} else {
				currentRole = "buildingmanager";
			}


			var building = buildingsRepository.GetById(id);
			if(building == null) {
				return HttpNotFound();
			}

			var votings = adminJobsVotingsRepository.GetByBuilding(building, 5);

			var buildingDetailModel = Mapper.Map<Building, BuildingDetailModel>(building);
			var votingsMapped = Mapper.Map<IList<AdministrationJobsVoting>, IList<AdminJobsVotingListModel>>(votings);

			LinksModel links = new LinksModel() {
				Id = building.Id
			};

			if (currentRole == "buildingmanager") {
				links.Links = NavLinksGenerator.GetManagerLinks(building, string.Empty);
			} else {
				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, string.Empty);
			}

			var buildingModel = new BuildingModel() {
				Building = buildingDetailModel,
				Votings = votingsMapped,
				Roles = Roles.GetRolesForUser(),
				CurrentRole = currentRole,
				Links = links
			};

			Session.Add("role", currentRole);
			Session.Add("lastPageId", building.Id);

			return View(buildingModel);
		}

		[NHibernateTransaction]
		public ActionResult AddManager(int id) {
			if (!User.IsInRole("representative")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);

			// provjeri da li je trenutna osoba predstavnik suvlasnika zgrade
			var representative = building.RepresentativeOfPartOwners;
			var currentUser = housingMgmtUsersRepository.GetUserByPerson(representative);

			if (currentUser == null) {
				return new HttpUnauthorizedResult();
			}

			var managers = buildingManagersRepository.GetAll()
				.Except(new List<BuildingManager> { building.BuildingManager}).ToList();

			var model = new AddBuildingManagerModel() {
				Managers = Mapper.Map<IList<BuildingManager>, IList<BuildingManagerModel>>(managers),
				Roles = Roles.GetRolesForUser(),
				Building = building.Address.StreetAddress + " " + building.Address.StreetAddressNumber,
				BuildingId = building.Id,
				CurrentRole = "representative",
				Links = new LinksModel() { Id = building.Id, Links = NavLinksGenerator.GetRepresentativeLinks(building, string.Empty)}
			};

			return View(model);

		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult AddManager(int id, int managerId) {
			if (!User.IsInRole("representative")) {
				return new HttpUnauthorizedResult();
			}

			var building = buildingsRepository.GetById(id);

			// provjeri da li je trenutna osoba predstavnik suvlasnika zgrade
			var representative = building.RepresentativeOfPartOwners;
			var currentUser = housingMgmtUsersRepository.GetUserByPerson(representative);

			if (currentUser == null) {
				return new HttpUnauthorizedResult();
			}

			var newManager = buildingManagersRepository.GetById(managerId);
			var newManagerUser = housingMgmtUsersRepository.GetUserByPerson(newManager.LegalPerson);

			// novi predstavnik mora imati otvoren racun (biti korisnik)
			if (newManagerUser != null) {
				var role = rolesRepository.GetRole("buildingmanager");

				// makni ulogu postojecem predstavniku
				var currentManager = building.BuildingManager;
				if (currentManager != null) {
					var currentManagerUser = housingMgmtUsersRepository.GetUserByPerson(currentManager.LegalPerson);
					currentManagerUser.RemoveRole(role);
				}

				// postavi ulogu novom predstavniku
				newManagerUser.AddRole(role);
				building.SetBuildingManager(newManager);
				return RedirectToAction("building", new {id = id});

			} else {
				ModelState.AddModelError("",
				                         string.Format(
				                                       "Osoba {0} ne može biti postavljena za upravitelja zgrade jer nema kreiran korisnički račun.",
				                                       newManager.LegalPerson.FullName));

				var managers = buildingManagersRepository.GetAll()
					.Except(new List<BuildingManager> {building.BuildingManager}).ToList();

				var model = new AddBuildingManagerModel() {
					Managers = Mapper.Map<IList<BuildingManager>, IList<BuildingManagerModel>>(managers),
					Roles = Roles.GetRolesForUser(),
					Building = building.Address.StreetAddress + " " + building.Address.StreetAddressNumber,
					BuildingId = building.Id,
					CurrentRole = "representative",
					Links = new LinksModel() {Id = building.Id,  Links = NavLinksGenerator.GetRepresentativeLinks(building, string.Empty)}
				};

				return View(model);
			}
		}

		[NHibernateTransaction]
		public ActionResult AddRepresentative(int id) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);

			// provjeri da li je trenutna osoba upravitelj zgrade
			var buildingManager = building.BuildingManager;
			var currentUser = housingMgmtUsersRepository.GetUserByPerson(buildingManager.LegalPerson);

			if(currentUser == null) {
				return new HttpUnauthorizedResult();
			}

			var owners = building.GetOwners()
				.Except(new List<Person> { building.RepresentativeOfPartOwners}).ToList();

			var model = new AddRepresentativeModel() {
				Owners = Mapper.Map<IList<Person>, IList<PersonModel>>(owners),
				Roles = Roles.GetRolesForUser(),
				Building = building.Address.StreetAddress + " " + building.Address.StreetAddressNumber,
				BuildingId = building.Id,
				CurrentRole = "buildingmanager",
				Links = new LinksModel() { Id = building.Id, Links = NavLinksGenerator.GetManagerLinks(building, string.Empty)}
			};

			return View(model);
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult AddRepresentative(int id, string oib) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);

			// provjeri da li je trenutna osoba upravitelj zgrade
			var buildingManager = building.BuildingManager;
			var currentPerson = personsRepository.GetLegalPersonByUsername(User.Identity.Name);

			if (currentPerson == null || !buildingManager.LegalPerson.Equals(currentPerson)) {
				return new HttpUnauthorizedResult();
			}

			var newRepresentative = personsRepository.GetByOib(oib);
			var newRepresentativeUser = housingMgmtUsersRepository.GetUserByPerson(newRepresentative);

			// novi predstavnik mora imati otvoren racun (biti korisnik)
			if (newRepresentativeUser != null) {
				var role = rolesRepository.GetRole("representative");

				// makni ulogu postojecem predstavniku
				var currentRepresentative = building.RepresentativeOfPartOwners;
				if (currentRepresentative != null) {
					var currentRepresentativeUser = housingMgmtUsersRepository.GetUserByPerson(currentRepresentative);
					currentRepresentativeUser.RemoveRole(role);
				}

				// postavi ulogu novom predstavniku
				newRepresentativeUser.AddRole(role);
				building.RepresentativeOfPartOwners = newRepresentative;
				return RedirectToAction("building", new { id = id });
				
			} else {
				ModelState.AddModelError("", string.Format("Osoba {0} ne može biti postavljena za predstavnika suvlasnika jer nema kreiran korisnički račun.", newRepresentative.FullName));

				var owners = building.GetOwners()
					.Except(new List<Person> { building.RepresentativeOfPartOwners }).ToList();

				var model = new AddRepresentativeModel() {
					Owners = Mapper.Map<IList<Person>, IList<PersonModel>>(owners),
					Roles = Roles.GetRolesForUser(),
					Building = building.Address.StreetAddress + " " + building.Address.StreetAddressNumber,
					BuildingId = building.Id,
					CurrentRole = "buildingmanager",
					Links = new LinksModel() {Id = building.Id, Links = NavLinksGenerator.GetManagerLinks(building, string.Empty)}
				};

				return View(model);
			}
			
		}

		[NHibernateTransaction]
		public ActionResult Voting(int id) {
			if (!User.IsInRole("owner") && !User.IsInRole("buildingmanager") && !User.IsInRole("representative")) {
				return new HttpUnauthorizedResult();
			}

			var voting = adminJobsVotingsRepository.GetById(id);
			if(voting == null) {
				return HttpNotFound();
			}

			
			if(!voting.IsFinished) {
				voting.Evaluate();	
				if(voting.IsFinished) {
					var url = Url.Action("voting", "buildingmanagement", new { Id = voting.Id }, "http");
					emailNotifier.NotifyOfAdminJobsVotingCompletition(voting, url);
				}
			}

			var currentPerson = personsRepository.GetPersonByUsername(User.Identity.Name);

			var adminJobsVotingDetail = Mapper.Map<AdministrationJobsVoting, AdminJobsVotingDetailModel>(voting);

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				links.Links = NavLinksGenerator.GetRepresentativeLinks(voting.Building, "Rad uprave");
			} else if (role == "buildingmanager") {
				links.Links = NavLinksGenerator.GetManagerLinks(voting.Building, "Rad uprave");
			} else if (role == "owner") {
				links.Links = NavLinksGenerator.GetOwnerLinks(voting.Building, "Rad uprave");
			}

			var model = new VotingModel() {
				Voting = adminJobsVotingDetail,
				IsUserVoted = voting.IsOwnerVoted(currentPerson),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult Votings(int id) {
			var building = buildingsRepository.GetById(id);
			if(building == null) {
				return HttpNotFound();
			}

			var votings = adminJobsVotingsRepository.GetByBuilding(building);
			foreach (var voting in votings) {
				if (!voting.IsFinished) {
					voting.Evaluate();
					if (voting.IsFinished) {
						var url = Url.Action("voting", "buildingmanagement", new { Id = voting.Id }, "http");
						emailNotifier.NotifyOfAdminJobsVotingCompletition(voting, url);
					}
				}
			}
			
			var votingsMapped = Mapper.Map<IList<AdministrationJobsVoting>, IList<AdminJobsVotingListModel>>(votings);

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			var isRepresentative = false;
			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Rad uprave");
				isRepresentative = true;
			} else if (role == "buildingmanager") {
				links.Links = NavLinksGenerator.GetManagerLinks(building, "Rad uprave");
			} else if (role == "owner") {
				links.Links = NavLinksGenerator.GetOwnerLinks(building, "Rad uprave");
			}

			var model = new VotingsModel() {
				Building = building.Address.ToString(),
				BuidlingId = building.Id,
				Votings = votingsMapped,
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links,
				IsRepresentative = isRepresentative
			};

			return View(model);

		}

		[NHibernateTransaction]
		public ActionResult CreateVoting(int id) {
			if (!User.IsInRole("representative")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);
			if(building == null) {
				return HttpNotFound();
			}

			var votingCreateModel = new VotingCreateModel() {
				EndDateTime = DateTime.Now.AddDays(1)
			};



			var model = new CreateVotingModel() {
				Voting = votingCreateModel,
				Roles = Roles.GetRolesForUser(),
				CurrentRole = "representative",
				Links = new LinksModel() {Id = building.Id, Links = NavLinksGenerator.GetRepresentativeLinks(building, "Rad uprave")}
			};

			return View(model);
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult CreateVoting(int id, CreateVotingModel createVotingModel) {
			if (!User.IsInRole("representative")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);

			if(building == null) {
				return HttpNotFound();
			}

			if (ModelState.IsValid) {
				var voting = createVotingModel.Voting;
				try {
					var administrationJobsType = (AdministrationJobsType) voting.AdministrationJobsType;
					AdministrationJobsVoting administrationJobsVoting = new AdministrationJobsVoting(administrationJobsType,
						building, voting.Subject, voting.Description, 
						voting.EndDateTime);

					adminJobsVotingsRepository.SaveOrUpdate(administrationJobsVoting);
					var url = Url.Action("voting", "buildingmanagement", new {Id = administrationJobsVoting.Id}, "http");
					emailNotifier.NotifyOfAdminJobsVotingCreation(administrationJobsVoting, url);
					return RedirectToAction("building", new { building.Id });

				} catch (BusinessRulesException ex) {
					ex.CopyTo(ModelState);
				}

			}

			createVotingModel.Roles = Roles.GetRolesForUser();
			createVotingModel.CurrentRole = "representative";
			createVotingModel.Links = new LinksModel() {Id = id, Links = NavLinksGenerator.GetRepresentativeLinks(building, "Rad Uprave")};

			return View(createVotingModel);	
		}

		[NHibernateTransaction]
		public ActionResult Vote(int id) {
			if (!User.IsInRole("owner")) { return new HttpUnauthorizedResult(); }

			var administrationJobsVoting = adminJobsVotingsRepository.GetById(id);
			if(administrationJobsVoting == null) {
				return HttpNotFound();
			}

			var currentPerson = personsRepository.GetPersonByUsername(User.Identity.Name);
			var isCurrentPersonOwner = administrationJobsVoting.Building.GetOwners().Contains(currentPerson);
			if(!isCurrentPersonOwner) {
				return new HttpUnauthorizedResult();
			}

			var votingVoteModel = Mapper.Map<AdministrationJobsVoting, VotingVoteModel>(administrationJobsVoting);

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
				links.Links = NavLinksGenerator.GetOwnerLinks(administrationJobsVoting.Building, "Rad uprave");
			}

			var model = new VoteModel() {
				Vote = votingVoteModel,
				Roles = Roles.GetRolesForUser(),
				CurrentRole = "owner",
				Links = links
			};

			return View(model);

		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult Vote(int id, VoteModel voteModel) {
			if (!User.IsInRole("owner")) { return new HttpUnauthorizedResult(); }

			var administrationJobsVoting = adminJobsVotingsRepository.GetById(id);
			if (administrationJobsVoting == null) {
				return HttpNotFound();
			}

			if (ModelState.IsValid) {
				var person = personsRepository.GetPersonByUsername(User.Identity.Name);
				var partitionSpace = partitionSpacesRepository.GetPartitionSpace(person, administrationJobsVoting.Building.LandRegistry);
				if(partitionSpace != null && partitionSpace.IsOwnedPartitionSpace) {
					try {
						var votingVoteModel = voteModel.Vote;
						var ownerVote = new OwnerVote(votingVoteModel.Vote, partitionSpace);
						administrationJobsVoting.AddVote(ownerVote);

						return RedirectToAction("voting", new { Id = administrationJobsVoting.Id });

					} catch (BusinessRulesException ex) {
						ex.CopyTo(ModelState);
					}	
				} else {
					ModelState.AddModelError("", "Etaža ne postoji ili niste vlasnik etaže za nevedenu zgradu, stoga ne možete glasati.");
				}

			}

			voteModel.Roles = Roles.GetRolesForUser();
			voteModel.CurrentRole = "owner";

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
				links.Links = NavLinksGenerator.GetOwnerLinks(administrationJobsVoting.Building, "Rad uprave");
			}

			voteModel.Links = links;

			return View(voteModel);	
		}

		[NHibernateTransaction]
		public ActionResult ChangeReserveCoef(int id) {
			if (!User.IsInRole("representative")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);
			if(building == null) {
				return HttpNotFound();
			}

			var model = new ChangeReservCoefModel() {
				ReserveCoefficient = building.ReserveCoefficient,
				Roles = Roles.GetRolesForUser(),
				CurrentRole = "representative",
				Links = new LinksModel() { Id = building.Id, Links = NavLinksGenerator.GetRepresentativeLinks(building)}
			};

			return View(model);

		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult ChangeReserveCoef(int id, ChangeReservCoefModel model) {
			if (!User.IsInRole("representative") && !User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(id);
			if(building == null) {
				return HttpNotFound();
			}

			if(ModelState.IsValid) {
				building.ReserveCoefficient = model.ReserveCoefficient;
				return RedirectToAction("building", new {Id = building.Id});
			} else {
				model.Roles = Roles.GetRolesForUser();
				model.CurrentRole = "representative";
				model.Links = new LinksModel() {Id = building.Id, Links = NavLinksGenerator.GetRepresentativeLinks(building)};
			
				return View(model);
			}

		}

		
		#endregion
	}
}
