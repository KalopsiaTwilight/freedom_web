using FreedomLogic.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Identity
{
    public class UserManager : UserManager<User>
    {
        private readonly DbFreedom _freedomDb;

        public UserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, 
            IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger,
            DbFreedom freedomDb) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _freedomDb = freedomDb;
        }

        public async Task UpdateDisplayAndEmail(int userId, string displayName, string email)
        {
            var user = await _freedomDb.Users.FindAsync(userId);
            user.DisplayName = displayName;
            user.RegEmail = email;
            await _freedomDb.SaveChangesAsync();
        }
    }
}
