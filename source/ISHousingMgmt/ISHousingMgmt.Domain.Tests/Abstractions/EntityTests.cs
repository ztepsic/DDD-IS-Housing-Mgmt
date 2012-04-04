using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using NUnit.Framework;
using ISHousingMgmt.Domain.Tests.Helpers;

namespace ISHousingMgmt.Domain.Tests.Abstractions {
	[TestFixture]
	class EntityTests {

		[Test]
		public void Compare_Entity_With_Null_As_NotEqual() {
			// Arrange
			EntityFake entity1 = new EntityFake("mica", "mile@gmail.com") { Age = 32 };
			entity1.SetIdTo(32);

			EntityFake entity2 = null;

			// Act

			// Assert
			Assert.AreNotEqual(entity1, entity2, "Two entites aren't equal.");
		}

		[Test]
		public void Compare_Entity_And_Object_As_NotEqual() {
			// Arrange
			EntityFake entity1 = new EntityFake("mica", "mile@gmail.com") { Age = 32 };
			entity1.SetIdTo(32);

			string obj ="Test";

			// Act

			// Assert
			Assert.AreNotEqual(entity1, obj, "Two objects aren't equal.");
			Assert.AreNotEqual(entity1.GetHashCode(), obj.GetHashCode(), "Two objects have different hashcodes.");
		}

		[Test]
		public void Compare_Two_Entites_Of_Diffrent_Type_Of_Hierarchy_As_NotEqual() {
			// Arrange
			EntityFake entity1 = new EntityFake("mica", "mile@gmail.com") { Age = 32 };
			entity1.SetIdTo(32);

			EntityFakeWithBusinessKey entity2 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") { Age = 32 };
			entity2.SetIdTo(32);

			// Act

			// Assert
			Assert.AreNotEqual(entity1, entity2, "Two entites aren't equal.");
			Assert.AreNotEqual(entity1.GetHashCode(), entity2.GetHashCode(), "Two entites have different hashcodes.");
		}

		[Test]
		public void Compare_Two_Equal_Entites_One_As_SubClass_One_Cast_As_BaseClass_As_Equal() {
			// Arrange
			EntityFakeWithBusinessKey2 entity1 = new EntityFakeWithBusinessKey2("mile", "mile@gmail.com") { Age = 32, Address = "Ulica"};
			entity1.SetIdTo(32);

			EntityFakeWithBusinessKey2 entity2 = new EntityFakeWithBusinessKey2("mile", "mile@gmail.com") { Age = 32, Address = "Ulica" };
			entity2.SetIdTo(32);

			EntityFakeWithBusinessKey entity22 = entity2 as EntityFakeWithBusinessKey;

			EntityFakeWithBusinessKey entity3 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") { Age = 32 };
			entity3.SetIdTo(32);

			// Act

			// Assert
			Assert.AreEqual(entity1, entity22, "Two entites aren't equal.");
			Assert.AreEqual(entity1.GetHashCode(), entity22.GetHashCode(), "Two entites haven't the same hashcodes.");
			Assert.AreEqual(typeof(EntityFakeWithBusinessKey2), entity1.GetUnproxiedType(), "Types are not equal.");
			Assert.AreEqual(typeof(EntityFakeWithBusinessKey2), entity22.GetUnproxiedType(), "Types are not equal.");

			Assert.AreNotEqual(entity1, entity3, "Two entites are equal.");
			Assert.AreNotEqual(entity1.GetHashCode(), entity3.GetHashCode(), "Two entites have the same hashcodes.");
			Assert.AreEqual(typeof(EntityFakeWithBusinessKey2), entity1.GetUnproxiedType(), "Types are not equal.");
			Assert.AreEqual(typeof(EntityFakeWithBusinessKey), entity3.GetUnproxiedType(), "Types are not equal.");
		}

