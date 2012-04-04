using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.Legislature;

namespace ISHousingMgmt.Domain.Finances {
	/// <summary>
	/// Razred(Entity) koji predstavlja pricuvu
	/// </summary>
	public class Reserve : NHibernateEntity {

		#region Members

		public const string REFERENCE_NUMBER_PREFIX = "333";

		/// <summary>
		/// Zgrada cija je pricuva
		/// </summary>
		private Building building;

		/// <summary>
		/// Dohvaca zgradu cija je pricuva
		/// </summary>
		public virtual Building Building { get { return building; } }

		/// <summary>
		/// Novac
		/// </summary>
		private decimal money;

		/// <summary>
		/// Trenutno stanje novca
		/// </summary>
		public virtual decimal Money { get { return money; } }

		/// <summary>
		/// Racuni pricuve
		/// </summary>
		private Iesi.Collections.Generic.ISet<Bill> reserveBills;		

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Defaultni kontruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected internal Reserve() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="building">zgrada</param>
		internal Reserve(Building building) : this(building, 0) { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="building">zgrada</param>
		/// <param name="money">novac</param>
		internal Reserve(Building building, decimal money) {
			this.building = building;
			this.money = money;
			reserveBills = new HashedSet<Bill>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Placanje racuna iz pricuve
		/// </summary>
		/// <param name="bill">racun</param>
		/// <returns></returns>
		public virtual bool PayBill(Bill bill) {
			var unpaidBills = GetUnpaidBills();
			if(!unpaidBills.Contains(bill)) {
				throw new BusinessRulesException("Navedeni račun nije za naplatu pričuvom zgrade.");
			}

			if (money < bill.TotalAmountWithTax) {
				return false;
			}

			money -= bill.TotalAmountWithTax;
			bill.SetPaid();

			return true;
		}

		/// <summary>
		/// Dodaje račun za naplatu
		/// </summary>
		/// <param name="bill">racun za naplatu</param>
		public virtual void AddBillForPayment(Bill bill) {
			// moguce je dodati samo one racune koje je izdala pravna osoba
			if(bill.From != null && bill.Reserve != null) {
				reserveBills.Add(bill);						
			} else {
				throw new BusinessRulesException("Za naplaćivanje računa iz pričuve moguće je dodati samo račune izdane od strane pravne osobe.");
			}
		}

		/// <summary>
		/// Izadaje racun za pricuvu
		/// </summary>
		/// <param name="partitionSpace">stan za kojieg se izdaje racun za placanje pricuve</param>
		/// <param name="tax">porez</param>
		/// <returns>racun za pricuvu</returns>
		public virtual void IssueReserveBillFor(IPartitionSpace partitionSpace, short tax) {
			if (partitionSpace.Owner == null) { throw new BusinessRulesException("Owner doesn't exist."); }
			if(!building.LandRegistry.PartitionSpaces.Contains(partitionSpace)) {
				throw new BusinessRulesException("Etaža ne pripada ovoj zgradi, onodno pričuvi.");
			}

			StringBuilder paymentDescriptionSb = new StringBuilder();
			paymentDescriptionSb.AppendLine(string.Format("Račun mjesečne pričuve za zgradu {0}.", building.Address));
			paymentDescriptionSb.AppendLine(string.Format("Temeljem stana broj uloška: {0}", partitionSpace.RegistryNumber));
			paymentDescriptionSb.AppendLine(string.Format("Površine {0}, i koeficijenta pričuve {1}.", partitionSpace.SurfaceArea, building.ReserveCoefficient));

			Bill bill = new Bill(this, partitionSpace.Owner, paymentDescriptionSb.ToString(), tax);
			decimal price = building.ReserveCoefficient * partitionSpace.SurfaceArea;
			BillItem billItem = new BillItem(1, price, "Mjesečna naknada za pričuvu");
			bill.AddBillItem(billItem);
			bill.ReferenceNumber = string.Format("{0}-{1}-{2}", REFERENCE_NUMBER_PREFIX, partitionSpace.Id, bill.DateTimeIssued.ToString("yyyy-MM-dd"));

			reserveBills.Add(bill);
		}

		/// <summary>
		/// Uplata novca dobivenog iz pricuve
		/// </summary>
		/// <param name="bill">racun</param>
		public virtual void ReceivePaymentFor(Bill bill) {
			// moze se samo naplatiti pricuva iz racuna za pricuvu, odnosno ako ga je izdala zgrada
			if (bill.Reserve != null && bill.To != null) {
				bill.SetPaid();
				money += bill.TotalAmountWithTax;
			} else {
				var businessEx = new BusinessRulesException<Reserve>();
				businessEx.AddErrorForModel("Nije moguce naplatiti pričuvu iz računa koji nije račun za pričuvu.");
			}

		}

		/// <summary>
		/// Dohvaca plaćene račune iz pričuve
		/// </summary>
		/// <returns>računi plaćeni novcem iz pričuve</returns>
		public virtual IList<Bill> GetPaidBills() {
			return reserveBills.Where(b => b.IsPaid && b.From != null).ToList();
		}

		/// <summary>
		/// Dohvaća račune koje je potrebno platiti novcem iz pričuve
		/// </summary>
		/// <returns>računi koje je potrebno platiti novcem iz pričuve</returns>
		public virtual IList<Bill> GetUnpaidBills() {
			return reserveBills.Where(b => b.IsPaid == false && b.From != null) .ToList();
		}

		#endregion

	}
}
