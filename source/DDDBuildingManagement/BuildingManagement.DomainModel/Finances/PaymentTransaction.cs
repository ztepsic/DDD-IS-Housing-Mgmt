using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.Finances {
	/// <summary>
	/// Razred(Value Object) koji predstavlja jednu stavku transakcije
	/// </summary>
	public class PaymentTransaction {

		#region Members


		/// <summary>
		/// Racun za kojeg se uplacuje ili isplacuje
		/// </summary>
		private readonly Bill bill;

		/// <summary>
		/// Svota novca
		/// </summary>
		private readonly decimal amountOfMoney;

		/// <summary>
		/// Datum transackije
		/// </summary>
		private readonly DateTime dateTime;

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="bill">racun za kojeg se uplacuje ili isplacuje</param>
		/// <param name="amountOfMoney">svota novca</param>
		public PaymentTransaction(Bill bill, decimal amountOfMoney) {
			this.bill = bill;
			this.amountOfMoney = amountOfMoney;
			dateTime = DateTime.Now;
		}

		#endregion

		#region Methods

		#endregion

	}
}
