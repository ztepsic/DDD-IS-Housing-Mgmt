using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.BuildingManagement;
using BuildingManagement.DomainModel.BusinessRulesAndValidation;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.Finances {
	/// <summary>
	/// Razred(Factory and Service) za izradu racuna za placanje pricuve
	/// </summary>
	public class ReserveBilling {

		#region Members

		/// <summary>
		/// Minimalni koeficijent pricuve zakonom odredeno
		/// </summary>
		public const decimal MINIMAL_LEGAL_RESERVE_COEFFICIENT = 1.53m;

		public const string REFERENCE_NUMBER_PREFIX = "333";

		/// <summary>
		/// Porez
		/// </summary>
		private readonly short tax;

		/// <summary>
		/// Upravitelj zgrade
		/// </summary>
		private readonly BuildingManager buildingManager;

		/// <summary>
		/// Koeficijent visine pricuve
		/// </summary>
		private readonly decimal reserveCoefficient;

		/// <summary>
		/// Dohvaca koeficijent visine pricuve
		/// </summary>
		public decimal ReserveCoefficient {
			get { return reserveCoefficient; }
		}


		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="reserveCoefficient">koeficijent visine pricuve</param>
		/// <param name="tax">porez</param>
		/// <param name="buildingManager">upravitelj zgrade</param>
		public ReserveBilling(decimal reserveCoefficient, short tax, BuildingManager buildingManager) {
			this.reserveCoefficient = reserveCoefficient;
			this.buildingManager = buildingManager;
			this.tax = tax;
		}

		/// <summary>
		/// Kontruktor
		/// </summary>
		/// <param name="tax">porez</param>
		/// <param name="buildingManager">upravitelj zgrade</param>
		public ReserveBilling(short tax, BuildingManager buildingManager) : this(MINIMAL_LEGAL_RESERVE_COEFFICIENT, tax, buildingManager) { }

		#endregion

		#region Methods

		/// <summary>
		/// Izadaje racun za pricuvu
		/// </summary>
		/// <param name="apartment">stan za kojieg se izdaje racun za placanje pricuve</param>
		/// <returns>racun za pricuvu</returns>
		public Bill IssueReserveBillFor(Apartment apartment) {
			if(apartment.ResponsibleTenant == null) {
				throw new RulesException("Responsible tenant is not set.");
			}

			Bill bill = new Bill(apartment.ResponsibleTenant, buildingManager.LegalPerson, tax);
			decimal price = reserveCoefficient * apartment.SurfaceArea;
			BillItem billItem = new BillItem(1, price, "Pričuva");
			bill.AddBillItem(billItem);
			bill.ReferenceNumber = REFERENCE_NUMBER_PREFIX + apartment.Id + bill.DateTimeIssued.ToShortDateString();

			return bill;
		}

		#endregion

	}
}
