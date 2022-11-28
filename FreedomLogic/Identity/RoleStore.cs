using FreedomLogic.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FreedomLogic.Identity
{
    public class RoleStore : IRoleStore<FreedomRole>
    {
        private readonly DbFreedom _freedomDb;
        public RoleStore(DbFreedom freedomDb)
        {
            _freedomDb = freedomDb;
        }

        public void Dispose()
        {
            _freedomDb.Dispose();
        }
        public async Task<IdentityResult> CreateAsync(FreedomRole role, CancellationToken cancellationToken)
        {
            await _freedomDb.Roles.AddAsync(role, cancellationToken);
            await _freedomDb.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(FreedomRole role, CancellationToken cancellationToken)
        {
            _freedomDb.Roles.Remove(role);
            await _freedomDb.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<FreedomRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var role = await _freedomDb.Roles.FindAsync(int.Parse(roleId));
            return role;
        }

        public async Task<FreedomRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await _freedomDb.Roles.FirstOrDefaultAsync(x => x.Name == normalizedRoleName);
            return role;
        }

        public Task<string> GetNormalizedRoleNameAsync(FreedomRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task<string> GetRoleIdAsync(FreedomRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(FreedomRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(FreedomRole role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(FreedomRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(FreedomRole role, CancellationToken cancellationToken)
        {
            _freedomDb.Entry(role).State= EntityState.Modified;
            await _freedomDb.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }
    }
}
