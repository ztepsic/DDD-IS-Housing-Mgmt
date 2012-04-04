using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.Legislature {
	public class CadastresNHRepository : NHibernateReadOnlyRepository<Cadastre>, ICadastresRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public CadastresNHRepository() { }


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public CadastresNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion


		#region Methods

		/// <summary>
		/// Dohvaca katastar preko maticnog broja
		/// </summary>
		/// <param name="mbr">maticni broj katastra</param>
		/// <returns></returns>
		public Cadastre GetByMbr(string mbr) {
			return Session.QueryOver<Cadastre>()
				.Where(c => c.Mbr == mbr)
				.SingleOrDefault();
		}

		/// <summary>
		/// Dohvaca listu katastara za odredeni grad
		/// </summary>
		/// <param name="cityId">identifikator grada</param>
		/// <returns>lista katastara za odredeni grad</returns>
		public IList<Cadastre> GetByCity(int cityId) {
			return Session.QueryOver<Cadastre>()
				.Where(c => c.City.Id == cityId)
				.List();
		}

		#endregion

	}
}
