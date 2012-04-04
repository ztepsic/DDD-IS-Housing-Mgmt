using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BuildingManagement.DomainModel.BusinessRulesAndValidation {
	/// <summary>
	/// Custom exception type that can store a collection of error messages.
	/// </summary>
	/// <remarks>Steven Sanderson: Pro ASP.NET MVC 2 Framework, Puting your model in Charge of Validation, str. 472,</remarks>
	public class RulesException : Exception {

		#region Members

		public readonly IList<RuleViolation> Errors = new List<RuleViolation>();
		private static readonly Expression<Func<object, object>> thisObject = x => x;

		#endregion

		#region Constructors and Init

		public RulesException() {}
		public RulesException(string message) {
			ErrorForModel(message);
		}

		#endregion

		#region Methods

		public void ErrorForModel(string message) {
			Errors.Add(new RuleViolation{ Property = thisObject, Message = message });
		}


		public override string Message {
			get {
				if (!Errors.Any()) {
					return base.Message;
				} else {
					StringBuilder stringBuilder = new StringBuilder();
					foreach (var ruleViolation in Errors) {
						stringBuilder.AppendLine(ruleViolation.Message);
					}

					return stringBuilder.ToString();
				}
			}
		}

		#endregion


	}

	/// <summary>
	/// Tipizirana (strongly-typed) verzija omogucava da sintaksom lambda izraza referenciramo svojstva objekta (properties)
	/// </summary>
	/// <typeparam name="TModel"></typeparam>
	/// <see cref="RulesException"/>
	public class RulesException<TModel> : RulesException {
		public void ErrorFor<TProperty>(Expression<Func<TModel, TProperty>> property, string message) {
			Errors.Add(new RuleViolation{Property = property, Message = message});
		}
	}
}
