using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web.Mail;
using System.Web.Mvc;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.MembershipAndRoles;
using ISHousingMgmt.Domain.PersonsAndRoles;
using MailMessage = System.Net.Mail.MailMessage;

namespace ISHousingMgmt.Infrastructure.Services {
	public class EmailNotifier : IEmailNotifier {

		#region Members

		private const string MAIL_FROM = "no-reply@housingmgmt.com";

		private readonly IHousingMgmtUsersRepository housingMgmtUsersRepository;
		private readonly IPersonsRepository personsRepository;

		#endregion

		#region Constructors and Init

		public EmailNotifier(IHousingMgmtUsersRepository housingMgmtUsersRepository, IPersonsRepository personsRepository) {
			this.housingMgmtUsersRepository = housingMgmtUsersRepository;
			this.personsRepository = personsRepository;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Salje emailom obavijest o registraciji
		/// </summary>
		/// <param name="user">registrirani korisnik</param>
		public void NotifyOfRegistration(HousingMgmtUser user) {
			var mailTo = user.Email;
			var mailSubject = "Potvrda registracije";
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendFormat("Poštovani {0},{1}", user.Person.FullName, Environment.NewLine);
			mailBodySb.AppendLine("zahvaljujemo se na registraciji.");

			string mailBody = mailBodySb.ToString();

			using (var smtpClient = new SmtpClient()) {
				using(var mailMessage = new MailMessage(MAIL_FROM, mailTo, mailSubject, null)) {
					using(var alternateView = AlternateView.CreateAlternateViewFromString(mailBody, mailMessage.BodyEncoding, "text/html")) {
						alternateView.TransferEncoding = TransferEncoding.SevenBit;
						mailMessage.AlternateViews.Add(alternateView);
						smtpClient.Send(mailMessage);	
					}
				}
			}

		}

		/// <summary>
		/// Salje emailom obavijest o stvaranju rada uprave
		/// </summary>
		/// <param name="administrationJobsVoting">posao rada uprave</param>
		public void NotifyOfAdminJobsVotingCreation(AdministrationJobsVoting administrationJobsVoting, string url) {
			var mailSubject = "Rad uprave:" + administrationJobsVoting.Subject;
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendLine("Kreiran je novi rad uprave.");
			mailBodySb.AppendLine("----");
			mailBodySb.AppendFormat("Za zgradu: {0}", administrationJobsVoting.Building.Address).AppendLine();
			var type = administrationJobsVoting.AdministrationJobsType == AdministrationJobsType.Regular
			           	? "Regularna"
			           	: "Izvanredna";
			mailBodySb.AppendFormat("Vrsta upave: {0}", type).AppendLine();
			mailBodySb.AppendFormat("Naslov: {0}", administrationJobsVoting.Subject).AppendLine();
			mailBodySb.AppendFormat("Tema: {0}", administrationJobsVoting.Description).AppendLine();
			mailBodySb.AppendFormat("Početak: {0}", administrationJobsVoting.StartDateTime.ToShortDateString()).AppendLine();
			mailBodySb.AppendFormat("Kraj: {0}", administrationJobsVoting.EndDateTime.ToShortDateString()).AppendLine();
			mailBodySb.AppendLine();
			mailBodySb.AppendLine("Molimo glasajte").AppendLine();
			mailBodySb.AppendFormat("Detaljnije: {0}", url);

			string mailBody = mailBodySb.ToString();

			using (var smtpClient = new SmtpClient()) {
				foreach(var owner in administrationJobsVoting.Building.GetOwners()) {
					var user = housingMgmtUsersRepository.GetUserByPerson(owner);
					if(user != null) {
						var mailTo = user.Email;
						using (var mailMessage = new MailMessage(MAIL_FROM, mailTo, mailSubject, null)) {
							using (var alternateView = AlternateView.CreateAlternateViewFromString(mailBody, mailMessage.BodyEncoding, "text/html")) {
								alternateView.TransferEncoding = TransferEncoding.SevenBit;
								mailMessage.AlternateViews.Add(alternateView);
								smtpClient.Send(mailMessage);
							}
						}	
					}
				}
			}
		}

		/// <summary>
		/// Salje emailom obavijest o zavrsetku posla rada uprave
		/// </summary>
		/// <param name="administrationJobsVoting">posao rada uprave</param>
		/// <param name="url"></param>
		public void NotifyOfAdminJobsVotingCompletition(AdministrationJobsVoting administrationJobsVoting, string url) {
			var mailSubject = "Rad uprave:" + administrationJobsVoting.Subject;
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendLine("Završen je rad uprave.");
			mailBodySb.AppendLine("----");
			mailBodySb.AppendFormat("Za zgradu: {0}", administrationJobsVoting.Building.Address).AppendLine();
			var type = administrationJobsVoting.AdministrationJobsType == AdministrationJobsType.Regular
						? "Regularna"
						: "Izvanredna";
			mailBodySb.AppendFormat("Vrsta upave: {0}", type).AppendLine();
			mailBodySb.AppendFormat("Naslov: {0}", administrationJobsVoting.Subject).AppendLine();
			mailBodySb.AppendFormat("Tema: {0}", administrationJobsVoting.Description).AppendLine();
			mailBodySb.AppendFormat("Početak: {0}", administrationJobsVoting.StartDateTime.ToShortDateString()).AppendLine();
			mailBodySb.AppendFormat("Kraj: {0}", administrationJobsVoting.EndDateTime.ToShortDateString()).AppendLine();
			mailBodySb.AppendLine();
			mailBodySb.AppendLine();
			mailBodySb.AppendLine("Rezultati");
			mailBodySb.AppendFormat("Rad uprave prihvaćen: {0}", administrationJobsVoting.IsAccepted ? "Da" : "Ne");
			mailBodySb.AppendFormat("Glasali: {0} pozitivno, {1} negativo", administrationJobsVoting.NumberOfPositiveVotes, administrationJobsVoting.NumberOfNegativeVotes);
			mailBodySb.AppendFormat("Detaljnije: {0}", url);

			string mailBody = mailBodySb.ToString();

			using (var smtpClient = new SmtpClient()) {
				string mailTo = null;
				foreach (var owner in administrationJobsVoting.Building.GetOwners()) {
					
					var user = housingMgmtUsersRepository.GetUserByPerson(owner);
					if(user != null) {
						mailTo = user.Email;
						using (var mailMessage = new MailMessage(MAIL_FROM, mailTo, mailSubject, null)) {
							using (
								var alternateView = AlternateView.CreateAlternateViewFromString(mailBody, mailMessage.BodyEncoding, "text/html")
								) {
								alternateView.TransferEncoding = TransferEncoding.SevenBit;
								mailMessage.AlternateViews.Add(alternateView);
								smtpClient.Send(mailMessage);
							}
						}
					}
				}

				mailTo = housingMgmtUsersRepository.GetUserByPerson(administrationJobsVoting.Building.RepresentativeOfPartOwners).Email;
				using (var mailMessage = new MailMessage(MAIL_FROM, mailTo, mailSubject, null)) {
					using (var alternateView = AlternateView.CreateAlternateViewFromString(mailBody, mailMessage.BodyEncoding, "text/html")) {
						alternateView.TransferEncoding = TransferEncoding.SevenBit;
						mailMessage.AlternateViews.Add(alternateView);
						smtpClient.Send(mailMessage);
					}
				}


			}
		}

		private string generateGeneralInfoOfMaintenance(Maintenance maintenance) {
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendFormat("Za zgradu: {0}", maintenance.Building.Address).AppendLine();
			mailBodySb.AppendFormat("Lokacija: {0}", maintenance.MaintenanceRequest.Location).AppendLine();
			mailBodySb.AppendFormat("Naslov: {0}", maintenance.MaintenanceRequest.Subject).AppendLine();
			mailBodySb.AppendFormat("Opis: {0}", maintenance.MaintenanceRequest.Description).AppendLine();

			switch (maintenance.Urgency) {
				case Urgency.Low:
					mailBodySb.AppendLine("Hitnost: Niska");
					break;
				case Urgency.Normal:
					mailBodySb.AppendLine("Hitnost: Normalna");
					break;
				case Urgency.High:
					mailBodySb.AppendLine("Hitnost: Visoka");
					break;
				default:
					break;
			}

			mailBodySb.AppendFormat("Vrsta: {0}", maintenance.ServiceType.Name).AppendLine();
			mailBodySb.AppendFormat("Datum prijave: {0}", maintenance.MaintenanceRequest.DateTimeOfRequest.ToShortDateString()).AppendLine();
			mailBodySb.AppendFormat("Prijavio: {0}", maintenance.MaintenanceRequest.Submitter.FullName);
			

			return mailBodySb.ToString();
		}

		/// <summary>
		/// Salje emailom obavijest o stvaranju novog odrzavanja
		/// </summary>
		/// <param name="maintenance">odrzavanje</param>
		/// <param name="url"></param>
		public void NotifyOfMaintenanceCreation(Maintenance maintenance, string url) {
			string mailSubject = "Zahtjev za održavanjem/popravkom:" + maintenance.MaintenanceRequest.Subject;
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendLine("Kreiran je novi zahtjev za popravkom/održavanjem.");
			mailBodySb.AppendLine("----");
			mailBodySb.AppendLine(generateGeneralInfoOfMaintenance(maintenance));
			mailBodySb.AppendFormat("Detaljnije: {0}", url);

			string mailBody = mailBodySb.ToString();

			using (var smtpClient = new SmtpClient()) {
				string mailToRepresentative = housingMgmtUsersRepository.GetUserByPerson(maintenance.Building.RepresentativeOfPartOwners).Email;
				string mailToManager = housingMgmtUsersRepository.GetUserByPerson(maintenance.Building.BuildingManager.LegalPerson).Email;
				using (var mailMessage = new MailMessage(MAIL_FROM, mailToManager)) {
					using (var alternateView = AlternateView.CreateAlternateViewFromString(mailBody)) {
						mailMessage.Subject = mailSubject;
						alternateView.TransferEncoding = TransferEncoding.SevenBit;
						mailMessage.AlternateViews.Add(alternateView);
						smtpClient.Send(mailMessage);

						mailMessage.Sender = new MailAddress(mailToRepresentative);
						smtpClient.Send(mailMessage);

					}
				}
				
			}
		}

		/// <summary>
		/// Salje emailom obavijest o delegaciji kvara izvodacu poslova
		/// </summary>
		/// <param name="maintenance">odrzavanje</param>
		/// <param name="url"></param>
		public void NotifyOfMaintenanceDelegation(Maintenance maintenance, string url) {
			string mailSubject = "Delegiranje za održavanja/popravka:" + maintenance.MaintenanceRequest.Subject;
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendLine("Delegiranje popravka/održavanja.");
			mailBodySb.AppendLine("----");
			mailBodySb.AppendLine(generateGeneralInfoOfMaintenance(maintenance));
			mailBodySb.AppendLine();
			mailBodySb.AppendFormat("Izvođač radova: {0}", maintenance.Contractor.FullName).AppendLine();
			mailBodySb.AppendFormat("Instrukcije upravitelja: {0}", maintenance.Instructions).AppendLine();
			mailBodySb.AppendLine();
			mailBodySb.AppendFormat("Detaljnije: {0}", url);

			string mailBody = mailBodySb.ToString();

			using (var smtpClient = new SmtpClient()) {
				var contractor = personsRepository.GetByOib(maintenance.Contractor.Oib);
				string mailTo = housingMgmtUsersRepository.GetUserByPerson(contractor).Email;
				using (var mailMessage = new MailMessage(MAIL_FROM, mailTo)) {
					using (var alternateView = AlternateView.CreateAlternateViewFromString(mailBody)) {
						mailMessage.Subject = mailSubject;
						alternateView.TransferEncoding = TransferEncoding.SevenBit;
						mailMessage.AlternateViews.Add(alternateView);
						smtpClient.Send(mailMessage);

						mailMessage.Sender = new MailAddress(mailTo);
						smtpClient.Send(mailMessage);

					}
				}

			}
		}

		/// <summary>
		/// Salje emailom obavijest o zavrsetku radova
		/// </summary>
		/// <param name="maintenance">odrzavanje</param>
		/// <param name="url"></param>
		public void NotifyOfMaintenanceCompletition(Maintenance maintenance, string url) {
			string mailSubject = "Završeno održavanje/popravak za:" + maintenance.MaintenanceRequest.Subject;
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendLine("Delegiranje popravka/održavanja.");
			mailBodySb.AppendLine("----");
			mailBodySb.AppendLine(generateGeneralInfoOfMaintenance(maintenance));
			mailBodySb.AppendLine();
			mailBodySb.AppendFormat("Izvođač radova: {0}", maintenance.Contractor.FullName).AppendLine();
			mailBodySb.AppendFormat("Instrukcije upravitelja: {0}", maintenance.Instructions).AppendLine();
			mailBodySb.AppendFormat("Zaključak izvođača radova: {0}", maintenance.ContractorsConclusion).AppendLine();
			mailBodySb.AppendLine();
			mailBodySb.AppendFormat("Detaljnije: {0}", url);

			string mailBody = mailBodySb.ToString();

			using (var smtpClient = new SmtpClient()) {
				string mailTo = housingMgmtUsersRepository.GetUserByPerson(maintenance.Building.RepresentativeOfPartOwners).Email;
				using (var mailMessage = new MailMessage(MAIL_FROM, mailTo)) {
					using (var alternateView = AlternateView.CreateAlternateViewFromString(mailBody)) {
						mailMessage.Subject = mailSubject;
						alternateView.TransferEncoding = TransferEncoding.SevenBit;
						mailMessage.AlternateViews.Add(alternateView);
						smtpClient.Send(mailMessage);
					}
				}

			}
		}

		/// <summary>
		/// Salje emailom obavijest o prihvacanju popravka kvara
		/// </summary>
		/// <param name="maintenance">odrzavanje</param>
		/// <param name="submitterUrl"></param>
		/// <param name="contractorUrl"></param>
		public void NotifyOfMaintenanceAcception(Maintenance maintenance, string submitterUrl, string contractorUrl) {
			string mailSubject = "Prihvaćeno održavanje/popravak za:" + maintenance.MaintenanceRequest.Subject;
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendLine("Prihvaćeno održavanje/popravak.");
			mailBodySb.AppendLine("----");
			mailBodySb.AppendLine(generateGeneralInfoOfMaintenance(maintenance));
			mailBodySb.AppendLine();
			mailBodySb.AppendFormat("Izvođač radova: {0}", maintenance.Contractor.FullName).AppendLine();
			mailBodySb.AppendFormat("Instrukcije upravitelja: {0}", maintenance.Instructions).AppendLine();
			mailBodySb.AppendFormat("Zaključak izvođača radova: {0}", maintenance.ContractorsConclusion).AppendLine();
			mailBodySb.AppendLine();
			if(maintenance.MaintenanceRemarks.Count > 0){
				mailBodySb.AppendLine("Napomene predstavnika suvlasnika:");
				foreach (var maintenanceRemark in maintenance.MaintenanceRemarks) {
					mailBodySb.AppendFormat("{0}: {1}", maintenanceRemark.RemarkDateTime.ToString("dd.MM.yyyy, hh:mm:ss"), maintenanceRemark.Remark).AppendLine();	
				}
			}

			mailBodySb.AppendLine();

			string mailBody = mailBodySb.ToString();

			using (var smtpClient = new SmtpClient()) {
				
				var contractorPerson = personsRepository.GetByOib(maintenance.Contractor.Oib);
				string contractorMailTo = housingMgmtUsersRepository.GetUserByPerson(contractorPerson).Email;

				using (var mailMessage = new MailMessage(MAIL_FROM, contractorMailTo)) {
					string mailBodyContractor = mailBody + string.Format("Detaljnije: {0}{1}", contractorUrl, Environment.NewLine);
					using (var alternateView = AlternateView.CreateAlternateViewFromString(mailBodyContractor)) {
						mailMessage.Subject = mailSubject;
						alternateView.TransferEncoding = TransferEncoding.SevenBit;
						mailMessage.AlternateViews.Add(alternateView);
						smtpClient.Send(mailMessage);
					}
				}


				string managerMailTo = housingMgmtUsersRepository.GetUserByPerson(maintenance.Building.BuildingManager.LegalPerson).Email;

				var submitterPerson = personsRepository.GetByOib(maintenance.MaintenanceRequest.Submitter.Oib);
				string submitterMailTo = housingMgmtUsersRepository.GetUserByPerson(submitterPerson).Email;

				using (var mailMessage = new MailMessage(MAIL_FROM, contractorMailTo)) {
					string mailBodyGeneral = mailBody + string.Format("Detaljnije: {0}{1}", submitterUrl, Environment.NewLine);
					using (var alternateView = AlternateView.CreateAlternateViewFromString(mailBodyGeneral)) {
						mailMessage.Subject = mailSubject;
						alternateView.TransferEncoding = TransferEncoding.SevenBit;
						mailMessage.AlternateViews.Add(alternateView);
						mailMessage.Sender = new MailAddress(managerMailTo);
						smtpClient.Send(mailMessage);

						mailMessage.Sender = new MailAddress(submitterMailTo);
						smtpClient.Send(mailMessage);
					}
				}

			}
		}


		private string generateGeneralInfoOfBill(Bill bill) {
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendFormat("Izdao: {0}", bill.From == null ? bill.BuildingName : bill.From.FullName).AppendLine();
			mailBodySb.AppendFormat("Za: {0}", bill.To == null ? bill.BuildingName : bill.To.FullName).AppendLine();
			mailBodySb.AppendFormat("Datum izdavanja računa: {0}", bill.DateTimeIssued.ToShortDateString()).AppendLine();
			if(bill.PaidDateTime.HasValue) {
				mailBodySb.AppendFormat("Datum plaćanja računa: {0}", bill.PaidDateTime.Value.ToShortDateString()).AppendLine();	
			}
			mailBodySb.AppendFormat("Poziv na broj: {0}", bill.ReferenceNumber).AppendLine();
			mailBodySb.AppendLine().AppendLine();
			mailBodySb.AppendFormat("Osnovica za PDV: {0} kn", bill.TotalAmount).AppendLine();
			mailBodySb.AppendFormat("PDV: {0} kn", bill.TaxAmount).AppendLine();
			mailBodySb.AppendFormat("Ukupno: {0} kn", bill.TotalAmountWithTax).AppendLine();

			return mailBodySb.ToString();
		}

		/// <summary>
		/// Salje emailom obavijest o izdavanju racuna
		/// </summary>
		/// <param name="bill">racun koji se izdaje</param>
		/// <param name="url"></param>
		public void NotifyOfBilling(Bill bill, string url) {
			string mailSubject = "Izdan račun: " + bill.Id ;
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendFormat("Izdan je račun za {0}.", bill.To == null ? bill.BuildingName : bill.To.FullName).AppendLine();
			mailBodySb.AppendLine("----");
			mailBodySb.AppendLine(generateGeneralInfoOfBill(bill));
			mailBodySb.AppendLine();
			mailBodySb.AppendFormat("Detaljnije: {0}", url);

			string mailBody = mailBodySb.ToString();

			using (var smtpClient = new SmtpClient()) {
				Person person = bill.To == null ? bill.Reserve.Building.RepresentativeOfPartOwners : personsRepository.GetByOib(bill.To.Oib);
				var user = housingMgmtUsersRepository.GetUserByPerson(person);

				if(user != null) {
					string mailTo = user.Email;

					using (var mailMessage = new MailMessage(MAIL_FROM, mailTo)) {
						using (var alternateView = AlternateView.CreateAlternateViewFromString(mailBody)) {
							mailMessage.Subject = mailSubject;
							alternateView.TransferEncoding = TransferEncoding.SevenBit;
							mailMessage.AlternateViews.Add(alternateView);
							smtpClient.Send(mailMessage);
						}
					}	
				}
			}
		}

		/// <summary>
		/// Salje obavijest o uplati racuna
		/// </summary>
		/// <param name="bill">racun</param>
		/// <param name="url"></param>
		public void NotifyOfPaying(Bill bill, string url) {
			string mailSubject = "Plaćen račun: " + bill.Id;
			StringBuilder mailBodySb = new StringBuilder();
			mailBodySb.AppendFormat("Plaćen je račun br. {0}.", bill.Id).AppendLine();
			mailBodySb.AppendLine("----");
			mailBodySb.AppendLine(generateGeneralInfoOfBill(bill));
			mailBodySb.AppendLine();
			mailBodySb.AppendFormat("Detaljnije: {0}", url);

			string mailBody = mailBodySb.ToString();

			using (var smtpClient = new SmtpClient()) {
				Person person = bill.From == null ? bill.Reserve.Building.RepresentativeOfPartOwners : personsRepository.GetByOib(bill.From.Oib);
				string mailTo = housingMgmtUsersRepository.GetUserByPerson(person).Email;

				using (var mailMessage = new MailMessage(MAIL_FROM, mailTo)) {
					using (var alternateView = AlternateView.CreateAlternateViewFromString(mailBody)) {
						mailMessage.Subject = mailSubject;
						alternateView.TransferEncoding = TransferEncoding.SevenBit;
						mailMessage.AlternateViews.Add(alternateView);
						smtpClient.Send(mailMessage);
					}
				}

			}
		}

		#endregion
	}
}
