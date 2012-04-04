using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.BusinessRulesAndValidation;
using BuildingManagement.DomainModel.Legislature;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.BuildingManagement {
	/// <summary>
	/// Razred(Value Object) koji predstavlja glasove vlasnika
	/// </summary>
	public class OwnerVote {

		#region Members

		/// <summary>
		/// Glas
		/// </summary>
		private readonly bool vote;

		/// <summary>
		/// Dohvaca glas
		/// </summary>
		public bool Vote {
			get { return vote; }
		}

		/// <summary>
		/// Snapshot osobe
		/// </summary>
		private PersonSnapshot owner;

		/// <summary>
		/// Dohvaca snapshot osobu
		/// </summary>
		public PersonSnapshot Owner {
			get { return owner; }
		}

		/// <summary>
		/// Udio ukupnog vlasnistva temeljem etaze
		/// </summary>
		private readonly decimal shareOfTotalOwnershihp;

		/// <summary>
		/// Dohvaca udio ukupnog vlasnistva temeljem etaze
		/// </summary>
		public decimal ShareOfTotalOwnershihp {
			get { return shareOfTotalOwnershihp; }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="vote">glas</param>
		/// <param name="partitionSpace">vlasnicka etaza</param>
		public OwnerVote(bool vote, IPartitionSpace partitionSpace) {
			if(!partitionSpace.IsOwnedPartitionSpace) {
				throw new RulesException("This partition space is not OwnedPartitionSpace.");
			}

			this.vote = vote;
			this.shareOfTotalOwnershihp = partitionSpace.ShareOfTotalOwnershihp;
			owner = new PersonSnapshot(partitionSpace.Owner);
		}

		#endregion

		#region Methods

		#endregion

	}
}