		[Test]
		public void Compare_Two_NotEqual_Entites_As_NotEqual_By_Operator() {
			// Arrange
			Entity entity1 = new EntityFake("mile", "mile@gmail.com") { Age = 32 };
			entity1.SetIdTo(32);

			Entity entity2 = new EntityFake("mile", "mile@gmail.com") { Age = 6 };
			entity2.SetIdTo(6);

			// Act
			var areNotEqual = entity1 != entity2;

			// Assert
			Assert.IsTrue(areNotEqual, "Operator != isn't working.");
			Assert.AreNotEqual(entity1, entity2, "Two entites aren't equal.");
			Assert.AreNotEqual(entity1.GetHashCode(), entity2.GetHashCode(), "Two entites have equal hashcodes.");
		}

		[Test]
		public void Compare_Two_Equal_Entites_As_Equal_By_Operator() {
			// Arrange
			Entity entity1 = new EntityFakeWithBusinessKey("mica", "mile@gmail.com") { Age = 32 };
			entity1.SetIdTo(32);

			Entity entity2 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") { Age = 6 };
			entity2.SetIdTo(32);

			// Act
			var areEqual = entity1 == entity2;

			// Assert
			Assert.IsTrue(areEqual, "Operator == isn't working.");
			Assert.AreEqual(entity1, entity2, "Two entites aren't equal.");
			Assert.AreEqual(entity1.GetHashCode(), entity2.GetHashCode(), "Two entites have different hashcodes.");
		}

		/// <summary>
		/// Usporedba dva perzistentna objekta (imaju Id definiran) na osnovu njihovih
		/// dekoriranih svojstava sa temelju <see cref="BusinessKeyOfEntityAttribute"/>
		/// </summary>
		[Test]
		public void Compare_Two_Equal_Persisted_Enitites_With_BusinessKey_Properties_As_Equal_Only_By_Ids() {
			// Arrange
			Entity entity1 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") {Age = 32};
			entity1.SetIdTo(32);

			Entity entity2 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") {Age = 6};
			entity2.SetIdTo(32);

			// Act

			// Assert
			Assert.AreEqual(entity1, entity2, "Two entites aren't equal.");
			Assert.AreEqual(entity1.GetHashCode(), entity2.GetHashCode(), "Two entites have different haschodes.");
		}

		[Test]
		public void Compare_Two_Different_Transient_Enitites_As_NotEqual_Only_By_Theirs_BusinessKey_Properties() {
			// Arrange
			Entity entity1 = new EntityFakeWithBusinessKey("mila", "mila@hotmail.com") { Age = 32 };

			Entity entity2 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") { Age = 32 };

			// Act

			// Assert
			Assert.AreNotEqual(entity1, entity2, "Two entites are equal.");
			Assert.AreNotEqual(entity1.GetHashCode(), entity2.GetHashCode(), "Two hashcodes are equal.");
		}

		/// <summary>
		/// Usporedba dva perzistentna objekta (imaju Id definiran) na osnovu njihovih identifikatora
		/// </summary>
		[Test]
		public void Compare_Two_Equal_Persisted_Entites_As_Equal_Without_BusinessKey_Properties() {
			// Arrange
			Entity entity1 = new EntityFake("mica", "mile@gmail.com") { Age = 32 };
			entity1.SetIdTo(32);

			Entity entity2 = new EntityFake("mile", "mile@gmail.com") { Age = 6 };
			entity2.SetIdTo(32);

			// Act

			// Assert
			Assert.AreEqual(entity1, entity2, "Two entites aren't equal.");
			Assert.AreEqual(entity1.GetHashCode(), entity2.GetHashCode(), "Two entites have different hashcodes.");
		}

