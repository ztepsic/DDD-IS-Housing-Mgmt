using log4net;
using NHibernate;
using NHibernate.Cfg;

namespace ISHousingMgmt.Infrastructure.DataAccess.NHibernate {
	public static class NHibernateSessionProvider {

		#region Members

		/// <summary>
		/// NHibernate tvornica sjednica
		/// </summary>
		private static readonly ISessionFactory sessionFactory;

		/// <summary>
		/// Dohvaca NHibernate tvornicu sjednica
		/// </summary>
		public static ISessionFactory SessionFactory { get { return sessionFactory; } }

		/// <summary>
		/// Dohvaca trenutnu sjednicu
		/// </summary>
		public static ISession CurrentSession { get { return sessionFactory.GetCurrentSession(); } }

		#endregion

		#region Constructors and Init

		static NHibernateSessionProvider() {
			ILog log = LogManager.GetLogger(typeof(NHibernateSessionProvider));
			log.Debug("Trying to create NHibernate session factory.");
			try {
				Configuration nhConfig = new Configuration().Configure();
				sessionFactory = nhConfig.BuildSessionFactory();
			} catch(HibernateException ex) {	
				log.Error("Can't create NHibernate session factory.", ex);
				throw;
			}
			
		}

		/// <summary>
		/// Metoda za inicijalizaciju NHibernate tvornice sjednica.
		/// Nista ne radi osim sto osigurava da je pozvan konstruktor
		/// </summary>
		public static void Init() { }

		#endregion
	}
}
