using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using ISHousingMgmt.Web.App_Start;
using NUnit.Framework;

namespace ISHousingMgmt.Web.Tests.AutoMapper {
	[TestFixture]
	public class AutoMapperConfigurationTests {

		[Test]
		public void TestMappings() {
			AutoMapperBootstrapper.Bootstrap();
			Mapper.AssertConfigurationIsValid();
		}
		
	}
}
