using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.Finances {
	/// <summary>
	/// Razred(Value Object) koji predstavlja stavku racuna
	/// </summary>
	public class BillItem {

		#region Members

		/// <summary>
		/// Kolicina
		/// </summary>
		private readonly int quantity;

		/// <summary>
		/// Dohvaca kolicinu
		/// </summary>
		public int Quantity {
			get { return quantity; }
		}

		/// <summary>
		/// Cijena
		/// </summary>
		private readonly decimal price;

		/// <summary>
		/// Dohvaca cijenu
		/// </summary>
		public decimal Price {
			get { return price; }
		}

		/// <summary>
		/// Opis
		/// </summary>
		private readonly string description;

		public string Description {
			get { return description; }
		}


		/// <summary>
		/// Ukupan iznos
		/// </summary>
		public decimal TotalAmount {
			get { return price * quantity; }
		}

		#endregion

		#region Constructors and Init

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

		/// <summary>
		/// Provjerava da li su dva objekta jednaka
		/// </summary>
		/// <param name="obj">objekt s kojim se usporeduje</param>
		/// <returns>true ukoliko su objekti jednaki, inace false</returns>
		public override bool Equals(object obj) {
			if(obj == null || GetType() == obj.GetType()) {
				return false;
			}

			BillItem billItem = (BillItem) obj;
			return price.Equals(billItem.Price) &&
			       quantity.Equals(billItem.Quantity) &&
				   description.Equals(billItem.Description);
		}

		/// <summary>
		/// Vraca hash code trenutnog objekta
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return price.GetHashCode() ^
			       quantity.GetHashCode() ^
			       description.GetHashCode();
		}

		#endregion

	}
}
