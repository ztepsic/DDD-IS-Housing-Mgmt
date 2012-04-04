using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.BusinessRulesAndValidation;
using BuildingManagement.DomainModel.Finances;
using BuildingManagement.DomainModel.PersonsAndRoles;
using NUnit.Framework;

namespace BuildingManagement.DomainModel.Tests.Finances {
	[TestFixture]
	class BillTests {

		[Test]
		public void Can_Create_Bill() {
			// Arrange
			Person person = new PhysicalPerson("12345678901", "Mile", "Milic");
			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o") {
				NumberOfBankAccount = "123487815645"
			};

			// Act
			Bill bill = new Bill(person, legalPerson, 23);

			// Assert
			Assert.IsNotNull(bill);
			Assert.IsFalse(bill.IsPayed);

		}

		[Test]
		[ExpectedException(typeof(RulesException))]
		public void PersonFrom_Must_Have_Valid_Bank_Account_Number() {
			// Arrange
			Person person = new PhysicalPerson("12345678901", "Mile", "Milic");
			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o");

			// Act
			Bill bill = new Bill(person, legalPerson, 23);

			// Assert
		}

		[Test]
		public void Can_Calculate_Correct_Total_Amount_With_And_Without_Tax() {
			// Arrange
			Person person = new PhysicalPerson("12345678901", "Mile", "Milic");
			LegalPerson legalPerson = new LegalPerson("12345678901", "Mile d.o.o") {
				NumberOfBankAccount = "123487815645"
			};

			Bill bill = new Bill(person, legalPerson, 23);
			bill.AddBillItem(new BillItem(1, 23.5m, "Kruške"));
			bill.AddBillItem(new BillItem(3, 46.8m, "Jabuke"));

			// Act
			var totalAmount = bill.TotalAmount;
			var totalAmountWithTax = bill.TotalAmountWithTax;
			

			// Assert
			Assert.AreEqual(163.9m, totalAmount);
			Assert.AreEqual(201.597m, totalAmountWithTax);
		}

	}
}
