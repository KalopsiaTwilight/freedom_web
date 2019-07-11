using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Identity
{
    /// <summary>
    /// Freedom User store
    /// </summary>
    public class UserStore : IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserLockoutStore<User, int>,
        IUserTwoFactorStore<User, int>,
        IUserEmailStore<User, int>,
        IUserSecurityStampStore<User, int>,
        IUserRoleStore<User, int>
    {
        private DbFreedom _freedomDb;
        private DbAuth _authDb;

        public UserStore()
        {
            _freedomDb = new DbFreedom();
            _authDb = new DbAuth();
        }

        #region IUserStore
        public Task CreateAsync(User user)
        {
            BnetAccount bnetAcc = AccountManager.CreateBnetAccount(user.UserName, user.BnetAccPassHash);
            _authDb.BnetAccounts.Add(bnetAcc);
            _authDb.SaveChanges();

            GameAccount gameAcc = AccountManager.CreateGameAccount(bnetAcc.Id, user.RegEmail, user.GameAccPassHash);
            _authDb.GameAccounts.Add(gameAcc);
            _authDb.SaveChanges();

            AccountManager.SetGameAccAccessLevel(gameAcc.Id, GMLevel.Player);

            user.BnetAccountId = bnetAcc.Id;
            _freedomDb.Users.Add(user);
            return _freedomDb.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _freedomDb.Dispose();
            _authDb.Dispose();
        }

        public Task<User> FindByIdAsync(int userId)
        {
            var user = _freedomDb.Users
                .Include(u => u.FreedomRoles)
                .Where(e => e.Id == userId)
                .FirstOrDefault();
            return Task.FromResult(user);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var user = _freedomDb.Users
                .Include(u => u.FreedomRoles)
                .Where(e => e.UserName.ToUpper() == userName.ToUpper())
                .FirstOrDefault();
            return Task.FromResult(user);
        }

        public Task UpdateAsync(User user)
        {
            // Update game server data
            BnetAccount bnetAcc = user.UserData.BnetAccount;
            GameAccount gameAcc = user.UserData.GameAccount;
            AccountManager.UpdateBnetAccount(bnetAcc, user.UserName, user.BnetAccPassHash);
            AccountManager.UpdateGameAccount(gameAcc, user.RegEmail, user.GameAccPassHash);
            _authDb.Entry(bnetAcc).State = EntityState.Modified;
            _authDb.Entry(gameAcc).State = EntityState.Modified;
            _authDb.SaveChanges();

            // Update web application data
            _freedomDb.Entry(user).State = EntityState.Modified;
            return _freedomDb.SaveChangesAsync();
        }
        #endregion

        #region IUserPasswordStore
        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.BnetAccPassHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(true);
        }

        public Task SetPasswordHashAsync(User user, string plainPassword)
        {
            user.BnetAccPassHash = AccountManager.BnetAccountCalculateShaHash(user.UserName, plainPassword);
            user.GameAccPassHash = AccountManager.GameAccountCalculateShaHash(user.UserName, plainPassword);
            return Task.FromResult(0);
        }
        #endregion

        #region IUserLockoutStore(Disabled)
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.FromResult(0);
        }
        #endregion

        #region IUserTwoFactorStore(Disabled)
        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }
        #endregion

        #region IUserEmailStore
        public Task SetEmailAsync(User user, string email)
        {
            user.RegEmail = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult(user.RegEmail);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return _freedomDb.Users
                .Where(u => u.RegEmail.ToUpper() == email.ToUpper())
                .FirstOrDefaultAsync();
        }
        #endregion

        #region IUserSecurityStampStore
        public Task SetSecurityStampAsync(User user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
        #endregion

        #region IUserRoleStore
        public Task AddToRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {            
            return Task.FromResult<IList<string>>(
                user
                .FreedomRoles
                .Select(r => r.Name)
                .ToList());
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return Task.FromResult(user.FreedomRoles.Any(r => r.Name == roleName));
        }
        #endregion
    }
}
