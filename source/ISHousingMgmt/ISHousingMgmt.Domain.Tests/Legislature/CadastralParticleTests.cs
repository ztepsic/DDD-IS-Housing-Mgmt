using System;
using ISHousingMgmt.Domain.Legislature;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.Legislature {
	[TestFixture]
	class CadastralParticleTests {

		/// <summary>
		/// Katastar
		/// </summary>
		private Cadastre cadastre;

		[SetUp]
		public void SetUp() {
			cadastre = new Cadastre("Trešnjevka", "332134", new City(10000, "Zagreb"));
		}

	}
}
