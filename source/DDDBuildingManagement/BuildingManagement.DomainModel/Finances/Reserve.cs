using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.BuildingManagement;

namespace BuildingManagement.DomainModel.Finances {
	/// <summary>
	/// Razred(Entity) koji predstavlja pricuvu
	/// </summary>
	public class Reserve : EntityBase {

		#region Members

		/// <summary>
		/// Zgrada cija je pricuva
		/// </summary>
		private readonly Building building;

		/// <summary>
		/// Novac
		/// </summary>
		private decimal money;

		/// <summary>
		/// Trenutno stanje novca
		/// </summary>
		public decimal MoneyStatus {
			get { return money; }
		}

		/// <summary>
		/// Novcane transakcije
		/// </summary>
		private readonly List<PaymentTransaction> paymentTransactions;

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="building">zgrada</param>
		public Reserve(Building building) : this(building, 0) {}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="building">zgrada</param>
		/// <param name="money">novac</param>
		public Reserve(Building building, decimal money) {
			this.building = building;
			this.money = money;
			paymentTransactions = new List<PaymentTransaction>();
		}

		#endregion


		#region Methods

		/// <summary>
		/// Placanje racuna iz pricuve
		/// </summary>
		/// <param name="bill">racun</param>
		/// <returns></returns>
		public bool PayBill(Bill bill) {
			if(money < bill.TotalAmountWithTax) {
				return false;
			}

			money -= bill.TotalAmountWithTax;
			paymentTransactions.Add(new PaymentTransaction(bill, bill.TotalAmountWithTax));
			bill.IsPayed = true;

			return true;
		}

		/// <summary>
		/// Uplata novca dobivenog iz pricuve
		/// </summary>
		/// <param name="bill">racun</param>
		public void ReceivePaymentFor(Bill bill) {
			bill.IsPayed = true;
			money += bill.TotalAmountWithTax;
		}

		#endregion

	}
}
