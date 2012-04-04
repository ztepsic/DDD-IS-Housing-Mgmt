using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.Finances {
	/// <summary>
	/// Razred(Value Object) koji predstavlja stavku racuna
	/// </summary>
	public class BillItem : ValueObject {

		#region Members

		/// <summary>
		/// Kolicina
		/// </summary>
		private readonly int quantity;

		/// <summary>
		/// Dohvaca kolicinu
		/// </summary>
		public virtual int Quantity { get { return quantity; } }

		/// <summary>
		/// Cijena
		/// </summary>
		private readonly decimal price;

		/// <summary>
		/// Dohvaca cijenu
		/// </summary>
		public virtual decimal Price { get { return price; } }

		/// <summary>
		/// Opis
		/// </summary>
		private readonly string description;

		public virtual string Description { get { return description; } }

		/// <summary>
		/// Ukupan iznos
		/// </summary>
		public virtual decimal TotalAmount { get { return price * quantity; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		private BillItem() {
			quantity = 0;
			price = 0;
			description = null;
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="quantity">kolicina</param>
		/// <param name="price">cijena</param>
		/// <param name="description">opis</param>
		public BillItem(int quantity, decimal price, string description) {
			this.quantity = quantity;
			this.price = price;
			this.description = description;
		}

		#endregion

		#region Methods

		#endregion

	}
}
