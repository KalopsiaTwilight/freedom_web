using FreedomUtils.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomUtils.MvcUtils.Attributes
{
    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute()
            : base()
        {
            ErrorMessageResourceName = "ErrorPassword";
            ErrorMessageResourceType = typeof(CommonRes);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
            }

            string password = value.ToString();

            // length check
            if (password.Length < 8)
            {
                return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
