using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.NHibernate {
	/// <summary>
	/// Bazni NHibernate repozitorij
	/// Sadrzi NHibernate sjednicu.
	/// </summary>
	public abstract class NHibernateRepository {

		#region Members

		/// <summary>
		/// Tvornica sjednice
		/// </summary>
		private readonly ISessionFactory sessionFactoryFactory;

		/// <summary>
		/// NHibernate sjednica
		/// </summary>
		protected ISession Session { get { return sessionFactoryFactory.GetCurrentSession(); } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		protected NHibernateRepository() : this(NHibernateSessionProvider.SessionFactory) {}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactoryFactoryFactoryFactory">NHibernate tvornica sjednice</param>
		protected NHibernateRepository(ISessionFactory sessionFactoryFactoryFactoryFactory) {
			this.sessionFactoryFactory = sessionFactoryFactoryFactoryFactory;
		}

		#endregion

		#region Methods
		#endregion

	}



}