		[Test]
		public void Compare_Two_Different_Persisted_Entites_As_NotEqual_Without_BusinessKey_Properties() {
			// Arrange
			Entity entity1 = new EntityFake("mile", "mile@gmail.com") { Age = 32 };
			entity1.SetIdTo(32);

			Entity entity2 = new EntityFake("mile", "mile@gmail.com") { Age = 6 };
			entity2.SetIdTo(6);

			// Act

			// Assert
			Assert.AreNotEqual(entity1, entity2, "Two entites are equal.");
			Assert.AreNotEqual(entity1.GetHashCode(), entity2.GetHashCode(), "To hashcodes are equal.");
		}

		[Test]
		public void Two_Entity_Objects_Which_Have_The_Same_Reference_Are_Always_Equal() {
			// Arrange
			Entity entity1 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") { Age = 32 };
			entity1.SetIdTo(32);
			Entity entity12 = entity1;

			Entity entity2 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") { Age = 6 };
			Entity entity22 = entity2;

			Entity entity3 = new EntityFake("mile", "mile@gmail.com") { Age = 32 };
			entity1.SetIdTo(32);
			Entity entity32 = entity3;

			Entity entity4 = new EntityFake("mile", "mile@gmail.com") { Age = 6 };
			Entity entity42 = entity4;

			// Act

			// Assert
			Assert.AreEqual(entity1, entity12, "Two entites aren't equal.");
			Assert.AreEqual(entity1.GetHashCode(), entity12.GetHashCode(), "Two hashcodes aren't equal.");
			Assert.AreSame(entity1, entity12, "Two entites haven't the same reference.");

			Assert.AreEqual(entity2, entity22, "Two entites aren't equal.");
			Assert.AreEqual(entity2.GetHashCode(), entity22.GetHashCode(), "Two hashcodes aren't equal.");
			Assert.AreSame(entity2, entity22, "Two entites haven't the same reference.");

			Assert.AreEqual(entity3, entity32, "Two entites aren't equal.");
			Assert.AreEqual(entity3.GetHashCode(), entity32.GetHashCode(), "Two hashcodes aren't equal.");
			Assert.AreSame(entity3, entity32, "Two entites haven't the same reference.");

			Assert.AreEqual(entity4, entity42, "Two entites aren't equal.");
			Assert.AreEqual(entity4.GetHashCode(), entity42.GetHashCode(), "Two hashcodes aren't equal.");
			Assert.AreSame(entity4, entity42, "Two entites haven't the same reference.");
		}

		[Test]
		public void One_Entity_Is_Transient_One_Persistent__With_The_Same_Properties_They_Are_NotEqual() {
			// Arrange
			Entity entity1 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") { Age = 32 };
			entity1.SetIdTo(32);
			Entity entity12 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") { Age = 32 };

			Entity entity2 = new EntityFake("mile", "mile@gmail.com") {Age = 6};
			entity2.SetIdTo(6);
			Entity entity22 = new EntityFake("mile", "mile@gmail.com") { Age = 6 };

			// Act

			// Assert
			Assert.AreNotEqual(entity1, entity12, "Two entites are equal.");
			Assert.AreNotEqual(entity1.GetHashCode(), entity12.GetHashCode(), "Two hashcodes are equal.");

			Assert.AreNotEqual(entity2, entity22, "Two entites are equal.");
			Assert.AreNotEqual(entity2.GetHashCode(), entity22.GetHashCode(), "Two hashcodes are equal.");
		}

		[Test]
		public void Entity_Properties_Which_Arent_Decorated_Havent_Influance_In_Comparison() {
			// Arrange
			EntityFakeWithBusinessKey entity1 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") { Age = 32 };

			EntityFakeWithBusinessKey entity2 = new EntityFakeWithBusinessKey("mile", "mile@gmail.com") { Age = 6 };

			// Act

			// Assert
			Assert.AreEqual(entity1, entity2, "Two entites aren't equal.");
			Assert.AreEqual(entity1.GetHashCode(), entity2.GetHashCode(), "Two entites have different haschodes.");
			Assert.AreNotEqual(entity1.Age, entity2.Age);
		}
	}
}
