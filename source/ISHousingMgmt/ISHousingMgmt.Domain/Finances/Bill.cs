using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.Finances {
	/// <summary>
	/// Razred(Entity) koji predstavlja racun za pricuvu
	/// </summary>
	public class Bill : NHibernateEntity {

		#region Members

		/// <summary>
		/// Pričuva zgrade
		/// </summary>
		private Reserve reserve;

		/// <summary>
		/// Dohvaca pričuvu zgrade
		/// </summary>
		public virtual Reserve Reserve { get { return reserve; } }

		/// <summary>
		/// Dohvaca naziv zgrade
		/// </summary>
		public virtual string BuildingName { get { return reserve.Building.Address.ToString(); } }

		/// <summary>
		/// Datum izadavnja racuna
		/// </summary>
		private DateTime dateTimeIssued;

		/// <summary>
		/// Dohvaca datum izadavanja racuna
		/// </summary>
		public virtual DateTime DateTimeIssued { get { return dateTimeIssued; } }

		/// <summary>
		/// Postotak poreza
		/// </summary>
		private short tax;

		/// <summary>
		/// Dohvaca postotak poreza
		/// </summary>
		public virtual short Tax { get { return tax; } }

		/// <summary>
		/// Pravna osoba koja je izdala racun
		/// </summary>
		private LegalPersonSnapshot from;

		/// <summary>
		/// Dohvaca pravnu osobu koja je izdala racun
		/// </summary>
		public virtual LegalPersonSnapshot From { get { return from; } }

		/// <summary>
		/// Osoba koja placa racun
		/// </summary>
		private PersonSnapshot to;

		/// <summary>
		/// Dohvaca osobu koja placa racun
		/// </summary>
		public virtual PersonSnapshot To { get { return to; } }

		/// <summary>
		/// Poziv na broj zaduzenja
		/// </summary>
		public virtual string ReferenceNumber { get; set; }

		/// <summary>
		/// Stavke racuna
		/// </summary>
		private IList<BillItem> billItems;

		/// <summary>
		/// Dohvaca stavke racuna
		/// </summary>
		public virtual IList<BillItem> BillItems { get { return new ReadOnlyCollection<BillItem>(billItems); } }

		/// <summary>
		/// Puni iznos racuna bez poreza
		/// </summary>
		public virtual decimal TotalAmount { get { return billItems.Sum(x => x.TotalAmount); } }

		/// <summary>
		/// Iznos poreza za platiti
		/// </summary>
		/// <returns></returns>
		public virtual decimal TaxAmount { get { return (TotalAmount * tax) / 100m; } }

		/// <summary>
		/// Puni iznos racuna sa porezom
		/// </summary>
		public  virtual decimal TotalAmountWithTax { get { return calculateTotalAmountWithTax(); } }

		/// <summary>
		/// Datum i vrijeme kada je racun placen
		/// </summary>
		private DateTime? paidDateTime;

		/// <summary>
		/// Dohvaca datum i vrijeme kada je racun placen
		/// </summary>
		public virtual DateTime? PaidDateTime { get { return paidDateTime; } }

		/// <summary>
		/// Da li je racun placen
		/// </summary>
		private bool isPaid;

		/// <summary>
		/// Da li je racun placen
		/// </summary>
		public virtual bool IsPaid { get { return isPaid; } }

		/// <summary>
		/// Opis plaćanja
		/// </summary>
		private string paymentDescription;

		/// <summary>
		/// Dohvaca opis plaćanja
		/// </summary>
		public virtual string PaymentDescription { get { return paymentDescription; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Defaultni kontruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected Bill() { }

		
		/// <summary>
		/// Konstruktor (izdaje pravna osoba za pričuvu/zgradu)
		/// </summary>
		/// <param name="legalPersonFrom">pravna osoba koja šalje račun za zgradu</param>
		/// <param name="reserve">Pričuva</param>
		/// <param name="paymentDescription">opis plaćanja</param>
		/// <param name="tax">porez/PDV</param>
		public Bill(LegalPerson legalPersonFrom, Reserve reserve, string paymentDescription, short tax) {
			if (String.IsNullOrEmpty(legalPersonFrom.NumberOfBankAccount)) {
				throw new BusinessRulesException("Legal Person has not valid bank account number.");
			}

			to = null;
			this.reserve = reserve;
			from = new LegalPersonSnapshot(legalPersonFrom);
			this.paymentDescription = paymentDescription;
			this.tax = tax;
			dateTimeIssued = DateTime.Now;
			isPaid = false;

			billItems = new List<BillItem>();
		}

		/// <summary>
		/// Konstruktor (izdaje pričuva/zgrada za osobu)
		/// </summary>
		/// <param name="reserve">pričuva</param>
		/// <param name="personTo">osoba koja placa racun</param>
		/// <param name="paymentDescription">opis placanja</param>
		/// <param name="tax">porez/PDV</param>
		public Bill(Reserve reserve, Person personTo, string paymentDescription, short tax) {
			from = null;
			this.reserve = reserve;
			to = new PersonSnapshot(personTo);
			this.paymentDescription = paymentDescription;
			this.tax = tax;
			dateTimeIssued = DateTime.Now;
			isPaid = false;

			billItems = new List<BillItem>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Postavlja stanje racuna na placeno
		/// </summary>
		public virtual void SetPaid() {
			isPaid = true;
			paidDateTime = DateTime.Now;
		}

		/// <summary>
		/// Izracunava sveukupnu cijenu sa porezom
		/// </summary>
		/// <returns></returns>
		private decimal calculateTotalAmountWithTax() {
			decimal totalAmountWithTax = 0;

			decimal totalAmount = TotalAmount;
			totalAmountWithTax = totalAmount * ((100 + tax) / 100m);

			return totalAmountWithTax;
		}

		/// <summary>
		/// Dodavanje stavke racuna
		/// </summary>
		/// <param name="billItem">stavka racuna koja se dodaje</param>
		public virtual void AddBillItem(BillItem billItem) {
			billItems.Add(billItem);
		}

		/// <summary>
		/// Brisanje stavke racuna
		/// </summary>
		/// <param name="billItem">stavka racuna koja se brise</param>
		public virtual void RemoveBillItem(BillItem billItem) {
			billItems.Remove(billItem);
		}

		#endregion

	}
}
