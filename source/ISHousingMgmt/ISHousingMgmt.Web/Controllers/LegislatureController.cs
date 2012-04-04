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
using ISHousingMgmt.Web.Exstensions;
using ISHousingMgmt.Web.Helpers;
using ISHousingMgmt.Web.Models;
using ISHousingMgmt.Web.Models.Legislature;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Controllers {
	[Authorize]
	public class LegislatureController : Controller {

		#region Members

		private readonly ICitiesRepository citiesRepository;
		private readonly ICadastresRepository cadastresRepository;
		private readonly ILandRegistriesRepository landRegistriesRepository;
		private readonly IPartitionSpacesRepository partitionSpacesRepository;
		private readonly IPersonsRepository personsRepository;
		private readonly IBuildingsRepository buildingsRepository;
		private readonly IHousingMgmtUsersRepository housingMgmtUsersRepository;
		private readonly IRolesRepository rolesRepository;

		#endregion

		#region Constructors and Init

		public LegislatureController(ICitiesRepository citiesRepository, ICadastresRepository cadastresRepository,
			ILandRegistriesRepository landRegistriesRepository, IPartitionSpacesRepository partitionSpacesRepository,
			IPersonsRepository personsRepository, IBuildingsRepository buildingsRepository,
			IHousingMgmtUsersRepository housingMgmtUsersRepository, IRolesRepository rolesRepository) {
			this.citiesRepository = citiesRepository;
			this.cadastresRepository = cadastresRepository;
			this.landRegistriesRepository = landRegistriesRepository;
			this.partitionSpacesRepository = partitionSpacesRepository;
			this.personsRepository = personsRepository;
			this.buildingsRepository = buildingsRepository;
			this.housingMgmtUsersRepository = housingMgmtUsersRepository;
			this.rolesRepository = rolesRepository;
		}

		#endregion

		#region Actions

		[NHibernateTransaction]
		public ActionResult Create(int buildingId) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(buildingId);
			if (building != null) {
				var createLandRegistryModel = new CreateLandRegistryModel() {
					Cities = new SelectList(citiesRepository.GetCitiesWithCadastres(), "Id", "Name")
				};

				var createModel = new CreateModel() {
					LandRegistry = createLandRegistryModel,
					Roles = Roles.GetRolesForUser(),
					CurrentRole = "buildingmanager",
					Links = new LinksModel { Id = buildingId, Links = NavLinksGenerator.GetManagerLinks(building)}
				};

				return View(createModel);
			} else {
				return HttpNotFound();
			}
		}


		[NHibernateTransaction]
		[HttpPost]
		public ActionResult Create(int buildingId, CreateModel createModel) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var building = buildingsRepository.GetById(buildingId);
			if(building == null) { return HttpNotFound(); }

			if (ModelState.IsValid) {
				// provjeri da li vec postoji katastarska cestica pod tim brojem
				var landRegistryModel = createModel.LandRegistry;

				LandRegistry landRegistry =
					landRegistriesRepository.GetByNumberOfCadastralParticle(landRegistryModel.NumberOfCadastralParticle);

				if (landRegistry == null) {
					try {
						Cadastre cadastre = cadastresRepository.GetById(landRegistryModel.Cadastre);
						CadastralParticle cadastralParticle = new CadastralParticle(cadastre, landRegistryModel.NumberOfCadastralParticle,
							landRegistryModel.SurfaceArea, landRegistryModel.Description);

						landRegistry = new LandRegistry(cadastralParticle);
						landRegistriesRepository.SaveOrUpdate(landRegistry);

						building.LandRegistry = landRegistry;

						return RedirectToAction("landregistry", new { Id = landRegistry.Id });

					} catch (BusinessRulesException ex) {
						ex.CopyTo(ModelState);
					}	

				} else {
					ModelState.AddModelError("NumberOfCadastralParticle", "Zemljisna knjiga sa takvim brojem katastarske cestice već postoji.");
				}
			}

			createModel.LandRegistry.Cities = new SelectList(citiesRepository.GetCitiesWithCadastres(), "Id", "Name",
			                                          createModel.LandRegistry.City);
			createModel.Roles = Roles.GetRolesForUser();
			createModel.CurrentRole = "buildingmanager";
			createModel.Links = new LinksModel {Id = buildingId, Links = NavLinksGenerator.GetManagerLinks(building)};


			return View(createModel);	
		}

		[NHibernateTransaction]
		public JsonResult Cadastres(int cityId) {
			var cadastres = cadastresRepository.GetByCity(cityId);
			IList<CadastreJsonModel> jsonCadastres = Mapper.Map<IList<Cadastre>, IList<CadastreJsonModel>>(cadastres);
			return Json(jsonCadastres);
		}

		[Authorize]
		[NHibernateTransaction]
		public ActionResult LandRegistry(int id) {
			if(!User.IsInRole("buildingmanager") && !User.IsInRole("representative")) {
				return new HttpUnauthorizedResult();
			}

			var landRegistry = landRegistriesRepository.GetById(id);
			if(landRegistry == null) {
				return HttpNotFound();
			}

			var building = buildingsRepository.GetByLandRegistry(landRegistry);

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			string role = (string)Session["role"] ?? string.Empty;
			if(role == "representative") {
				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Zemljišna knjiga");
			} else if (role == "buildingmanager") {
				links.Links = NavLinksGenerator.GetManagerLinks(building, "Zemljišna knjiga");
			} else if (role == "owner") {
				links.Links = NavLinksGenerator.GetOwnerLinks(building, "Zemljišna knjiga");
			}

			var landRegistryDetailModel = Mapper.Map<LandRegistry, LandRegistryDetailModel>(landRegistry);
			var landRegistryModel = new LandRegistryModel() {
				LandRegistry = landRegistryDetailModel,
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links
			};

			return View(landRegistryModel);
		}

		[NHibernateTransaction]
		public ActionResult AddPartitionSpace(int id, string owned) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			var landRegistry = landRegistriesRepository.GetById(id);
			if(landRegistry == null) { return HttpNotFound(); }

			var building = buildingsRepository.GetByLandRegistry(landRegistry);
			var cadastralParticle = landRegistry.CadastralParticle;
			

			PartitionSpaceCreateModel partitionSpaceCreateModel = new PartitionSpaceCreateModel() {
				CadastralParticle = Mapper.Map<CadastralParticle, CadastralParticleDetailModel>(cadastralParticle)
			};

			var addPartitionSpaceModel = new AddPartitionSpaceModel() {
				PartitionSpace = partitionSpaceCreateModel,
				Roles = Roles.GetRolesForUser(),
				CurrentRole = "buildingmanager",
				Links = new LinksModel { Id = building.Id, Links = NavLinksGenerator.GetManagerLinks(building, "Zemljišna knjiga")}
			};

			if (string.IsNullOrEmpty(owned)) {
				return View(addPartitionSpaceModel);
			} else {
				partitionSpaceCreateModel.Owner = new PhysicalPersonModel {
					Address = new AddressModel() { Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name") }
				};
				return View("AddOwnedPartitionSpace", addPartitionSpaceModel);
			}

			
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult AddPartitionSpace(int id, string owned, AddPartitionSpaceModel addPartitionSpaceModel) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			LandRegistry landRegistry = landRegistriesRepository.GetById(id);
			if (ModelState.IsValid) {
				// provjeri da li vec postoji etaža sa istim brojem uloška
				IPartitionSpace existingPartitionSpace = partitionSpacesRepository.GetPartitionSpace(addPartitionSpaceModel.PartitionSpace.RegistryNumber);

				if (existingPartitionSpace == null) {
					if(landRegistry != null) {
						var partitionSpaceModel = addPartitionSpaceModel.PartitionSpace;
						try {
							if(string.IsNullOrEmpty(owned)) {
								landRegistry.CreatePartitionSpace(partitionSpaceModel.RegistryNumber, partitionSpaceModel.SurfaceArea,
															  partitionSpaceModel.Description);	
							} else {
								Person owner = personsRepository.GetByOib(partitionSpaceModel.Owner.Oib);
								var addressModel = partitionSpaceModel.Owner.Address;
								var address = new Address(addressModel.StreetAddress, addressModel.StreetAddressNumber, citiesRepository.GetById(addressModel.City.Id));
								if(string.IsNullOrEmpty(partitionSpaceModel.Owner.Surname)) {
									owner = owner ?? new LegalPerson(partitionSpaceModel.Owner.Oib, partitionSpaceModel.Owner.Name) {
										Address = address
									};
								} else {
									owner = owner ?? new PhysicalPerson(partitionSpaceModel.Owner.Oib, partitionSpaceModel.Owner.Name,
										partitionSpaceModel.Owner.Surname) {
											Address = address
									};
								}

								var ownerRole = rolesRepository.GetRole("owner");
								var newOwnerUser = housingMgmtUsersRepository.GetUserByPerson(owner);
								if (newOwnerUser != null && !newOwnerUser.Roles.Contains(ownerRole)) {
									newOwnerUser.AddRole(ownerRole);
								}

								landRegistry.CreatePartitionSpace(partitionSpaceModel.RegistryNumber, partitionSpaceModel.SurfaceArea,
															  partitionSpaceModel.Description, owner);
							}
							

							return RedirectToAction("landregistry", new { id = landRegistry.Id });

						} catch (BusinessRulesException ex) {
							ex.CopyTo(ModelState);
						}	
					} else {
						return HttpNotFound();
					}				
				} else {
					ModelState.AddModelError("PartitionSpace.RegistryNumber", "Etaža sa takvim brojem uloška već postoji.");
				}
			}


			
			var cadastralParticle = landRegistry.CadastralParticle;
			var building = buildingsRepository.GetByLandRegistry(landRegistry);
			addPartitionSpaceModel.PartitionSpace.CadastralParticle = Mapper.Map<CadastralParticle, CadastralParticleDetailModel>(cadastralParticle);
			addPartitionSpaceModel.Roles = Roles.GetRolesForUser();
			addPartitionSpaceModel.CurrentRole = "buildingmanager";
			addPartitionSpaceModel.Links = new LinksModel {Id = building.Id, Links = NavLinksGenerator.GetManagerLinks(building)};

			if (string.IsNullOrEmpty(owned)) {
				return View(addPartitionSpaceModel);
			} else {
				addPartitionSpaceModel.PartitionSpace.Owner.Address.Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name");
				return View("AddOwnedPartitionSpace", addPartitionSpaceModel);
			}	
		}

		[NHibernateTransaction]
		public ActionResult LockLandRegistry(int id) {
			if (!User.IsInRole("buildingmanager")) { return new HttpUnauthorizedResult(); }

			LandRegistry landRegistry = landRegistriesRepository.GetById(id);
			if (landRegistry != null) {
				landRegistry.Locked = true;
				return RedirectToAction("landregistry", new {Id = id});
			} else {
				return HttpNotFound();
			}
		}

		[NHibernateTransaction]
		public ActionResult PartitionSpace(int id) {
			IPartitionSpace partitionSpace = partitionSpacesRepository.GetById(id);
			if(partitionSpace == null) {
				return HttpNotFound();
			}

			PartitionSpaceDetailModel detailModel = Mapper.Map<IPartitionSpace, PartitionSpaceDetailModel>(partitionSpace);

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			var building = buildingsRepository.GetByPartitionSpace(partitionSpace);

			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Zemljišna knjiga");
			} else if (role == "buildingmanager") {
				links.Links = NavLinksGenerator.GetManagerLinks(building, "Zemljišna knjiga");
			} else if (role == "owner") {
				links.Links = NavLinksGenerator.GetOwnerLinks(building, "Zemljišna knjiga");
			}

			var partitionSpaceModel = new PartitionSpaceModel() {
				PartitionSpace = detailModel,
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links
			};


			return View(partitionSpaceModel);

		}

		[NHibernateTransaction]
		public ActionResult ChangeOwner(int id) {
			if(!User.IsInRole("buildingmanager") && !User.IsInRole("representative")) {
				return new HttpUnauthorizedResult();
			}

			var partitionSpace = partitionSpacesRepository.GetById(id);
			if(partitionSpace == null) {
				return HttpNotFound();
			}

			if(!partitionSpace.IsOwnedPartitionSpace) {
				return new HttpStatusCodeResult(405);
			}

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			var building = buildingsRepository.GetByPartitionSpace(partitionSpace);

			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Zemljišna knjiga");
			} else if (role == "buildingmanager") {
				links.Links = NavLinksGenerator.GetManagerLinks(building, "Zemljišna knjiga");
			}

			var newOwner = new PhysicalPersonModel {
				Address = new AddressModel() {
					Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name")
				}
			};

			var model = new ChangeOwnerModel {
				NewOwner = newOwner,
				PartitionSpace = Mapper.Map<PartitionSpace, PartitionSpaceDetailModel>(partitionSpace),
				Roles = Roles.GetRolesForUser(),
				CurrentRole = role,
				Links = links
			};

			return View(model);

		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult ChangeOwner(int id, ChangeOwnerModel model) {
			if (!User.IsInRole("buildingmanager") && !User.IsInRole("representative")) {
				return new HttpUnauthorizedResult();
			}

			var partitionSpace = partitionSpacesRepository.GetById(id);
			if (partitionSpace == null) {
				return HttpNotFound();
			}

			if (!partitionSpace.IsOwnedPartitionSpace) {
				return new HttpStatusCodeResult(405);
			}

			if(ModelState.IsValid) {
				var person = personsRepository.GetByOib(model.NewOwner.Oib);
				if(person != null) {
					partitionSpace.Owner = person;
				} else {
					Person newPerson = null;
					if(string.IsNullOrEmpty(model.NewOwner.Surname)) {
						newPerson = new LegalPerson(model.NewOwner.Oib, model.NewOwner.Name);
					} else {
						newPerson = new PhysicalPerson(model.NewOwner.Oib, model.NewOwner.Name, model.NewOwner.Surname);
					}

					var city = citiesRepository.GetById(model.NewOwner.Address.City.Id);
					newPerson.Address = new Address(model.NewOwner.Address.StreetAddress, model.NewOwner.Address.StreetAddressNumber, city);

					var ownerRole = rolesRepository.GetRole("owner");
					var contractorRole = rolesRepository.GetRole("contractor");
					var managerRole = rolesRepository.GetRole("buildingmanager");

					var previousOwner = partitionSpace.Owner;
					var previousOwnerUser = housingMgmtUsersRepository.GetUserByPerson(previousOwner);
					if(previousOwnerUser.Roles.Contains(contractorRole) || previousOwnerUser.Roles.Contains(managerRole)) {
						previousOwnerUser.RemoveRole(ownerRole);
					}

					partitionSpace.Owner = newPerson;
					var newOwnerUser = housingMgmtUsersRepository.GetUserByPerson(newPerson);
					if(newOwnerUser != null && !newOwnerUser.Roles.Contains(ownerRole)) {
						newOwnerUser.AddRole(ownerRole);
					}

				}

				return RedirectToAction("partitionspace", new {Id = id});
			}

			LinksModel links = new LinksModel();
			if (Session["lastPageId"] != null) {
				links.Id = (int)Session["lastPageId"];
			}

			var building = buildingsRepository.GetByPartitionSpace(partitionSpace);

			string role = (string)Session["role"] ?? string.Empty;
			if (role == "representative") {
				links.Links = NavLinksGenerator.GetRepresentativeLinks(building, "Zemljišna knjiga");
			} else if (role == "buildingmanager") {
				links.Links = NavLinksGenerator.GetManagerLinks(building, "Zemljišna knjiga");
			}

			model.NewOwner.Address.Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name", model.NewOwner.Address.City.Id);
			model.Roles = Roles.GetRolesForUser();
			model.CurrentRole = role;
			model.Links = links;

			return View(model);
		}

		#endregion

	}
}
