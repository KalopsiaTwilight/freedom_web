using FreedomLogic.DAL;
using FreedomLogic.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Managers
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class UserManager : UserManager<User, int>
    {
        public UserManager(UserStore store)
            : base(store)
        {
        }

        public static void UpdateDisplayName(int userId, string newDisplayName)
        {
            using (var db = new DbFreedom())
            {
                var user = GetByKey(userId);
                user.DisplayName = newDisplayName.Trim();
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static User GetByUsername(string username)
        {
            using (var db = new DbFreedom())
            {
                return db.Users
                    .Include(u => u.FreedomRoles)
                    .Where(u => u.UserName.ToUpper() == username.ToUpper())
                    .FirstOrDefault();
            }
        }

        public static User GetByKey(int userId)
        {
            using (var db = new DbFreedom())
            {
                return db.Users
                    .Include(u => u.FreedomRoles)
                    .Where(u => u.Id == userId)
                    .FirstOrDefault();
            }
        }

        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            var manager = new UserManager(new UserStore());
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = false
            };

            // Configure password hashing
            manager.PasswordHasher = new FreedomShaHasher();

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User, int>
            {
                Subject = "WoW Freedom - Security Code",
                BodyFormat = "Your security code is {0}"
            });

            // Email
            manager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }
}
