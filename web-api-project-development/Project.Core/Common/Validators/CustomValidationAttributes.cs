using System.ComponentModel.DataAnnotations;

namespace Project.Core.Common.Validators
{
    /// <summary>
    /// Custom validator to ensure property value is not default
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotDefaultAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;

            var type = value.GetType();
            var defaultValue = Activator.CreateInstance(type);
            
            return !value.Equals(defaultValue);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} cannot be the default value.";
        }
    }

    /// <summary>
    /// Custom validator to ensure value is a positive number
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PositiveAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (decimal.TryParse(value.ToString(), out var decimalValue))
            {
                return decimalValue > 0;
            }

            if (int.TryParse(value.ToString(), out var intValue))
            {
                return intValue > 0;
            }

            if (double.TryParse(value.ToString(), out var doubleValue))
            {
                return doubleValue > 0;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a positive number.";
        }
    }

    /// <summary>
    /// Custom validator to ensure string contains only alphanumeric characters
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AlphanumericAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            var stringValue = value.ToString();
            return !string.IsNullOrEmpty(stringValue) && stringValue.All(char.IsLetterOrDigit);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must contain only alphanumeric characters.";
        }
    }

    /// <summary>
    /// Custom validator to ensure string matches a specific pattern
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RegexPatternAttribute : ValidationAttribute
    {
        private readonly string _pattern;

        public RegexPatternAttribute(string pattern)
        {
            _pattern = pattern;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            var stringValue = value.ToString();
            if (string.IsNullOrEmpty(stringValue))
                return true;

            var regex = new System.Text.RegularExpressions.Regex(_pattern);
            return regex.IsMatch(stringValue);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} does not match the required pattern.";
        }
    }
}
