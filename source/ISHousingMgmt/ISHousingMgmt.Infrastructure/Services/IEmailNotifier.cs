using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.MembershipAndRoles;

namespace ISHousingMgmt.Infrastructure.Services {
	public interface IEmailNotifier {

		/// <summary>
		/// Salje emailom obavijest o registraciji
		/// </summary>
		/// <param name="user">registrirani korisnik</param>
		void NotifyOfRegistration(HousingMgmtUser user);

		/// <summary>
		/// Salje emailom obavijest o stvaranju rada uprave
		/// </summary>
		/// <param name="administrationJobsVoting">posao rada uprave</param>
		/// <param name="url">url adresa</param>
		void NotifyOfAdminJobsVotingCreation(AdministrationJobsVoting administrationJobsVoting, string url);

		/// <summary>
		/// Salje emailom obavijest o zavrsetku posla rada uprave
		/// </summary>
		/// <param name="administrationJobsVoting">posao rada uprave</param>
		/// <param name="url">url adresa</param>
		void NotifyOfAdminJobsVotingCompletition(AdministrationJobsVoting administrationJobsVoting, string url);

		/// <summary>
		/// Salje emailom obavijest o stvaranju novog odrzavanja
		/// </summary>
		/// <param name="maintenance">odrzavanje</param>
		/// <param name="url">url adresa</param>
		void NotifyOfMaintenanceCreation(Maintenance maintenance, string url);

		/// <summary>
		/// Salje emailom obavijest o delegaciji kvara izvodacu poslova
		/// </summary>
		/// <param name="maintenance">odrzavanje</param>
		/// <param name="url">url adresa</param>
		void NotifyOfMaintenanceDelegation(Maintenance maintenance, string url);

		/// <summary>
		/// Salje emailom obavijest o zavrsetku radova
		/// </summary>
		/// <param name="maintenance">odrzavanje</param>
		/// <param name="url">url adresa</param>
		void NotifyOfMaintenanceCompletition(Maintenance maintenance, string url);

		/// <summary>
		/// Salje emailom obavijest o prihvacanju popravka kvara
		/// </summary>
		/// <param name="maintenance">odrzavanje</param>
		/// <param name="submitterUrl"></param>
		/// <param name="contractorUrl"></param>
		void NotifyOfMaintenanceAcception(Maintenance maintenance, string submitterUrl, string contractorUrl);

		/// <summary>
		/// Salje emailom obavijest o izdavanju racuna
		/// </summary>
		/// <param name="bill">racun koji se izdaje</param>
		/// <param name="url">url adresa</param>
		void NotifyOfBilling(Bill bill, string url);

		/// <summary>
		/// Salje obavijest o uplati racuna
		/// </summary>
		/// <param name="bill">racun</param>
		/// <param name="url">url adresa</param>
		void NotifyOfPaying(Bill bill, string url);

	}
}
