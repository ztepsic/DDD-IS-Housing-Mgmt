using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.BusinessRulesAndValidation;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.BuildingManagement {
	/// <summary>
	/// Razred(Entity) koji predstavlja posao uprave (redovne i izvanredne)
	/// </summary>
	public class AdministrationJobsVoting : EntityBase {

		#region Members

		/// <summary>
		/// Vrsta posla uprave
		/// </summary>
		private readonly AdministrationJobsType administrationJobsType;

		/// <summary>
		/// Dohvaca vrstu posla uprave
		/// </summary>
		public AdministrationJobsType AdministrationJobsType {
			get { return administrationJobsType; }
		}

		/// <summary>
		/// Ukupan broj vlasnika
		/// </summary>
		private readonly int numberOfOwners;

		/// <summary>
		/// Da li je glasanje zavrseno
		/// </summary>
		private bool isFinished;

		/// <summary>
		/// Da li je glasanje zavrseno
		/// </summary>
		public bool IsFinished {
			get { return isFinished; }
		}

		/// <summary>
		/// Da li je prihvaceno
		/// </summary>
		private bool isAccepted;

		public bool IsAccepted {
			get { return isAccepted; }
		}


		/// <summary>
		/// Vlasnicki glasovi
		/// </summary>
		private readonly IList<OwnerVote> ownerVotes;

		/// <summary>
		/// Dohvaca vlasnicke glasove
		/// </summary>
		public IList<OwnerVote> OwnerVotes {
			get { return new ReadOnlyCollection<OwnerVote>(ownerVotes); }
		}

		/// <summary>
		/// Broj pozitivnih glasova
		/// </summary>
		public int NumberOfPositiveVotes {
			get { return ownerVotes.Count(x => x.Vote); }
		}

		/// <summary>
		/// Broj negativnih glasova
		/// </summary>
		public int NumberOfNegativeVotes {
			get { return ownerVotes.Count(x => x.Vote == false); }
		}

		/// <summary>
		/// Zgrada
		/// </summary>
		private readonly Building building;

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="administrationJobsType">vrsta posla uprave</param>
		/// <param name="building">zgrada</param>
		public AdministrationJobsVoting(AdministrationJobsType administrationJobsType, Building building) {
			this.administrationJobsType = administrationJobsType;
			this.building = building;
			this.numberOfOwners = building.Apartments.Count;
			ownerVotes = new List<OwnerVote>();
			isFinished = false;
			isAccepted = false;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Glasanje
		/// </summary>
		/// <param name="ownerVote">glas vlasnika</param>
		public void AddVote(OwnerVote ownerVote) {
			ownerVotes.Add(ownerVote);
			evaluate();
		}

		/// <summary>
		/// Evaluacija - ukoliko se radi o poslovima redovne uprave dovoljno je da je pozitivno glasalo 51% ukupne vlasnicke povrsine, a
		/// ukoliko se radi o poslovima izvanredne uprave treba glasati pozitivno 100% ukupne vlasnicke povrsine.
		/// </summary>
		private void evaluate() {
			if(administrationJobsType == AdministrationJobsType.Regular) {
				if(isThereMoreThanHalfPositiveVotes()) {
					isAccepted = true;
					isFinished = true;
				} else {
					if(ownerVotes.Count == numberOfOwners) {
						isFinished = false;
						isAccepted = false;
					}
				}
			} else if(administrationJobsType == AdministrationJobsType.Extraordinary) {
				if(isThereAllPositiveVotes()) {
					isAccepted = true;
					isFinished = true;
				} else {
					if (ownerVotes.Count == numberOfOwners) {
						isFinished = false;
						isAccepted = false;
					}
				}
			} else {
				throw new RulesException("This AdministrationJobsType is not supported.");
			}
		}

		/// <summary>
		/// Da li je prikupljena polovica pozitivnih glasova
		/// </summary>
		/// <returns>true ako je, inace false</returns>
		private bool isThereMoreThanHalfPositiveVotes() {
			decimal sumOfShareOfTotalOwnershihpOfPositiveVotes = ownerVotes.Sum(x => x.ShareOfTotalOwnershihp);
			return sumOfShareOfTotalOwnershihpOfPositiveVotes >= 51 ? true : false;
		}

		/// <summary>
		/// Da li su svi glasali pozitivno
		/// </summary>
		/// <returns>true ako jesu, inace false</returns>
		private bool isThereAllPositiveVotes() {
			return ownerVotes.All(x => x.Vote);
		}

		#endregion

	}
}
