using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;

namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders
{
    public class UserRoleStore<T, U> : StoreBase, IRoleStore<CsUserRole<T, U>>
    {
        private readonly AuthDbContext<T, U> _dbContext;

        public UserRoleStore(AuthDbContext<T, U> dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public async Task<IdentityResult> CreateAsync(CsUserRole<T, U> role, CancellationToken cancellationToken)
        {
            ThrowCheck(role, cancellationToken);
            await _dbContext.UserRole.AddAsync(role, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not assign user-role: {role.UserId}-{role.RoleId}");
        }

        public async Task<IdentityResult> UpdateAsync(CsUserRole<T, U> role, CancellationToken cancellationToken)
        {
            ThrowCheck(role, cancellationToken);
            _dbContext.UserRole.Update(role);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not assign user-role: {role.UserId}-{role.RoleId}");
        }

        public async Task<IdentityResult> DeleteAsync(CsUserRole<T, U> role, CancellationToken cancellationToken)
        {
            ThrowCheck(role, cancellationToken);
            _dbContext.UserRole.Remove(role);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not delete user-role: {role.UserId}-{role.RoleId}");
        }

        public Task<string> GetRoleIdAsync(CsUserRole<T, U> role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(CsUserRole<T, U> role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetRoleNameAsync(CsUserRole<T, U> role, string roleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(CsUserRole<T, U> role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(CsUserRole<T, U> role, string normalizedName,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<CsUserRole<T, U>> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<CsUserRole<T, U>> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}