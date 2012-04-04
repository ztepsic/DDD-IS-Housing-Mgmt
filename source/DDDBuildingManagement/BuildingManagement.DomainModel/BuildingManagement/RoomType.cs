using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.BuildingManagement {
	/// <summary>
	/// Razred(Entity) koji predstavlja tipove prostorija u zgradi
	/// </summary>
	public class RoomType : EntityBase {

		#region Members

		/// <summary>
		/// Naziv tipa sobe
		/// </summary>
		public string Type { get; set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="type">naziv tipa sobe</param>
		public RoomType(string type) {
			Type = type;
		}

		#endregion

		#region Methods
		#endregion

	}
}
