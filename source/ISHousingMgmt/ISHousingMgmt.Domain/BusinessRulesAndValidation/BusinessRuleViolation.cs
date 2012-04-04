using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISHousingMgmt.Domain.BusinessRulesAndValidation {
	/// <summary>
	/// Razred koji predstavlja prekrsaj pravila.
	/// Specificira nad kojim svojstvom je prekrseno pravilo i poruku greske.
	/// </summary>
	public class BusinessRuleViolation {

		#region Members

		/// <summary>
		/// Naziv svojstva nad kojim je greska napravljena
		/// </summary>
		public LambdaExpression Property { get; set; }

		/// <summary>
		/// Poruka greske
		/// </summary>
		public string Message { get; set; }

		#endregion
	}
}
