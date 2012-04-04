using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.PersonsAndRoles {
	public class PersonsNHRepository : NHibernateCrudRepository<Person>, IPersonsRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public PersonsNHRepository() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public PersonsNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }


		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca pravnu osobu preko oib-a
		/// </summary>
		/// <param name="oib">oib</param>
		/// <returns>pravna osoba</returns>
		public LegalPerson GetLegalPersonByOib(string oib) {
			return Session.QueryOver<LegalPerson>()
				.Where(lp => lp.Oib == oib)
				.SingleOrDefault();
		}

		/// <summary>
		/// Dohvaca fizicku osobu preko oib-a
		/// </summary>
		/// <param name="oib">oib</param>
		/// <returns>fizicka osoba</returns>
		public PhysicalPerson GetPhysicalPersonByOib(string oib) {
			return Session.QueryOver<PhysicalPerson>()
				.Where(pp => pp.Oib == oib)
				.SingleOrDefault();
		}

		/// <summary>
		/// Dohvaca osobu preko oib-a
		/// </summary>
		/// <param name="oib"></param>
		/// <returns></returns>
		public Person GetByOib(string oib) {
			var hql = @"from Person p
						where p.Oib = :oib";
			return Session.CreateQuery(hql)
				.SetString("oib", oib)
				.UniqueResult<Person>();
		}

		/// <summary>
		/// Dohvaca pravnu osobu preko identifikatora
		/// </summary>
		/// <param name="id">identifikator osobe</param>
		/// <returns>pravna osoba</returns>
		public LegalPerson GetLegalPersonById(int id) {
			return Session.Get<LegalPerson>(id);
		}

		/// <summary>
		/// Dohvaca fizicku osobu preko identifikatora
		/// </summary>
		/// <param name="id">id</param>
		/// <returns>fizicka osoba</returns>
		public PhysicalPerson GetPhysicalPersonById(int id) {
			return Session.Get<PhysicalPerson>(id);
		}

		/// <summary>
		/// Dohvaca pravnu osobu preko korisnickog imena
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>pravna osoba</returns>
		public LegalPerson GetLegalPersonByUsername(string username) {
			var hql = @"select person from HousingMgmtUser user
					join user.Person person
					where user.UserName = :username";
			return Session.CreateQuery(hql)
			       	.SetString("username", username)
			       	.UniqueResult<LegalPerson>();
		}

		/// <summary>
		/// Dohvaca fizičku osobu preko korisnickog imena
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>fizička osoba</returns>
		public PhysicalPerson GetPhysicalPersonByUsername(string username) {
			var hql = @"select person from HousingMgmtUser user
					join user.Person person
					where user.UserName = :username";
			return Session.CreateQuery(hql)
					.SetString("username", username)
					.UniqueResult<PhysicalPerson>();
		}

		/// <summary>
		/// Dohvaca osobu preko korisnickog imena
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <returns>osoba</returns>
		public Person GetPersonByUsername(string username) {
			var hql = @"select person from HousingMgmtUser user
					join user.Person person
					where user.UserName = :username";
			return Session.CreateQuery(hql)
					.SetString("username", username)
					.UniqueResult<Person>();
		}

		/// <summary>
		/// Dohvaca sve stanare zgrade
		/// </summary>
		/// <param name="buildingId">dohvaca sve stanare zgrade</param>
		/// <returns></returns>
		public IList<Person> GetTenantsFrom(int buildingId) {
			throw new NotImplementedException();
		}

		#endregion

	}
}
