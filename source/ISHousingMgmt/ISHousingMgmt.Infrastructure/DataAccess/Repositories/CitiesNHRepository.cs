using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories {
	public class CitiesNHRepository : NHibernateReadOnlyRepository<City>, ICitiesRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public CitiesNHRepository() { }


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public CitiesNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Members

		public override IEnumerable<City> GetAll() {
			return Session.QueryOver<City>()
				.OrderBy(c => c.Name).Asc
				.List();
		}

		/// <summary>
		/// Dohvaca grad preko postanskog broja
		/// </summary>
		/// <param name="postalCode">postanski broj grada</param>
		/// <returns></returns>
		public City GetCityByPostalCode(int postalCode) {
			return Session.QueryOver<City>()
				.Where(c => c.PostalCode == postalCode)
				.SingleOrDefault();
		}

		/// <summary>
		/// Dohvaca gradove za koje postoje katastarske opcine
		/// </summary>
		/// <returns>gradovi za koje postoje katastarske opcine</returns>
		public IList<City> GetCitiesWithCadastres() {
			var hql = @"select distinct city from Cadastre cadastre
				join cadastre.City city";

			return Session.CreateQuery(hql)
				.List<City>();
		}

		#endregion
	}
}
