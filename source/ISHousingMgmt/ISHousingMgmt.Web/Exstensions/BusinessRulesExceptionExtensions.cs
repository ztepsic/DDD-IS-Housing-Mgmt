using System.Web.Mvc;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;

namespace ISHousingMgmt.Web.Exstensions {
	public static class BusinessRulesExceptionExtensions {
		public static void CopyTo(this BusinessRulesException ex, ModelStateDictionary modelState) {
			CopyTo(ex, modelState, null);
		}

		public static void CopyTo(this BusinessRulesException ex, ModelStateDictionary modelState, string prefix) {
			prefix = string.IsNullOrEmpty(prefix) ? "" : prefix + ".";
			foreach (var propertyError in ex.Errors) {
				string key = ExpressionHelper.GetExpressionText(propertyError.Property);
				modelState.AddModelError(prefix + key, propertyError.Message);
			}
		}
	}
}
