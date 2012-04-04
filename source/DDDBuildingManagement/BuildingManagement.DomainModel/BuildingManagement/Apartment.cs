using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Legislature;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.BuildingManagement {
	/// <summary>
	/// Razred(Entity) koji predstavlja stan
	/// </summary>
	public class Apartment : Room {

		#region Members

		/// <summary>
		/// Broj trenutnih stanara stana
		/// </summary>
		public int NumberOfTenants { get; set; }

		/// <summary>
		/// Odgovorni stanar/osoba
		/// </summary>
		public Person ResponsibleTenant { get; set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="partitionSpace">etaza</param>
		public Apartment(IPartitionSpace partitionSpace) : base(partitionSpace) { }

		#endregion

		#region Methods

		#endregion

	}
}
