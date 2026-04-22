using System.ComponentModel.DataAnnotations;

namespace Project.Core.Common.Validators
{
    /// <summary>
    /// Helper class for model validation with detailed error information
    /// </summary>
    public class ValidationHelper
    {
        /// <summary>
        /// Validates an object and returns validation results with error details
        /// </summary>
        /// <param name="model">The model to validate</param>
        /// <returns>Dictionary with validation results: Key = property name, Value = list of errors</returns>
        public static Dictionary<string, List<string>> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            var errors = new Dictionary<string, List<string>>();

            if (!Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true))
            {
                foreach (var validationResult in validationResults)
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        if (!errors.ContainsKey(memberName))
                        {
                            errors[memberName] = new List<string>();
                        }
                        errors[memberName].Add(validationResult.ErrorMessage ?? "Validation error");
                    }
                }
            }

            return errors;
        }

        /// <summary>
        /// Checks if a model is valid
        /// </summary>
        public static bool IsModelValid(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
        }
    }
}
