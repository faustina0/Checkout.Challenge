using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace Checkout.Challenge.Api.Attributes
{
    public class CardExpiryDate: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var date = (string)value;

            if(!DateTime.TryParseExact(date,
                   "MM/yy",
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out var dateValue))
            {
                return new ValidationResult("Invalid format");
            }

            return dateValue.AddMonths(1)
                            .AddDays(-1) <= DateTime.Now? new ValidationResult("Past expiry date"): ValidationResult.Success;
        }
    }
}
