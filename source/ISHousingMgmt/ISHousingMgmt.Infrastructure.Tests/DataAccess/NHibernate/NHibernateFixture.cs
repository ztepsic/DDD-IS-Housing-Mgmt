using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate {
	public abstract class NHibernateFixture : BaseFixture {

		#region Members

		protected ISessionFactory SessionFactory {
			get { return NHibernateSessionProvider.SessionFactory; }
		}

		protected ISession Session {
			get { return SessionFactory.GetCurrentSession(); }
		}

		#endregion

		#region Constructors and Init

		protected NHibernateFixture() {
			//var captureProfilerOutput = new CaptureProfilerOutput(@"/path/to/profiler.exe");
			//captureProfilerOutput.StartListening();

			NHibernateProfiler.Initialize();

			// run code that uses NHibernate

			//Report report = captureProfilerOutput.StopAndReturnReport();

			

		}

		#endregion

		#region Methods

		protected override void OnSetup() {
			SetupNHibernateSession();
			base.OnSetup();
		}

		protected override void OnTeardown() {
			TearDownNHibernateSession();
			base.OnTeardown();
		}

		protected void SetupNHibernateSession() {
			TestConnectionProvider.CloseDatabase();
			setupContextualSession();
			buildSchema();
		}

		protected void TearDownNHibernateSession() {
			tearDownContextualSession();
			TestConnectionProvider.CloseDatabase();
		}

		private void setupContextualSession() {
			var session = SessionFactory.OpenSession();
			CurrentSessionContext.Bind(session);
		}

		private void tearDownContextualSession() {
			var sessionFactory = NHibernateSessionProvider.SessionFactory;
			var session = CurrentSessionContext.Unbind(sessionFactory);
			session.Close();
		}

		private void buildSchema() {
			var cfg = NHibernateSessionProvider.Configuration;
			var schemaExport = new SchemaExport(cfg);
			//schemaExport.Create(false, true);
			schemaExport.Execute(false, true, false, Session.Connection, null);
		}

		#endregion

	}
}
