using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Identity
{
    public class FreedomPasswordValidator : IPasswordValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            IdentityResult result = null;
            if (password.Length < 8)
            {
                result = IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordLengthTooShort",
                    Description = "Your password must be longer than 8 characters.",
                });
            } else if (password.Length > 16)
            {
                result = IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordLengthTooLong",
                    Description = "Unfortunately due to game client restriction your password can not be longer than 16 characters."
                });
            }
            if (result == null)
            {
                result = IdentityResult.Success;
            }
            return Task.FromResult(result);
        }
    }
}
