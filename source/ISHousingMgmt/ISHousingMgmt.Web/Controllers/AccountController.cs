using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.MembershipAndRoles;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using ISHousingMgmt.Infrastructure.MembershipAndRoles;
using ISHousingMgmt.Infrastructure.Services;
using ISHousingMgmt.Web.Exstensions;
using ISHousingMgmt.Web.Models;
using ISHousingMgmt.Web.Models.Account;

namespace ISHousingMgmt.Web.Controllers {
	public class AccountController : Controller {

		#region Members

		private readonly ICitiesRepository citiesRepository;
		private readonly IRepairServicesRepository repairServicesRepository;
		private readonly IContractorsRepository contractorsRepository;
		private readonly IBuildingManagersRepository buildingManagersRepository;
		private readonly IPersonsRepository personsRepository;
		private readonly IRolesRepository rolesRepository;
		private readonly IPartitionSpacesRepository partitionSpacesRepository;
		private readonly IHousingMgmtUsersRepository housingMgmtUsersRepository;

		private readonly IEmailNotifier emailNotifier;

		#endregion

		#region Constructors and Init

		public AccountController(ICitiesRepository citiesRepository, IRepairServicesRepository repairServicesRepository,
			IContractorsRepository contractorsRepository, IBuildingManagersRepository buildingManagersRepository,
			IPersonsRepository personsRepository, IRolesRepository rolesRepository, IPartitionSpacesRepository partitionSpacesRepository,
			IHousingMgmtUsersRepository housingMgmtUsersRepository, IEmailNotifier emailNotifier) {
			this.citiesRepository = citiesRepository;
			this.repairServicesRepository = repairServicesRepository;
			this.contractorsRepository = contractorsRepository;
			this.buildingManagersRepository = buildingManagersRepository;
			this.personsRepository = personsRepository;
			this.rolesRepository = rolesRepository;
			this.partitionSpacesRepository = partitionSpacesRepository;
			this.housingMgmtUsersRepository = housingMgmtUsersRepository;
			this.emailNotifier = emailNotifier;
		}

		#endregion

		#region Actions

		/// <summary>
		/// Odjava sa sustava
		/// </summary>
		/// <returns></returns>
		[NHibernateTransaction]
		public ActionResult LogOff() {
			FormsAuthentication.SignOut();

			return RedirectToAction("login", "account");
		}

		/// <summary>
		/// Prikaz forme za prijavu na sustav
		/// </summary>
		/// <returns></returns>
		public ActionResult LogIn() {
			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("index", "dashboard");
			}

			return View();
		}

		/// <summary>
		/// Obrada podataka za prijavu
		/// </summary>
		/// <returns></returns>
		[NHibernateTransaction]
		[HttpPost]
		public ActionResult LogIn(LogInModel model, string returnUrl) {
			if (ModelState.IsValid) {
				if (Membership.ValidateUser(model.UserName, model.Password)) {
					FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
					if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")) {
						return Redirect(returnUrl);
					} else {
						return RedirectToAction("index", "dashboard");
					}
				} else {
					ModelState.AddModelError("", "Korisničko ime ili lozinka nisu ispravni.");
				}
			}

