using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using ISHousingMgmt.Domain.MembershipAndRoles;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using ISHousingMgmt.Web.Models.PersonsAndRoles;

namespace ISHousingMgmt.Web.Controllers {

	public class PersonController : Controller {

		#region Members

		private readonly IPersonsRepository personsRepository;
		private readonly IHousingMgmtUsersRepository housingMgmtUsersRepository;

		#endregion

		#region Constructors and Init

		public PersonController(IPersonsRepository personsRepository, IHousingMgmtUsersRepository housingMgmtUsersRepository) {
			this.personsRepository = personsRepository;
			this.housingMgmtUsersRepository = housingMgmtUsersRepository;
		}

		#endregion

		#region Actions

		[Authorize]
		[NHibernateTransaction]
		public ActionResult Index(int id) {
			var person = personsRepository.GetById(id);

			var model = new PersonIndexModel() {
				Roles = Roles.GetRolesForUser()
			};

			if (person is PhysicalPerson) {
				model.Person = Mapper.Map<PhysicalPerson, PhysicalPersonModel>(person as PhysicalPerson);
			} else {
				model.Person = Mapper.Map<LegalPerson, PhysicalPersonModel>(person as LegalPerson);
			}

			return View(model);
		}

		[NHibernateTransaction]
		public JsonResult Exists(string oib) {
			var person = personsRepository.GetByOib(oib);
			if (person == null) {
				return Json(null);
			} else {
				bool userExists = housingMgmtUsersRepository.GetUserByPerson(person) != null;
				if (person is PhysicalPerson) {
					return
						Json(
							 new {
								 Person = Mapper.Map<PhysicalPerson, PhysicalPersonModel>(person as PhysicalPerson),
								 UserExists = userExists
							 });
				} else {
					return Json(new { Person = Mapper.Map<LegalPerson, LegalPersonModel>(person as LegalPerson), UserExists = userExists });
				}
			}

		}

		#endregion

	}
}
