using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.Interfaces;

namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders
{
    public class UserRoleStore<T, U, V> : StoreBase, IRoleStore<IUserRole<U, V>> where T : DbContext
    {
        private readonly AuthDbContext<T, U, V> _dbContext;

        public UserRoleStore(AuthDbContext<T, U, V> dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public async Task<IdentityResult> CreateAsync(IUserRole<U, V> role, CancellationToken cancellationToken)
        {
            ThrowCheck(role, cancellationToken);
            await _dbContext.UserRole.AddAsync(role, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not assign user-role: {role.UserId}-{role.RoleId}");
        }

        public async Task<IdentityResult> UpdateAsync(IUserRole<U, V> role, CancellationToken cancellationToken)
        {
            ThrowCheck(role, cancellationToken);
            _dbContext.UserRole.Update(role);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not assign user-role: {role.UserId}-{role.RoleId}");
        }

        public async Task<IdentityResult> DeleteAsync(IUserRole<U, V> role, CancellationToken cancellationToken)
        {
            ThrowCheck(role, cancellationToken);
            _dbContext.UserRole.Remove(role);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not delete user-role: {role.UserId}-{role.RoleId}");
        }

        public Task<string> GetRoleIdAsync(IUserRole<U, V> role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(IUserRole<U, V> role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetRoleNameAsync(IUserRole<U, V> role, string roleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(IUserRole<U, V> role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(IUserRole<U, V> role, string normalizedName,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IUserRole<U, V>> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IUserRole<U, V>> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}