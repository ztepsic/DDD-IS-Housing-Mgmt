using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;

namespace ISHousingMgmt.Domain.BuildingManagement {
	public class BuildingMustHaveBuildingManagerException : BusinessRulesException {

		/// <summary>
		/// Konstruktor
		/// </summary>
		public BuildingMustHaveBuildingManagerException() {
			this.AddErrorForModel("Building must always have BuildingManager.");
		}
	}
}
