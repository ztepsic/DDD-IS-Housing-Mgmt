using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate {
	public abstract class BaseFixture {

		#region Members

		protected static ILog log = new Func<ILog>(() => {
		                                           	log4net.Config.XmlConfigurator.Configure();
		                                           	return LogManager.GetLogger(typeof (BaseFixture));
		                                           }).Invoke();

		protected virtual void OnFixtureSetup() { }
		protected virtual void OnFixtureTeardown() { }
		protected virtual void OnSetup() { }
		protected virtual void OnTeardown() { }


		#endregion

		#region Constructors and Init

		#endregion

		#region Methods

		[TestFixtureSetUp]
		public void FixtureSetup() {
			OnFixtureSetup();
		}

		[TestFixtureTearDown]
		public void FixtureTearDown() {
			OnFixtureTeardown();
		}

		[SetUp]
		public void Setup() {
			OnSetup();
		}

		[TearDown]
		public void TearDown() {
			OnTeardown();
		}
		#endregion

	}
}
