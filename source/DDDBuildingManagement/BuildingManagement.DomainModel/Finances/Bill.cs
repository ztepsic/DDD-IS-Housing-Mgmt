using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.BusinessRulesAndValidation;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.Finances {
	/// <summary>
	/// Razred(Entity) koji predstavlja racun za pricuvu
	/// </summary>
	public class Bill : EntityBase {

		#region Members

		/// <summary>
		/// Datum izadavnja racuna
		/// </summary>
		private readonly DateTime dateTimeIssued;

		/// <summary>
		/// Dohvaca datum izadavanja racuna
		/// </summary>
		public DateTime DateTimeIssued {
			get { return dateTimeIssued; }
		}

		/// <summary>
		/// Postotak poreza
		/// </summary>
		private short tax;

		/// <summary>
		/// Tko je izdao racun
		/// </summary>
		public LegalPersonSnapshot From { get; set; }

		/// <summary>
		/// Za koga je izdan racun
		/// </summary>
		public PersonSnapshot To { get; set; }

		/// <summary>
		/// Poziv na broj zaduzenja
		/// </summary>
		public string ReferenceNumber { get; set; }

		/// <summary>
		/// Stavke racuna
		/// </summary>
		private readonly IList<BillItem> billItems;

		/// <summary>
		/// Dohvaca stavke racuna
		/// </summary>
		public IList<BillItem> BillItems {
			get { return new ReadOnlyCollection<BillItem>(billItems); }
		}

		/// <summary>
		/// Puni iznos racuna bez poreza
		/// </summary>
		public decimal TotalAmount {
			get { return billItems.Sum(x => x.TotalAmount); }
		}

		/// <summary>
		/// Puni iznos racuna sa porezom
		/// </summary>
		public decimal TotalAmountWithTax {
			get { return calculateTotalAmountWithTax(); }
		}

		/// <summary>
		/// Da li je racun placen
		/// </summary>
		private bool isPayed;

		/// <summary>
		/// Da li je racun placen
		/// </summary>
		public bool IsPayed {
			get { return isPayed; }
			set { isPayed = value; }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="personTo">osoba koja mora platiti</param>
		/// <param name="legalPersonFrom">osoba kojoj se placa</param>
		/// <param name="tax">porez</param>
		public Bill(Person personTo, LegalPerson legalPersonFrom, short tax) {
			if(String.IsNullOrEmpty(legalPersonFrom.NumberOfBankAccount)) {
				throw new RulesException("Legal Person has not valid bank account number.");
			}

			To = new PersonSnapshot(personTo);
			From = new LegalPersonSnapshot(legalPersonFrom);
			this.tax = tax;
			isPayed = false;
			dateTimeIssued = DateTime.Now;

			billItems = new List<BillItem>();
			
		}

		#endregion

		#region Methods

		/// <summary>
		/// Izracunava sveukupnu cijenu sa porezom
		/// </summary>
		/// <returns></returns>
		private decimal calculateTotalAmountWithTax() {
			decimal totalAmountWithTax = 0;

			decimal totalAmount = TotalAmount;
			totalAmountWithTax = totalAmount * ((100 + tax)/100m);

			return totalAmountWithTax;
		}

		/// <summary>
		/// Dodavanje stavke racuna
		/// </summary>
		/// <param name="billItem">stavka racuna koja se dodaje</param>
		public void AddBillItem(BillItem billItem) {
			billItems.Add(billItem);
		}

		/// <summary>
		/// Brisanje stavke racuna
		/// </summary>
		/// <param name="billItem">stavka racuna koja se brise</param>
		public void RemoveBillItem(BillItem billItem) {
			billItems.Remove(billItem);
		}

		#endregion
		
	}
}
