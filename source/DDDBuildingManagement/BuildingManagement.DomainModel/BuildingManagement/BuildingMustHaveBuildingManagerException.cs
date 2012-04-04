using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.BusinessRulesAndValidation;

namespace BuildingManagement.DomainModel.BuildingManagement {
	public class BuildingMustHaveBuildingManagerException : RulesException {

		/// <summary>
		/// Konstruktor
		/// </summary>
		public BuildingMustHaveBuildingManagerException() {
			this.ErrorForModel("Building must always have BuildingManager.");
		}
	}
}
