using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.BuildingManagement {
	/// <summary>
	/// Razred(Entity) koji predstavlja posao uprave (redovne i izvanredne)
	/// </summary>
	public class AdministrationJobsVoting : NHibernateEntity {

		#region Members

		/// <summary>
		/// Vrsta posla uprave
		/// </summary>
		private AdministrationJobsType administrationJobsType;

		/// <summary>
		/// Dohvaca vrstu posla uprave
		/// </summary>
		public virtual AdministrationJobsType AdministrationJobsType { get { return administrationJobsType; } }

		/// <summary>
		/// Svrha/naslov glasovanja
		/// </summary>
		private string subject;

		/// <summary>
		/// Dohvaca svrhu/naslov glasovanja
		/// </summary>
		[BusinessKeyOfEntity]
		public virtual string Subject { get { return subject; } }

		/// <summary>
		/// Opis razloga za glasovanje
		/// </summary>
		private string description;

		/// <summary>
		/// Dohvaca opis razloga za glasovanje
		/// </summary>
		public virtual string Description { get { return description; } }

		/// <summary>
		/// Datum i vrijeme pocetka glasovanja
		/// </summary>
		[BusinessKeyOfEntity]
		private DateTime startDateTime;

		/// <summary>
		/// Dohvaca datum i vrijeme pocetka glasovanja
		/// </summary>
		public virtual DateTime StartDateTime { get { return startDateTime; } }

		/// <summary>
		/// Datum i vrijeme kraja glasovanja
		/// </summary>
		private DateTime endDateTime;

		/// <summary>
		/// Dohvaca datum i vrijeme kraja glasovanja
		/// </summary>
		public virtual DateTime EndDateTime { get { return endDateTime; } }

		/// <summary>
		/// Ukupan broj vlasnika
		/// </summary>
		private int numberOfOwners;

		/// <summary>
		/// Dohvaca ukupan broj vlasnika
		/// </summary>
		public virtual int NumberOfOwners { get { return numberOfOwners; } }

		/// <summary>
		/// Da li je glasanje zavrseno
		/// </summary>
		private bool isFinished;

		/// <summary>
		/// Da li je glasanje zavrseno
		/// </summary>
		public virtual bool IsFinished { get { return isFinished; } }

		/// <summary>
		/// Da li je prihvaceno
		/// </summary>
		private bool isAccepted;

		public virtual bool IsAccepted { get { return isAccepted; } }


		/// <summary>
		/// Vlasnicki glasovi
		/// </summary>
		private Iesi.Collections.Generic.ISet<OwnerVote> ownerVotes;

		/// <summary>
		/// Dohvaca vlasnicke glasove
		/// </summary>
		public virtual Iesi.Collections.Generic.ISet<OwnerVote> OwnerVotes {
			get { return new ImmutableSet<OwnerVote>(ownerVotes); }
		}

		/// <summary>
		/// Broj pozitivnih glasova
		/// </summary>
		public virtual int NumberOfPositiveVotes {
			get { return ownerVotes.Count(x => x.Vote); }
		}

		/// <summary>
		/// Broj negativnih glasova
		/// </summary>
		public virtual int NumberOfNegativeVotes {
			get { return ownerVotes.Count(x => x.Vote == false); }
		}

		/// <summary>
		/// Zgrada
		/// </summary>
		private Building building;

		/// <summary>
		/// Dohvaca zgradu na koju se odnosi glasovanje
		/// </summary>
		public virtual Building Building { get { return building; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected AdministrationJobsVoting() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="administrationJobsType">vrsta posla uprave</param>
		/// <param name="building">zgrada</param>
		/// <param name="subject">naslov razloga glasovanja</param>
		/// <param name="description">opis razloga glasovanja</param>
		/// <param name="endDateTime">datum i vrijeme zavrsetka glasovanja</param>
		public AdministrationJobsVoting(AdministrationJobsType administrationJobsType, Building building, 
			string subject, string description, DateTime endDateTime) {
			this.administrationJobsType = administrationJobsType;
			this.building = building;
			this.subject = subject;
			this.description = description;
			startDateTime = DateTime.Now;
			this.endDateTime = endDateTime;
			numberOfOwners = building.LandRegistry.OwnedPartitionSpaces.Count;
			ownerVotes = new HashedSet<OwnerVote>();
			isFinished = false;
			isAccepted = false;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Glasanje
		/// </summary>
		/// <param name="ownerVote">glas vlasnika</param>
		public virtual void AddVote(OwnerVote ownerVote) {
			var isOwnerVoted = ownerVotes.Contains(ownerVote);
			if (isOwnerVoted) { return; }

			var isOwnerInThisBuilding = building.LandRegistry.OwnedPartitionSpaces.Any(
				ownedPartitionSpace => ownedPartitionSpace.Owner.Oib == ownerVote.Owner.Oib);

			if(isOwnerInThisBuilding) {
				ownerVotes.Add(ownerVote);
				Evaluate();	
			}
		}

		/// <summary>
		/// Provjerava da li je vlasnik glasao
		/// </summary>
		/// <param name="owner">vlasnik</param>
		/// <returns>true ako je glasao, inace false</returns>
		public virtual bool IsOwnerVoted(Person owner) {
			return ownerVotes.Any(ownerVote => ownerVote.Owner.Oib == owner.Oib);
		}

		/// <summary>
		/// Evaluacija - ukoliko se radi o poslovima redovne uprave dovoljno je da je pozitivno glasalo 51% ukupne vlasnicke povrsine, a
		/// ukoliko se radi o poslovima izvanredne uprave treba glasati pozitivno 100% ukupne vlasnicke povrsine.
		/// </summary>
		public virtual void Evaluate() {
			if(isFinished){ return; }

			// ako je vrijeme glasovanja isteklo ili su svi suvlasnici glasali
			if (DateTime.Now > EndDateTime || ownerVotes.Count == numberOfOwners) {
				isFinished = true;
			}

			switch (administrationJobsType) {
				case AdministrationJobsType.Regular:
					if (isThereMoreThanHalfPositiveVotes()) {
						isAccepted = true;
					}
					break;
				case AdministrationJobsType.Extraordinary:
					if (isThereAllPositiveVotes()) {
						isAccepted = true;
						isFinished = true;
					}
					break;
				default:
					throw new BusinessRulesException("This AdministrationJobsType is not supported.");
			}
		}

		/// <summary>
		/// Da li je prikupljena polovica pozitivnih glasova
		/// </summary>
		/// <returns>true ako je, inace false</returns>
		private bool isThereMoreThanHalfPositiveVotes() {
			//decimal sumOfShareOfTotalOwnershihpOfPositiveVotes = ownerVotes.Sum(x => x.ShareOfTotalOwnership);
			decimal sumOfShareOfTotalOwnershihpOfPositiveVotes =
				ownerVotes.Where(ov => ov.Vote)
				.Sum(ov => ov.ShareOfTotalOwnership);

			sumOfShareOfTotalOwnershihpOfPositiveVotes *= 100;
			return sumOfShareOfTotalOwnershihpOfPositiveVotes >= 51 ? true : false;
		}

		/// <summary>
		/// Da li su svi glasali pozitivno
		/// </summary>
		/// <returns>true ako jesu, inace false</returns>
		private bool isThereAllPositiveVotes() {
	
			if(OwnerVotes.IsEmpty || OwnerVotes.Count != NumberOfOwners) {
				return false;
			} else {
				return OwnerVotes.Aggregate(true, (current, ownerVote) => current & ownerVote.Vote);	
			}
		}

		#endregion

	}
}
