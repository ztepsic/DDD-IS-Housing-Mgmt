using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.NHibernate {

	/// <summary>
	/// NHibernate repozitorij sa CRUD operacijama za entitete
	/// sa int identifikatorom entiteta/agregata.
	/// </summary>
	/// <typeparam name="TEntity">tip entiteta/agregata</typeparam>
	public class NHibernateCrudRepository<TEntity> :
		NHibernateCrudRepository<TEntity, int>,
		ICrudRepository<TEntity> where TEntity : Entity {

		/// <summary>
		/// Konstruktor
		/// </summary>
		public NHibernateCrudRepository() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactoryFactoryFactory">NHibernate tvornica sjednica</param>
		public NHibernateCrudRepository(ISessionFactory sessionFactoryFactoryFactory) : base(sessionFactoryFactoryFactory) { }
		
	}

	/// <summary>
	/// NHibernate repozitorij sa CRUD operacijama za entitete.
	/// </summary>
	/// <typeparam name="TEntity">tip entiteta/agregata</typeparam>
	/// <typeparam name="TId">tip identifikatora entiteta/agregata</typeparam>
	public class NHibernateCrudRepository<TEntity, TId> :
		NHibernateReadOnlyRepository<TEntity, TId>,
		ICrudRepository<TEntity, TId> where TEntity : Entity<TId> {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public NHibernateCrudRepository() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public NHibernateCrudRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Sprema novi ili azurira postojeci entitet/agregat iz repozitorija
		/// </summary>
		/// <param name="entity">entitet/agregat koji se sprema ili azurira</param>
		public void SaveOrUpdate(TEntity entity) {
			Session.SaveOrUpdate(entity);
		}

		/// <summary>
		/// Brise entitet/agregat iz repozitorija
		/// </summary>
		/// <param name="entity">enitet/agregat koji se brise</param>
		public void Delete(TEntity entity) {
			Session.Delete(entity);
		}

		#endregion


	}
}
