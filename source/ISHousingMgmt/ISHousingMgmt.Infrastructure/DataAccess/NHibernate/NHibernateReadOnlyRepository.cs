using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using NHibernate;
using NHibernate.Criterion;

namespace ISHousingMgmt.Infrastructure.DataAccess.NHibernate {

	/// <summary>
	/// NHibernate read-only repozitorij (samo za citanje).
	/// </summary>
	/// <typeparam name="TEntity">tip Entitet/Agregat</typeparam>
	public class NHibernateReadOnlyRepository<TEntity> :
		NHibernateReadOnlyRepository<TEntity, int>,
		IReadOnlyRepository<TEntity> where TEntity : Entity {

		/// <summary>
		/// Konstruktor
		/// </summary>
		public NHibernateReadOnlyRepository() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednice</param>
		public NHibernateReadOnlyRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }
	}

	/// <summary>
	/// NHibernate read-only repozitorij (samo za citanje).
	/// </summary>
	/// <typeparam name="TEntity">tip Entitet/Agregat</typeparam>
	/// <typeparam name="TId">tip identifikatora agregata</typeparam>
	public class NHibernateReadOnlyRepository<TEntity, TId> :
		NHibernateRepository, IReadOnlyRepository<TEntity, TId> where TEntity : Entity<TId> {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public NHibernateReadOnlyRepository() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactoryFactoryFactoryFactory">NHibernate tvornica sjednica</param>
		public NHibernateReadOnlyRepository(ISessionFactory sessionFactoryFactoryFactoryFactory) : base(sessionFactoryFactoryFactoryFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca sve perzistirane entitete/agregate
		/// </summary>
		/// <returns>Svi perzistirani entiteti/agregati</returns>
		public virtual IEnumerable<TEntity> GetAll() {
			ICriteria criteria = Session.CreateCriteria(typeof (TEntity));
			return criteria.List<TEntity>();
		}

		/// <summary>
		/// Dohvaca entitet/agregat na temelju njegovog identifikatora
		/// </summary>
		/// <param name="id">identifikator entiteta/agregata</param>
		/// <returns>entitet/agregat</returns>
		public virtual TEntity GetById(TId id) {
			// get vraca null ukoliko ne nade objekt, load baca exception
			return Session.Get<TEntity>(id);
		}

		#endregion

	}
}
