using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.Legislature {
	public class LandRegistriesNHRepository  : NHibernateCrudRepository<LandRegistry>, ILandRegistriesRepository {

		#region Members

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public LandRegistriesNHRepository() { }


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public LandRegistriesNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca zemljisnu knjigu na temelju broja katastarske cestice
		/// </summary>
		/// <param name="numberOfCadastralParticle">katastarska cestica</param>
		/// <returns>zemljisna knjiga</returns>
		public LandRegistry GetByNumberOfCadastralParticle(string numberOfCadastralParticle) {
			return Session.QueryOver<LandRegistry>()
				.Where(lr => lr.CadastralParticle.NumberOfCadastralParticle == numberOfCadastralParticle)
				.SingleOrDefault();
		}

		#endregion
	}
}
