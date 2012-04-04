using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Legislature;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.Legislature {
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

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void NumberOfCadastralParticle_Can_Not_Be_Null_Or_Empty(){
			CadastralParticle cadastralParticle1 = new CadastralParticle(cadastre, "", 486, "Zgrada");
			CadastralParticle cadastralParticle2 = new CadastralParticle(cadastre, null, 486, "Zgrada");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Description_Of_CadastralParticle_Can_Not_Be_Null_Or_Empty() {
			CadastralParticle cadastralParticle1 = new CadastralParticle(cadastre, "123", 486, "");
			CadastralParticle cadastralParticle2 = new CadastralParticle(cadastre, "123", 486, null);
			cadastralParticle1.SetDescription(null);
			cadastralParticle2.SetDescription("");

		}


		[Test]
		public void If_CadastralParticles_Are_Composite_Then_Composite_Must_Return_Sum_Of_SurfaceArea_Of_Each_CadastralParticle_Component() {
			// Arrange
			CadastralParticle cadastralParticle1 = new CadastralParticle(cadastre, "4037/12", 486, "Stambena zgrada Ante Topica Mimare xx");
			CadastralParticle cadastralParticle2 = new CadastralParticle(cadastre, "4037/12", 879, "Dvoriste");
			CadastralParticleComposite cadastralParticleComposite = new CadastralParticleComposite(cadastre, "4037/12");
			cadastralParticleComposite.Add(cadastralParticle1);
			cadastralParticleComposite.Add(cadastralParticle2);

			AbstractCadastralParticle abstractCadastralParticle = cadastralParticleComposite;

			// Act
			var sumOfSurfaceAreas = abstractCadastralParticle.SurfaceArea;

			// Assert
			Assert.AreEqual(1365, sumOfSurfaceAreas);
		}

		[Test]
		public void If_CadastralParticles_Are_Composite_Then_Composite_Must_Return_Aggregated_Description_Of_Each_CadastralParticle_Component() {
			// Arange
			CadastralParticle cadastralParticle1 = new CadastralParticle(cadastre, "4037/12", 486, "Stambena zgrada Ante Topica Mimare xx");
			CadastralParticle cadastralParticle2 = new CadastralParticle(cadastre, "4037/12", 879, "Dvoriste");
			CadastralParticleComposite cadastralParticleComposite = new CadastralParticleComposite(cadastre, "4037/12");
			cadastralParticleComposite.Add(cadastralParticle1);
			cadastralParticleComposite.Add(cadastralParticle2);

			AbstractCadastralParticle abstractCadastralParticle = cadastralParticleComposite;

			// Act
			var result = abstractCadastralParticle.Description;

			// Assert
			Assert.AreEqual("Stambena zgrada Ante Topica Mimare xx i Dvoriste", result);


		}

	}
}
