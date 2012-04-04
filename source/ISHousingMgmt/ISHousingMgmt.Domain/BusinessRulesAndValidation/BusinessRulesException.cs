using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ISHousingMgmt.Domain.BusinessRulesAndValidation {

	/// <summary>
	/// Iznimka poslovnih pravila koja moze spremiti kolekciju poruka o greskama.
	/// Tipizirana (strongly-typed) verzija omogucava da sintaksom lambda izraza referenciramo svojstva objekta (properties)
	/// </summary>
	/// <typeparam name="TModel"></typeparam>
	/// <see cref="BusinessRulesException"/>
	public class BusinessRulesException<TModel> : BusinessRulesException {
		public void AddErrorFor<TProperty>(Expression<Func<TModel, TProperty>> property, string message) {
			errors.Add(new BusinessRuleViolation { Property = property, Message = message });
		}
	}

	/// <summary>
	/// Iznimka poslovnih pravila koja moze spremiti kolekciju poruka o greskama.
	/// </summary>
	/// <remarks>Steven Sanderson: Pro ASP.NET MVC 2 Framework, Puting your model in Charge of Validation, str. 472. </remarks>
	public class BusinessRulesException : Exception {

		#region Members

		/// <summary>
		/// Kolekcija poruka o greskama vezanim za prekrsenje pravila
		/// </summary>
		protected readonly IList<BusinessRuleViolation> errors = new List<BusinessRuleViolation>();

		/// <summary>
		/// Dohvaca kolekciju poruka o greskama vezanim za prekrsenje pravila
		/// </summary>
		public IList<BusinessRuleViolation> Errors { get { return new ReadOnlyCollection<BusinessRuleViolation>(errors); } }

		private static readonly Expression<Func<object, object>> thisObject = x => x;

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public BusinessRulesException() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="message">poruka o gresci/prekrsaju pravila</param>
		public BusinessRulesException(string message) : base(message) {
			AddErrorForModel(message);
		}


		#endregion

		#region Methods

		/// <summary>
		/// Dodaje poruku o prekrsenom pravilu
		/// </summary>
		/// <param name="message"></param>
		public void AddErrorForModel(string message) {
			errors.Add(new BusinessRuleViolation() { Property = thisObject, Message =  message });
		}

		/// <summary>
		/// Poruka o iznimci poslovnih pravila
		/// </summary>
		public override string Message {
			get {
				if(errors.Any()) {
					return base.Message;
				} else {
					StringBuilder stringBuilder = new StringBuilder();
					foreach (var ruleViolation in errors) {
						stringBuilder.AppendLine(ruleViolation.Message);
					}

					return stringBuilder.ToString();
				}
			}
		}

		#endregion

	}
}
