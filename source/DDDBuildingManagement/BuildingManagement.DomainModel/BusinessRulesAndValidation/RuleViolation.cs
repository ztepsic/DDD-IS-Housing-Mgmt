using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BuildingManagement.DomainModel.BusinessRulesAndValidation {
	public class RuleViolation {

		#region Members

		/// <summary>
		/// Naziv svojstva/propertya nad kojim je greska napravljena
		/// </summary>
		public LambdaExpression Property { get; set; }

		/// <summary>
		/// Poruka greske
		/// </summary>
		public string Message { get; set; }

		#endregion

	}
}
