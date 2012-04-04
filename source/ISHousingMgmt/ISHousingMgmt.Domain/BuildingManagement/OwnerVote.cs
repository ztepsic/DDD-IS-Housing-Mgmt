using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.BuildingManagement {
	/// <summary>
	/// Razred(Value Object) koji predstavlja glasove vlasnika
	/// </summary>
	public class OwnerVote : ValueObject {

		#region Members

		/// <summary>
		/// Glas
		/// </summary>
		private readonly bool vote;

		/// <summary>
		/// Dohvaca glas
		/// </summary>
		public virtual bool Vote { get { return vote; } }

		/// <summary>
		/// Snapshot osobe
		/// </summary>
		private readonly PersonSnapshot owner;

		/// <summary>
		/// Dohvaca snapshot osobu
		/// </summary>
		public virtual PersonSnapshot Owner { get { return owner; } }

		/// <summary>
		/// Udio ukupnog vlasnistva temeljem etaze
		/// </summary>
		private readonly decimal shareOfTotalOwnership;

		/// <summary>
		/// Dohvaca udio ukupnog vlasnistva temeljem etaze
		/// </summary>
		public virtual decimal ShareOfTotalOwnership { get { return shareOfTotalOwnership; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Defaultni kontruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		private OwnerVote() {
			vote = false;
			owner = null;
			shareOfTotalOwnership = 0;
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="vote">glas</param>
		/// <param name="partitionSpace">vlasnicka etaza</param>
		public OwnerVote(bool vote, IPartitionSpace partitionSpace) {
			if (!partitionSpace.IsOwnedPartitionSpace) {
				throw new BusinessRulesException("This partition space is not OwnedPartitionSpace.");
			}

			this.vote = vote;
			this.shareOfTotalOwnership = partitionSpace.ShareOfTotalOwnership;
			owner = new PersonSnapshot(partitionSpace.Owner);
		}

		#endregion

		#region Methods

		#endregion

	}
}
