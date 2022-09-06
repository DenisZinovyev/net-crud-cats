namespace System.ComponentModel.DataAnnotations
{
    using System;
    using System.Text.RegularExpressions;

    public class NameCheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            if (!(value is string name))
                throw new ArgumentException("Attribute not applied on String");

            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                return new ValidationResult(GetErrorMessage(validationContext));

            return ValidationResult.Success;
        }

        private string GetErrorMessage(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
                return this.ErrorMessage;

            return $"{validationContext.DisplayName} can contains only English letters";
        }
    }
}
