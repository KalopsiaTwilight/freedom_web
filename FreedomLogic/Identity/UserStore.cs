using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Managers;
using FreedomLogic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FreedomLogic.Identity
{
    /// <summary>
    /// Freedom User store
    /// </summary>
    public class UserStore : IUserStore<User>,
        IUserPasswordStore<User>,
        IUserLockoutStore<User>,
        IUserTwoFactorStore<User>,
        IUserEmailStore<User>,
        IUserSecurityStampStore<User>,
        IUserRoleStore<User>
    {
        private readonly DbFreedom _freedomDb;
        private readonly DbAuth _authDb;
        private readonly AccountManager _accountManager;
        private readonly ExtraDataLoader _dataLoader;

        public UserStore(DbFreedom freedomDb, DbAuth authDb, AccountManager accountManager, ExtraDataLoader dataLoader)
        {
            _freedomDb = freedomDb;
            _authDb = authDb;
            _accountManager = accountManager;
            _dataLoader = dataLoader;
        }

        #region IUserStore
        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName;
            return Task.CompletedTask;
        }

        async Task<IdentityResult> IUserStore<User>.CreateAsync(User user, CancellationToken cancellationToken)
        {
            BnetAccount bnetAcc = _accountManager.CreateBnetAccount(user.UserName, user.BnetAccPassHash);
            _authDb.BnetAccounts.Add(bnetAcc);
            await _authDb.SaveChangesAsync();

            GameAccount gameAcc = _accountManager.CreateGameAccount(bnetAcc.Id, 1, user.RegEmail, user.GameAccPassHash);
            _authDb.GameAccounts.Add(gameAcc);
            await _authDb.SaveChangesAsync();

            _accountManager.SetGameAccAccessLevel(gameAcc.Id, GMLevel.Player);
            user.BnetAccountId = bnetAcc.Id;
            _freedomDb.Users.Add(user);
            await _freedomDb.SaveChangesAsync();

            user.FreedomRoles = new List<FreedomRole>();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            // Update game server data
            _dataLoader.LoadExtraUserData(user);
            BnetAccount bnetAcc = user.UserData.BnetAccount;
            GameAccount gameAcc = user.UserData.GameAccount;
            _accountManager.UpdateBnetAccount(bnetAcc, user.UserName, user.BnetAccPassHash);
            _accountManager.UpdateGameAccount(gameAcc, user.RegEmail, user.GameAccPassHash);
            _authDb.Entry(bnetAcc).State = EntityState.Modified;
            _authDb.Entry(gameAcc).State = EntityState.Modified;
            await _authDb.SaveChangesAsync();

            // Update web application data
            _freedomDb.Entry(user).State = EntityState.Modified;
            await _freedomDb.SaveChangesAsync();

            return IdentityResult.Success;
        }

        Task<IdentityResult> IUserStore<User>.DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            int id = int.Parse(userId ?? "-1");
            var user = _freedomDb.Users
                .Include(u => u.FreedomRoles)
                .Where(e => e.Id == id)
                .FirstOrDefault();
            return Task.FromResult(user);
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _freedomDb.Users
                .Include(u => u.FreedomRoles)
                .Where(e => e.UserName.ToUpper() == normalizedUserName.ToUpper())
                .FirstOrDefault();
            return Task.FromResult(user);
        }

        public void Dispose()
        {
            _freedomDb.Dispose();
            _authDb.Dispose();
        }
        #endregion

        #region IUserPasswordStore


        public Task SetPasswordHashAsync(User user, string plainPassword, CancellationToken cancellationToken)
        {
            user.BnetAccPassHash = _accountManager.BnetAccountCalculateShaHash(user.UserName, plainPassword);
            user.GameAccPassHash = plainPassword;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.BnetAccPassHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
        #endregion

        #region IUserLockoutStore(Disabled)

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult((DateTimeOffset?)null);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region IUserTwoFactorStore(Disabled)
        public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }
        #endregion

        #region IUserEmailStore
        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.RegEmail = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.RegEmail);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return _freedomDb.Users
                .Where(u => u.RegEmail.ToUpper() == normalizedEmail.ToUpper())
                .FirstOrDefaultAsync();
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.RegEmail);
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            //user.RegEmail = normalizedEmail;
            return Task.CompletedTask;
        }
        #endregion

        #region IUserSecurityStampStore
        public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }
        #endregion

        #region IUserRoleStore
        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<string>>(
                user
                .FreedomRoles
                .Select(r => r.Name)
                .ToList());
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.FreedomRoles.Any(r => r.Name == roleName));
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var users = await _freedomDb.Users
                .Include(u => u.FreedomRoles)
                .Where(u => u.FreedomRoles.Any(x => x.Name == roleName))
                .ToListAsync();
            return users;
        }

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
