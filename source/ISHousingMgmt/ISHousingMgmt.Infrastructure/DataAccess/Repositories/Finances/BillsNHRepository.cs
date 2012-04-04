using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.Finances {
	public class BillsNHRepository : NHibernateCrudRepository<Bill>, IBillsRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public BillsNHRepository() { }


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public BillsNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca sve racune izdane za zadanu osobu
		/// </summary>
		/// <param name="person">osoba za koju su izadni racuni</param>
		/// <returns>racuni izdani za zadanu osobu</returns>
		public IList<Bill> GetBillsTo(Person person) {
			return Session.QueryOver<Bill>()
				.Where(b => b.To.Oib == person.Oib)
				.OrderBy(b => b.DateTimeIssued).Desc
				.List();
		}

		/// <summary>
		/// Dohvaca sve racune izdane od strane zadane osobe
		/// </summary>
		/// <param name="legalPerson">osoba koja je izdala racune</param>
		/// <returns>racuni koje je izdala zadana osoba</returns>
		public IList<Bill> GetBillsFrom(Person legalPerson) {
			return Session.QueryOver<Bill>()
				.Where(b => b.From.Oib == legalPerson.Oib)
				.OrderBy(b => b.DateTimeIssued).Desc
				.List();
		}

		/// <summary>
		/// Dohvaca sve racune izdane od strane zadane osobe
		/// </summary>
		/// <param name="legalPerson">osoba koja je izdala racune</param>
		/// <param name="reserve">zgrada za koju su izdani racuni</param>
		/// <returns>racuni koje je izdala zadana osoba za zadanu zgradu</returns>
		public IList<Bill> GetBillsFrom(Person legalPerson, Reserve reserve) {
			return Session.QueryOver<Bill>()
				.Where(b => b.From.Oib == legalPerson.Oib)
				.And(b => b.Reserve == reserve)
				.OrderBy(b => b.DateTimeIssued).Desc
				.List();
		}

		/// <summary>
		/// Vraca indikaciju da li su izdani racuni pričuve za zadni mjesec i godinu
		/// </summary>
		/// <param name="reserve">pričuva za koju se izdaju računi</param>
		/// <param name="month">mjesec</param>
		/// <param name="year">godina</param>
		/// <returns>true ako jesu, inace false</returns>
		public bool AreIssuedReserveBillsFor(Reserve reserve, int month, int year) {
			var hql = @"select count(*) from Bill b
				where b.Reserve = :reserve and 
					month(b.DateTimeIssued) = :month and
					year(b.DateTimeIssued) = :year and
					b.To.Oib is not null";

			long resultCount = Session.CreateQuery(hql)
				.SetEntity("reserve", reserve)
				.SetInt32("month", month)
				.SetInt32("year", year)
				.UniqueResult<long>();

			return resultCount > 0;
		}

		/// <summary>
		/// Dohvaca periode izdavanje racuna za pricuvu
		/// </summary>
		/// <param name="reserve">pricuva</param>
		/// <returns>periodi(mjeseci) za koje postoje izdani racuni za placanje pricuve</returns>
		public IList<DateTime> GetReservePeriods(Reserve reserve) {
			var hql = @"select distinct month(b.DateTimeIssued), year(b.DateTimeIssued)
				from Bill b
				where b.Reserve = :reserve and
				b.To.Oib is not null
				order by month(b.DateTimeIssued) desc, year(b.DateTimeIssued)";

			var results = Session.CreateQuery(hql)
				.SetEntity("reserve", reserve)
				.List();

			IList<DateTime> dateTimes = new List<DateTime>();
			foreach (Object[] result in results) {
				dateTimes.Add(new DateTime((int)result[1], (int)result[0], 1));
			}

			return dateTimes;

		}

		/// <summary>
		/// Dohvaca izdane racune pricuve za zadani mjesec i godinu
		/// </summary>
		/// <param name="reserve">pricuva</param>
		/// <param name="month">mjesec</param>
		/// <param name="year">godina</param>
		/// <returns>racuni pricuve</returns>
		public IList<Bill> GetIssuedReserveBillsFor(Reserve reserve, int month, int year) {
			var hql = @"from Bill b
				where b.Reserve = :reserve and 
					month(b.DateTimeIssued) = :month and
					year(b.DateTimeIssued) = :year and
					b.To.Oib is not null";

			return Session.CreateQuery(hql)
				.SetEntity("reserve", reserve)
				.SetInt32("month", month)
				.SetInt32("year", year)
				.List<Bill>();
		}

		/// <summary>
		/// Dohvaca sve racune izdane za odredenu osobu
		/// </summary>
		/// <param name="person">osoba</param>
		/// <returns>racuni izani za zadanu osobu</returns>
		public IList<Bill> GetBillsIssuedFor(Person person) {
			return Session.QueryOver<Bill>()
				.Where(b => b.To.Oib == person.Oib)
				.OrderBy(b => b.DateTimeIssued).Desc
				.List();
		}

		/// <summary>
		/// Dohvaca sve racune iz odredene pricuve izdane za odredenu
		/// </summary>
		/// <param name="person">osoba</param>
		/// <param name="reserve">pricuva</param>
		/// <returns>racuni iz odredne pricuve izdani za zadanu osobu</returns>
		public IList<Bill> GetBillsIssuedFor(Person person, Reserve reserve) {
			return Session.QueryOver<Bill>()
				.Where(b => b.To.Oib == person.Oib)
				.And(b => b.Reserve == reserve)
				.OrderBy(b => b.DateTimeIssued).Desc
				.List();
		}

		#endregion

	}
}
