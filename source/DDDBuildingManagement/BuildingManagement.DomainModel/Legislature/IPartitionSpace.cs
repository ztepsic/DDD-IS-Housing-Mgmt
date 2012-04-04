using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.Legislature {
	/// <summary>
	/// Javno sucelje etaze
	/// </summary>
	public interface IPartitionSpace {

		/// <summary>
		/// Dohvaca katastarsku cesticu
		/// </summary>
		AbstractCadastralParticle CadastralParticle { get; }

		/// <summary>
		/// Dohbaca ili postavlja redni broj etaze
		/// </summary>
		int OrdinalNumber { get; }

		/// <summary>
		/// Povrsina etaze
		/// </summary>
		decimal SurfaceArea { get; }

		/// <summary>
		/// Opis etaziranog prostora
		/// </summary>
		string Description { get; }


		/// <summary>
		/// Dohvaca vlasnika etaze
		/// </summary>
		Person Owner { get; set; }

		/// <summary>
		/// Da li je etaza pod vlasnistvom
		/// </summary>
		bool IsOwnedPartitionSpace { get; }

		/// <summary>
		/// Dohvaca udio ukupnog vlasnistva temeljem ove etaze
		/// </summary>
		decimal ShareOfTotalOwnershihp { get; }

	}
}
