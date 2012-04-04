using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.Finances {
	/// <summary>
	/// Repozitorij racuna
	/// </summary>
	public interface IBillsRepository : ICrudRepository<Bill> {

		/// <summary>
		/// Dohvaca sve racune izdane za zadanu osobu
		/// </summary>
		/// <param name="person">osoba za koju su izadni racuni</param>
		/// <returns>racuni izdani za zadanu osobu</returns>
		IList<Bill> GetBillsTo(Person person);

		/// <summary>
		/// Dohvaca sve racune izdane od strane zadane osobe
		/// </summary>
		/// <param name="legalPerson">osoba koja je izdala racune</param>
		/// <returns>racuni koje je izdala zadana osoba</returns>
		IList<Bill> GetBillsFrom(Person legalPerson);

		/// <summary>
		/// Dohvaca sve racune izdane od strane zadane osobe
		/// </summary>
		/// <param name="legalPerson">osoba koja je izdala racune</param>
		/// <param name="reserve">zgrada za koju su izdani racuni</param>
		/// <returns>racuni koje je izdala zadana osoba za zadanu zgradu</returns>
		IList<Bill> GetBillsFrom(Person legalPerson, Reserve reserve);

		/// <summary>
		/// Vraca indikaciju da li su izdani racuni pričuve za zadni mjesec i godinu
		/// </summary>
		/// <param name="reserve">pričuva za koju se izdaju računi</param>
		/// <param name="month">mjesec</param>
		/// <param name="year">godina</param>
		/// <returns>true ako jesu, inace false</returns>
		bool AreIssuedReserveBillsFor(Reserve reserve, int month, int year);

		/// <summary>
		/// Dohvaca periode izdavanje racuna za pricuvu
		/// </summary>
		/// <param name="reserve">pricuva</param>
		/// <returns>periodi(mjeseci) za koje postoje izdani racuni za placanje pricuve</returns>
		IList<DateTime> GetReservePeriods(Reserve reserve);

		/// <summary>
		/// Dohvaca izdane racune pricuve za zadani mjesec i godinu
		/// </summary>
		/// <param name="reserve">pricuva</param>
		/// <param name="month">mjesec</param>
		/// <param name="year">godina</param>
		/// <returns>racuni pricuve</returns>
		IList<Bill> GetIssuedReserveBillsFor(Reserve reserve, int month, int year);

		/// <summary>
		/// Dohvaca sve racune izdane za odredenu osobu
		/// </summary>
		/// <param name="person">osoba</param>
		/// <returns>racuni izdani za zadanu osobu</returns>
		IList<Bill> GetBillsIssuedFor(Person person);

		/// <summary>
		/// Dohvaca sve racune iz odredene pricuve izdane za odredenu
		/// </summary>
		/// <param name="person">osoba</param>
		/// <param name="reserve">pricuva</param>
		/// <returns>racuni iz odredne pricuve izdani za zadanu osobu</returns>
		IList<Bill> GetBillsIssuedFor(Person person, Reserve reserve);
	}
}
