using System;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Context;

namespace ISHousingMgmt.Infrastructure.DataAccess.NHibernate {
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class NHibernateTransactionAttribute : ActionFilterAttribute {

		#region Members

		/// <summary>
		/// Redosljed atributa na stogu filter atributa. Sto visi broj
		/// jer je pozeljno da se otvaranje sjednice i transakcije odgodi do 
		/// posljednjeg trenutka.
		/// </summary>
		private const int ORDER_OF_ATTRIBUTE_IN_STACK_OF_ATTRIBUTES = 100;

		/// <summary>
		/// NHibernate tvornica sjednica - stvoriti samo jednu sjednicu
		/// za vrijeme trajanja aplikacije
		/// </summary>
		private readonly ISessionFactory sessionFactory;

		public bool RollbackOnModelStateError { get; set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public NHibernateTransactionAttribute() {
			Order = ORDER_OF_ATTRIBUTE_IN_STACK_OF_ATTRIBUTES;
			sessionFactory = NHibernateSessionProvider.SessionFactory;
			RollbackOnModelStateError = true;
		}

		#endregion

		#region Methods

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			ISession session = sessionFactory.OpenSession();
			session.BeginTransaction();
			CurrentSessionContext.Bind(session);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext) {
			ISession session = CurrentSessionContext.Unbind(sessionFactory);
			if (session != null) {
				try {
					var transaction = session.Transaction;
					if (transaction != null && transaction.IsActive) {
						if ((filterContext.Exception != null) && (!filterContext.ExceptionHandled) || shouldRollback(filterContext)) {
							session.Transaction.Rollback();
						} else {
							session.Transaction.Commit();
						}
					}
				} catch (HibernateException) {
					session.Transaction.Rollback();
				} finally {
					session.Close();
				}
			}
		}


		private bool shouldRollback(ControllerContext filterContext) {
			return RollbackOnModelStateError && !filterContext.Controller.ViewData.ModelState.IsValid;
		}

		#endregion

	}
}
