using FreedomLogic.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Managers
{
    // Configure the application sign-in manager which is used in this application.
    public class SignInManager : SignInManager<User, int>
    {
        public SignInManager(UserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool rememberMe)
        {
            string hashedPassword = AccountManager.BnetAccountCalculateShaHash(userName, password);
            return base.PasswordSignInAsync(userName.ToUpper(), hashedPassword, rememberMe, false);
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((UserManager)UserManager);
        }

        public static SignInManager Create(IdentityFactoryOptions<SignInManager> options, IOwinContext context)
        {
            return new SignInManager(context.GetUserManager<UserManager>(), context.Authentication);
        }
    }
}
