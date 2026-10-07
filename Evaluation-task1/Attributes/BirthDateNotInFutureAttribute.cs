using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

public class BirthDateNotInFutureAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime date)
        {
            if (date > DateTime.Now || date == DateTime.MinValue)
            {
                return new ValidationResult(ErrorMessage);
            }
        }
        return ValidationResult.Success;

    }
}
