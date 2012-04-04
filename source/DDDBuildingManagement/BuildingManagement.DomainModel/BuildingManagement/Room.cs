using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.Legislature;

namespace BuildingManagement.DomainModel.BuildingManagement {
	/// <summary>
	/// Razred(Entity) koji predstavlja prostoriju
	/// </summary>
	public class Room : EntityBase {

		#region Members

		/// <summary>
		/// Tip/Vrsta prostorije
		/// </summary>
		public RoomType RoomType { get; set; }

		/// <summary>
		/// Opis prostorije
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Lokacija prostorije
		/// </summary>
		public string Location { get; set; }

		/// <summary>
		/// Povrsina prostorije
		/// </summary>
		public decimal SurfaceArea { get { return partitionSpace.SurfaceArea; } }

		/// <summary>
		/// Etaza
		/// </summary>
		private readonly IPartitionSpace partitionSpace;

		/// <summary>
		/// Dohvaca etazu
		/// </summary>
		public IPartitionSpace PartitionSpace {
			get { return partitionSpace; }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="partitionSpace">etaza</param>
		public Room(IPartitionSpace partitionSpace) {
			this.partitionSpace = partitionSpace;
		}

		#endregion

		#region Methods

		#endregion

	}
}
