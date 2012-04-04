using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;
using BuildingManagement.DomainModel.Legislature;
using BuildingManagement.DomainModel.PersonsAndRoles;

namespace BuildingManagement.DomainModel.BuildingManagement {
	/// <summary>
	/// Razred(Entity) koji predstavlja stambenu zgradu
	/// </summary>
	public class Building : EntityBase {

		#region Members

		/// <summary>
		/// Adresa na kojoj se nalazi zgrada
		/// </summary>
		public Address Address { get; set; }

		/// <summary>
		/// Upravitelj zgrade
		/// </summary>
		private BuildingManager buildingManager;

		/// <summary>
		/// Dohvaca upravitelja zgrade
		/// </summary>
		public BuildingManager BuildingManager { get { return buildingManager; } }

		/// <summary>
		/// Predstavnik suvlasnika
		/// </summary>
		public Person RepresentativeOfPartOwners { get; set; }

		/// <summary>
		/// Zemljisna knjiga
		/// </summary>
		private readonly LandRegistry landRegistry;

		/// <summary>
		/// Dohvaca zemljisnu knjigu
		/// </summary>
		public LandRegistry LandRegistry {
			get { return landRegistry; }
		}

		/// <summary>
		/// Dohvaca koeficijent visine pricuve
		/// </summary>
		public decimal ReserveCoefficient { get; set; }


		/// <summary>
		/// Prostorije zgrade
		/// </summary>
		private readonly IList<Room> rooms;

		/// <summary>
		/// Dohvaca sve prostorije zgrade
		/// </summary>
		public IList<Room> Rooms {
			get { return new ReadOnlyCollection<Room>(rooms); }
		}

		/// <summary>
		/// Stanovi
		/// </summary>
		private readonly IList<Apartment> apartments;

		/// <summary>
		/// Dohvaca stanove
		/// </summary>
		public IList<Apartment> Apartments {
			get { return new ReadOnlyCollection<Apartment>(apartments); }
		}



		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="landRegistry">zemljisna knjiga</param>
		/// <param name="buildingManager">upravitelj zrade</param>
		public Building(LandRegistry landRegistry, BuildingManager buildingManager) {
			this.landRegistry = landRegistry;
			SetBuildingManager(buildingManager);
			rooms = new List<Room>();
			apartments = new List<Apartment>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Metoda koja dodaje prostorije zgrade
		/// </summary>
		/// <param name="room">prostorije zgrade</param>
		public void AddRoom(Room room) {
			rooms.Add(room);
		}

		/// <summary>
		/// Metoda koja dodaje stanove
		/// </summary>
		/// <param name="apartment">stan</param>
		public void AddApartment(Apartment apartment) {
			rooms.Add(apartment);
		}

		/// <summary>
		/// Postavlja upravitelja zgrade
		/// </summary>
		/// <param name="buildingManager"></param>
		public void SetBuildingManager(BuildingManager buildingManager) {
			if(buildingManager == null) {
				throw new BuildingMustHaveBuildingManagerException();
			}

			this.buildingManager = buildingManager;
		}

		#endregion

	}
}
