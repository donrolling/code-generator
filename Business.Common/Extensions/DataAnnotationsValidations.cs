using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Common.Extensions {
	public static class DataAnnotationValidation {
		public static List<ValidationResult> GetValidationErrors(this object obj) {
			ValidationContext validationContext = new ValidationContext(obj, null, null);
			List<ValidationResult> validationResults = new List<ValidationResult>();
			Validator.TryValidateObject(obj, validationContext, validationResults, true);
			return validationResults;
		}

		public static bool IsValid(this object obj) {
			ValidationContext validationContext = new ValidationContext(obj, null, null);
			return Validator.TryValidateObject(obj, validationContext, new List<ValidationResult>(), true);
		}
	}
}