using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.Finances;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.BuildingManagement {
	/// <summary>
	/// Razred(Entity) koji predstavlja stambenu zgradu
	/// </summary>
	public class Building : NHibernateEntity {

		#region Members

		/// <summary>
		/// Minimalni koeficijent pricuve zakonom odredeno
		/// </summary>
		public const decimal MINIMAL_LEGAL_RESERVE_COEFFICIENT = 1.53m;

		/// <summary>
		/// Adresa na kojoj se nalazi zgrada
		/// </summary>
		public virtual Address Address { get; set; }

		/// <summary>
		/// Upravitelj zgrade
		/// </summary>
		private BuildingManager buildingManager;

		/// <summary>
		/// Dohvaca upravitelja zgrade
		/// </summary>
		public virtual BuildingManager BuildingManager { get { return buildingManager; } }

		/// <summary>
		/// Predstavnik suvlasnika
		/// </summary>
		public virtual Person RepresentativeOfPartOwners { get; set; }

		/// <summary>
		/// Zemljisna knjiga
		/// </summary>
		[BusinessKeyOfEntity]
		private LandRegistry landRegistry;

		/// <summary>
		/// Dohvaca zemljisnu knjigu
		/// </summary>
		public virtual LandRegistry LandRegistry {
			get { return landRegistry; }
			set { landRegistry = value; }
		}

		/// <summary>
		/// Koeficijent visine pricuve
		/// </summary>
		private decimal reserveCoefficient = MINIMAL_LEGAL_RESERVE_COEFFICIENT;

		/// <summary>
		/// Dohvaca koeficijent visine pricuve
		/// </summary>
		public virtual decimal ReserveCoefficient {
			get { return reserveCoefficient; }
			set {
				if(value < MINIMAL_LEGAL_RESERVE_COEFFICIENT) {
					throw new BusinessRulesException("Koeficijent pričuve mora biti najmanje " + MINIMAL_LEGAL_RESERVE_COEFFICIENT);
				}

				reserveCoefficient = value;
			}
		}

		/// <summary>
		/// Pričuva zgrade
		/// </summary>
		private Reserve reserve;

		/// <summary>
		/// Dohvača pričuvu zgrade
		/// </summary>
		public virtual Reserve Reserve { get { return reserve; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Prazni konstruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected Building() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="buildingManager">upravitelj zrade</param>
		public Building(BuildingManager buildingManager) {
			SetBuildingManager(buildingManager);
			reserve = new Reserve(this);
		}

		#endregion

		#region Methods

		public virtual IList<Person> GetOwners() {
			if(landRegistry != null) {
				var partitionSpaces = landRegistry.OwnedPartitionSpaces;
				return partitionSpaces.Select(partitionSpace => partitionSpace.Owner).ToList();	
			} else {
				return new List<Person>(0);
			}
			
		}
		
		/// <summary>
		/// Postavlja upravitelja zgrade
		/// </summary>
		/// <param name="buildingManager"></param>
		public virtual void SetBuildingManager(BuildingManager buildingManager) {
			if (buildingManager == null) {
				throw new BuildingMustHaveBuildingManagerException();
			}

			this.buildingManager = buildingManager;
		}

		#endregion

	}
}
