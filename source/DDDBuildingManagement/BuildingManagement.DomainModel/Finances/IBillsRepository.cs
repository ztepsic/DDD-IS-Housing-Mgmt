using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.Finances {
	/// <summary>
	/// Repozitorij racuna
	/// </summary>
	public interface IBillsRepository : ICrudRepository<Bill> {
		/// <summary>
		/// Dohvaca racun za danu godinu, mjesec, poziv na broj i id pravne osobe koja je izdala racun
		/// </summary>
		/// <param name="year">godina</param>
		/// <param name="month">mjesec</param>
		/// <param name="referenceNumber">poziv na broj</param>
		/// <param name="legalPersonId">id pravne osobe</param>
		/// <returns>racun</returns>
		Bill GetBillFor(int year, int month, string referenceNumber, object legalPersonId);
	}
}
