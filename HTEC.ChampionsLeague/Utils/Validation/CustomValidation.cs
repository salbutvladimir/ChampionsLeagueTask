using System;
using System.ComponentModel.DataAnnotations;

namespace HTEC.ChampionsLeague.Utils.Validation
{
    public class CustomValidation
    {
        public sealed class DateGreaterThanAttribute : ValidationAttribute
        {
            public string otherPropertyName { get; set; }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ValidationResult validationResult = ValidationResult.Success;

                if (value == null)
                {
                    return validationResult;
                }

                // Using reflection we can get a reference to the other date property, in this example the project start date
                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(this.otherPropertyName);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue == null)
                {
                    return validationResult;
                }

                // Check that otherProperty is of type DateTime as we expect it to be
                if ((field.PropertyType == typeof(DateTime) ||
                    (field.PropertyType.IsGenericType && field.PropertyType == typeof(Nullable<DateTime>))))
                {
                    DateTime toValidate = (DateTime)value;
                    DateTime referenceProperty = (DateTime)field.GetValue(validationContext.ObjectInstance, null);

                    // if the end date is lower than the start date, than the validationResult will be set to false and return
                    // a properly formatted error message
                    if (toValidate.CompareTo(referenceProperty) < 1)
                    {
                        validationResult = new ValidationResult(ErrorMessageString);
                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }

                return validationResult;
            }

        }

    }

    public sealed class ScoreValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = value.ToString().Split(':');
            int i1, i2;
            if (result.Length == 2 && Int32.TryParse(result[0], out i1) && Int32.TryParse(result[1], out i2))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Score is not in corect format");
        }
    }

    public sealed class DateTimeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateTime;
            if(DateTime.TryParse(value.ToString(), out dateTime))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("DateTime is not in valid format");
        }
    }
}
