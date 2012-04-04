using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.Finances {
	/// <summary>
	/// Razred(Factory and Service) za izradu racuna za placanje pricuve
	/// </summary>
	public static class ReserveBilling {

		#region Members

		#endregion

		#region Constructors and Init

		#endregion

		#region Methods

		public static bool IssueMonthlyReserveBills(Reserve reserve, IBillsRepository billsRepository) {
			var currentDateTime = DateTime.Now;
			bool areIssuedReserveBillsFor = billsRepository.AreIssuedReserveBillsFor(reserve, currentDateTime.Month,
			                                                                         currentDateTime.Year);

			if(!areIssuedReserveBillsFor) {
				if(reserve.Building.LandRegistry != null && reserve.Building.LandRegistry.Locked) {
					var partitionSpaces = reserve.Building.LandRegistry.OwnedPartitionSpaces;
					
						foreach (var partitionSpace in partitionSpaces) {
							reserve.IssueReserveBillFor(partitionSpace, 23);
						}

					return true;
				}
			}

			return false;

		}

		#endregion

	}
}