			// nesto nije uredu sa prijavom, ponovno prikazu formu
			return View(model);
		}

		/// <summary>
		/// Prikazuje formu za registraciju upravitelja
		/// </summary>
		/// <returns></returns>
		[NHibernateTransaction]
		public ActionResult RegisterManager() {
			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("index", "dashboard");
			}

			RegisterManagerUserModel model = new RegisterManagerUserModel {
				Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name")
			};

			return View(model);
		}

		/// <summary>
		/// Obavlja registraciju upravitelja
		/// </summary>
		/// <param name="managerModel"></param>
		/// <returns></returns>
		[NHibernateTransaction]
		[HttpPost]
		public ActionResult RegisterManager(RegisterManagerUserModel managerModel) {
			if (ModelState.IsValid) {
				Person person = personsRepository.GetByOib(managerModel.Oib);
				bool isUserExistingForPerson = false;
				if(person != null) {
					var existingUser = housingMgmtUsersRepository.GetUserByPerson(person);
					isUserExistingForPerson = existingUser != null;
				}

				if (!isUserExistingForPerson) {

					MembershipCreateStatus createStatus;
					NHibernateMembershipUser membershipUser =
						Membership.CreateUser(managerModel.UserName, managerModel.Password, managerModel.Email,
						                      null, null, true, null, out createStatus) as NHibernateMembershipUser;

					if (createStatus == MembershipCreateStatus.Success) {
						if (person == null) {
							City city = citiesRepository.GetById(managerModel.City);
							person = new LegalPerson(managerModel.Oib, managerModel.Name) {
								NumberOfBankAccount = managerModel.NumberOfBankAccount,
								Address = new Address(managerModel.StreetAddress, managerModel.StreetAddressNumber, city)
							};

							if (!string.IsNullOrEmpty(managerModel.TelephoneNumber)) {
								person.AddTelephone(new Telephone("Telefon", managerModel.TelephoneNumber));
							}

							if (!string.IsNullOrEmpty(managerModel.MobileNumber)) {
								person.AddTelephone(new Telephone("Mobitel", managerModel.MobileNumber));
							}
						}

						if (person is LegalPerson) {
							BuildingManager buildingManager = null;

							try {
								membershipUser.User.Person = person;

								var managerRole = rolesRepository.GetRole("buildingmanager");
								membershipUser.User.AddRole(managerRole);

								var partitionSpaces = partitionSpacesRepository.GetPartitionSpaces(person);
								if (partitionSpaces.Count > 0) {
									var ownerRole = rolesRepository.GetRole("owner");
									membershipUser.User.AddRole(ownerRole);
								}

								LegalPerson legalPerson = (LegalPerson) person;
								buildingManager = new BuildingManager(legalPerson);

								FormsAuthentication.SetAuthCookie(managerModel.UserName, false);
								buildingManagersRepository.SaveOrUpdate(buildingManager);
								emailNotifier.NotifyOfRegistration(membershipUser.User);
								return RedirectToAction("index", "dashboard");

							} catch (BusinessRulesException ex) {
								ex.CopyTo(ModelState);
							}
						} else {
							ModelState.AddModelError("Oib", "Osoba sa ovim OIB-om je već upisana te nije pravna osoba.");
						}

					} else {
						ModelState.AddModelError("", errorCodeToString(createStatus));
					}
				} else {
					ModelState.AddModelError("", "Navedena osoba već ima kreiran korisnički račun.");	
				}
			}

			managerModel.Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name", managerModel.City);

			return View(managerModel);
		}

		/// <summary>
		/// Prikazuje formu za registraciju vlasnika
		/// </summary>
		/// <returns></returns>
		[NHibernateTransaction]
		public ActionResult RegisterOwner() {
			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("index", "dashboard");
			}

			RegisterOwnerUserModel model = new RegisterOwnerUserModel {
				Address = new AddressModel() { Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name") }
			};

			return View(model);
		}

		[NHibernateTransaction]
		[HttpPost]
		public ActionResult RegisterOwner(RegisterOwnerUserModel ownerUserModel) {
			if (ModelState.IsValid) {
				Person person = personsRepository.GetByOib(ownerUserModel.Oib);
				bool isUserExistingForPerson = false;
				if(person != null) {
					var existingUser = housingMgmtUsersRepository.GetUserByPerson(person);
					isUserExistingForPerson = existingUser != null;
				}
				
				if(!isUserExistingForPerson) {
					MembershipCreateStatus createStatus;
					NHibernateMembershipUser membershipUser =
						Membership.CreateUser(ownerUserModel.UserName, ownerUserModel.Password, ownerUserModel.Email,
											  null, null, true, null, out createStatus) as NHibernateMembershipUser;

					if (createStatus == MembershipCreateStatus.Success) {
						if (person == null) {
							City city = citiesRepository.GetById(ownerUserModel.Address.City.Id);

							if (string.IsNullOrEmpty(ownerUserModel.Surname)) {
								person = new LegalPerson(ownerUserModel.Oib, ownerUserModel.Name);
							} else {
								person = new PhysicalPerson(ownerUserModel.Oib, ownerUserModel.Name, ownerUserModel.Surname);
							}

							person.Address = new Address(ownerUserModel.Address.StreetAddress, ownerUserModel.Address.StreetAddressNumber,
														 city);

							if (!string.IsNullOrEmpty(ownerUserModel.TelephoneNumber)) {
								person.AddTelephone(new Telephone("Telefon", ownerUserModel.TelephoneNumber));
							}

							if (!string.IsNullOrEmpty(ownerUserModel.MobileNumber)) {
								person.AddTelephone(new Telephone("Mobitel", ownerUserModel.MobileNumber));
							}
						}

						try {
							membershipUser.User.Person = person;

							var role = rolesRepository.GetRole("owner");
							membershipUser.User.AddRole(role);

							FormsAuthentication.SetAuthCookie(ownerUserModel.UserName, false);
							personsRepository.SaveOrUpdate(person);
							emailNotifier.NotifyOfRegistration(membershipUser.User);
							return RedirectToAction("index", "dashboard");

						} catch (BusinessRulesException ex) {
							ex.CopyTo(ModelState);
						}

					} else {
						ModelState.AddModelError("", errorCodeToString(createStatus));
					}
				} else {
					ModelState.AddModelError("", "Navedena osoba već ima kreiran korisnički račun.");	
				}

			}


			ownerUserModel.Address.Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name");

			return View(ownerUserModel);
		}

		/// <summary>
		/// Prikazuje formu za registraciju izvodaca radova
		/// </summary>
		/// <returns></returns>
		[NHibernateTransaction]
		public ActionResult RegisterContractor() {
			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("index", "dashboard");
			}

			RegisterContractorUserModel model = new RegisterContractorUserModel {
				Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name"),
				RepairServices = new SelectList(repairServicesRepository.GetAll(), "Id", "Name")
			};

			return View(model);
		}

		/// <summary>
		/// Obavlja registraciju novog izvodaca radova
		/// </summary>
		/// <param name="contractorModel"></param>
		/// <returns></returns>
		[NHibernateTransaction]
		[HttpPost]
		public ActionResult RegisterContractor(RegisterContractorUserModel contractorModel) {
			if (ModelState.IsValid) {
				Person person = personsRepository.GetByOib(contractorModel.Oib);
				bool isUserExistingForPerson = false;
				if (person != null) {
					var existingUser = housingMgmtUsersRepository.GetUserByPerson(person);
					isUserExistingForPerson = existingUser != null;
				}

				if (!isUserExistingForPerson) {
					MembershipCreateStatus createStatus;
					NHibernateMembershipUser membershipUser =
						Membership.CreateUser(contractorModel.UserName, contractorModel.Password, contractorModel.Email,
						                      null, null, true, null, out createStatus) as NHibernateMembershipUser;

					if (createStatus == MembershipCreateStatus.Success) {


						if (person == null) {
							// Probaj kreirati pravnu osobu i izvodaca radova
							City city = citiesRepository.GetById(contractorModel.City);
							person = new LegalPerson(contractorModel.Oib, contractorModel.Name) {
								NumberOfBankAccount = contractorModel.NumberOfBankAccount,
								Address = new Address(contractorModel.StreetAddress, contractorModel.StreetAddressNumber, city)
							};

							if (!string.IsNullOrEmpty(contractorModel.TelephoneNumber)) {
								person.AddTelephone(new Telephone("Telefon", contractorModel.TelephoneNumber));
							}

							if (!string.IsNullOrEmpty(contractorModel.MobileNumber)) {
								person.AddTelephone(new Telephone("Mobitel", contractorModel.MobileNumber));
							}
						}

						if (person is LegalPerson) {
							Contractor contractor = null;
							try {
								membershipUser.User.Person = person;

								var contractorRole = rolesRepository.GetRole("contractor");
								membershipUser.User.AddRole(contractorRole);

								var partitionSpaces = partitionSpacesRepository.GetPartitionSpaces(person);
								if (partitionSpaces.Count > 0) {
									var ownerRole = rolesRepository.GetRole("owner");
									membershipUser.User.AddRole(ownerRole);
								}

								contractor = new Contractor(person as LegalPerson);

								foreach (var repairServiceId in contractorModel.SelectedRepairServices) {
									var repairService = repairServicesRepository.GetById(repairServiceId);
									if (repairService != null) {
										contractor.AddRepairService(repairService);
									}
								}

								FormsAuthentication.SetAuthCookie(contractorModel.UserName, false);
								contractorsRepository.SaveOrUpdate(contractor);
								emailNotifier.NotifyOfRegistration(membershipUser.User);
								return RedirectToAction("index", "dashboard");

							} catch (BusinessRulesException ex) {
								ex.CopyTo(ModelState);
							}
						} else {
							ModelState.AddModelError("Oib", "Osoba sa ovim OIB-om je već upisana te nije pravna osoba.");
						}

					} else {
						ModelState.AddModelError("", errorCodeToString(createStatus));
					}
				} else {
					ModelState.AddModelError("", "Navedena osoba već ima kreiran korisnički račun.");	
				}

			}

			contractorModel.Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name");
			contractorModel.RepairServices = new SelectList(repairServicesRepository.GetAll(), "Id", "Name", contractorModel.City);

			return View(contractorModel);
		}

		[Authorize]
		[NHibernateTransaction]
		public ActionResult Edit() {
			var person = personsRepository.GetPersonByUsername(User.Identity.Name);
			if (person == null) {
				return HttpNotFound();
			}

			NHibernateMembershipUser user = Membership.GetUser() as NHibernateMembershipUser;

			var model = new EditModel {
				Roles = Roles.GetRolesForUser(),
				UserName = user.UserName,
				Email = user.Email,
				Name = person.Name,
				Address = Mapper.Map<Address, AddressModel>(person.Address)
			};

			if (person is PhysicalPerson) {
				model.Surname = (person as PhysicalPerson).Surname;
			}

			model.Address.Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name", person.Address.City.Id);
			foreach (var telephone in person.Telephones) {
				if (telephone.NameOfTelephoneNumber == "Telefon") {
					model.TelephoneNumber = telephone.TelephoneNumber;
				} else if (telephone.TelephoneNumber == "Mobitel") {
					model.MobileNumber = telephone.TelephoneNumber;
				}
			}

			return View(model);
		}

		[Authorize]
		[NHibernateTransaction]
		[HttpPost]
		public ActionResult Edit(EditModel editModel) {
			if (ModelState.IsValid) {
				var person = personsRepository.GetPersonByUsername(User.Identity.Name);
				if (person == null) {
					return HttpNotFound();
				}

				person.Name = editModel.Name;

				if (!string.IsNullOrEmpty(editModel.Surname) && person is PhysicalPerson) {
					(person as PhysicalPerson).Surname = editModel.Surname;
				}

				var city = citiesRepository.GetById(editModel.Address.City.Id);
				var address = new Address(editModel.Address.StreetAddress, editModel.Address.StreetAddressNumber, city);
				person.Address = address;

				if (!string.IsNullOrEmpty(editModel.TelephoneNumber)) {
					person.AddTelephone(new Telephone("Telefon", editModel.TelephoneNumber));
				} else if (!string.IsNullOrEmpty(editModel.MobileNumber)) {
					person.AddTelephone(new Telephone("Mobitel", editModel.MobileNumber));
				}

				NHibernateMembershipUser user = Membership.GetUser() as NHibernateMembershipUser;
				user.User.Email = editModel.Email;

				if (!string.IsNullOrEmpty(editModel.Password) && !string.IsNullOrEmpty(editModel.ConfirmPassword) &&
					editModel.Password.Equals(editModel.ConfirmPassword)) {
					var membershipProvider = Membership.Provider;
					var isPasswordChanged = membershipProvider.ChangePassword(User.Identity.Name, editModel.OldPassword, editModel.Password);
					if (!isPasswordChanged) {
						ModelState.AddModelError("OldPassword", "Unjeli ste neispravnu trenutnu lozinku.");
						editModel.Address.Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name", editModel.Address.City.Id);
						editModel.Roles = Roles.GetRolesForUser();
						editModel.UserName = User.Identity.Name;
						return View(editModel);
					}
				}

				return RedirectToAction("index", "dashboard");

			}

			editModel.Address.Cities = new SelectList(citiesRepository.GetAll(), "Id", "Name", editModel.Address.City.Id);
			editModel.Roles = Roles.GetRolesForUser();
			editModel.UserName = User.Identity.Name;
			return View(editModel);

		}

		//
		// GET: /Account/ChangePassword

		[Authorize]
		public ActionResult ChangePassword() {
			return View();
		}

		//
		// POST: /Account/ChangePassword

		[Authorize]
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model) {
			if (ModelState.IsValid) {

				// ChangePassword will throw an exception rather
				// than return false in certain failure scenarios.
				bool changePasswordSucceeded;
				try {
					MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
					changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
				} catch (Exception) {
					changePasswordSucceeded = false;
				}

				if (changePasswordSucceeded) {
					return RedirectToAction("ChangePasswordSuccess");
				} else {
					ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Account/ChangePasswordSuccess

		public ActionResult ChangePasswordSuccess() {
			return View();
		}

		#endregion

		#region Status Codes
		private static string errorCodeToString(MembershipCreateStatus createStatus) {
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus) {
				case MembershipCreateStatus.DuplicateUserName:
					return "User name already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}
		#endregion

	}
}
