using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using NUnit.Framework;

namespace ISHousingMgmt.Domain.Tests.Abstractions {
	[TestFixture]
	class ValueObjectTests {

		[Test]
		public void Compare_Two_Equal_ValueObjects_As_Equal_By_Comparing_All_Properties() {
			// Arrange
			ValueObject valueObject1 = new ValueObjectFake("Ulica 1", "Zagreb");
			ValueObject valueObject2 = new ValueObjectFake("Ulica 1", "Zagreb");

			// Act

			// Assert
			Assert.AreEqual(valueObject1, valueObject2, "ValueObjects aren't equal.");
			Assert.AreEqual(valueObject1.GetHashCode(), valueObject2.GetHashCode(), "HashCodes of ValueObjects aren't equal.");
		}

		[Test]
		public void Two_ValueObjects_Which_Have_The_Same_Reference_Are_Always_Equal() {
			// Arrange
			ValueObject valueObject1 = new ValueObjectFake("Ulica 1", "Zagreb");
			ValueObject valueObject12 = valueObject1;

			// Act

			// Assert
			Assert.AreEqual(valueObject1, valueObject12, "Two ValueObjects aren't equal.");
			Assert.AreEqual(valueObject1.GetHashCode(), valueObject12.GetHashCode(), "Hashcodes of ValueObjects aren't equal.");
			Assert.AreSame(valueObject1, valueObject12, "Two ValueObjects haven't the same reference.");
		}

		[Test]
		public void Compare_ValueObject_With_Null_As_NotEqual() {
			// Arrange
			ValueObject valueObject1 = new ValueObjectFake("Ulica 1", "Zagreb");

			ValueObject valueObject2 = null;
			
			// Act

			// Assert
			Assert.AreNotEqual(valueObject1, valueObject2, "Two ValueObjects aren't equal.");
		}

		[Test]
		public void Compare_ValueObject_And_Object_As_NotEqual() {
			// Arrange
			ValueObject valueObject1 = new ValueObjectFake("Ulica 1", "Zagreb");

			string obj = "Test";

			// Act

			// Assert
			Assert.AreNotEqual(valueObject1, obj, "Two object aren't equal.");
			Assert.AreNotEqual(valueObject1.GetHashCode(), obj.GetHashCode(), "Two objects have different hashcodes.");
		}

		[Test]
		public void Compare_Two_ValueObjects_Of_Diffrent_Type_Of_Hierarchy_As_NotEqual() {
			// Arrange
			ValueObject valueObject1 = new ValueObjectFake("Ulica 1", "Zagreb");

			ValueObject valueObject2 = new ValueObjectFake2("Ulica 1", "Zagreb", "Croatia");

			// Act

			// Assert
			Assert.AreNotEqual(valueObject1, valueObject2, "Two ValueObjects aren't equal.");
			Assert.AreNotEqual(valueObject1.GetHashCode(), valueObject2.GetHashCode(), "Two ValueObjects have different hashcodes.");
		}

		[Test]
		public void Compare_Two_Equal_Entites_One_As_SubClass_One_Cast_As_BaseClass_As_Equal() {
			// Arrange
			ValueObjectFake2 valueObject1 = new ValueObjectFake2("Ulica 1", "Zagreb", "Croatia");

			ValueObjectFake2 valueObject2 = new ValueObjectFake2("Ulica 1", "Zagreb", "Croatia");
			ValueObjectFake valueObject22 = valueObject2 as ValueObjectFake;

			ValueObjectFake valueObject3 = new ValueObjectFake("Ulica 1", "Zagreb");

			// Act

			// Assert
			Assert.AreEqual(valueObject1, valueObject22, "Two ValueObjects aren't equal.");
			Assert.AreEqual(valueObject1.GetHashCode(), valueObject22.GetHashCode(), "Two ValueObjects haven't the same hashcodes.");
			Assert.AreEqual(typeof(ValueObjectFake2), valueObject1.GetUnproxiedType(), "Types are not equal.");
			Assert.AreEqual(typeof(ValueObjectFake2), valueObject22.GetUnproxiedType(), "Types are not equal.");

			Assert.AreNotEqual(valueObject1, valueObject3, "Two ValueObjects are equal.");
			Assert.AreNotEqual(valueObject1.GetHashCode(), valueObject3.GetHashCode(), "Two ValueObjects have the same hashcodes.");
			Assert.AreEqual(typeof(ValueObjectFake2), valueObject1.GetUnproxiedType(), "Types are not equal.");
			Assert.AreEqual(typeof(ValueObjectFake), valueObject3.GetUnproxiedType(), "Types are not equal.");
		}

		[Test]
		public void Compare_Two_NotEqual_ValueObjects_As_NotEqual_By_Operator() {
			// Arrange
			ValueObject valueObject1 = new ValueObjectFake("Ulica 1", "Zagreb");

			ValueObject valueObject2 = new ValueObjectFake("Ulica 2", "Zagreb");

			// Act
			var areNotEqual = valueObject1 != valueObject2;

			// Assert
			Assert.IsTrue(areNotEqual, "Operator != isn't working.");
			Assert.AreNotEqual(valueObject1, valueObject2, "Two ValueObjects are equal.");
			Assert.AreNotEqual(valueObject1.GetHashCode(), valueObject2.GetHashCode(), "Two ValueObjects have equal hashcodes.");
		}

		[Test]
		public void Compare_Two_Equal_Entites_As_Equal_By_Operator() {
			// Arrange
			ValueObject valueObject1 = new ValueObjectFake("Ulica 1", "Zagreb");

			ValueObject valueObject2 = new ValueObjectFake("Ulica 1", "Zagreb");

			// Act
			var areEqual = valueObject1 == valueObject2;

			// Assert
			Assert.IsTrue(areEqual, "Operator == isn't working.");
			Assert.AreEqual(valueObject1, valueObject2, "Two ValueObjects aren't equal.");
			Assert.AreEqual(valueObject1.GetHashCode(), valueObject2.GetHashCode(), "Two ValueObjects have different hashcodes.");
		}

	}
}
