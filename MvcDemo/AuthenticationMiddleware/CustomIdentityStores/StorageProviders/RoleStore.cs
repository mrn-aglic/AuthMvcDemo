using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;

namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders
{
    public class RoleStore<T, U> : StoreBase, IRoleStore<CsRole<U>>
    {
        private readonly AuthDbContext<T, U> _dbContext;

        public RoleStore(AuthDbContext<T, U> dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public async Task<IdentityResult> CreateAsync(CsRole<U> csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);
            await _dbContext.Role.AddAsync(csRole, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not create role: {csRole.Name}");
        }

        public async Task<IdentityResult> UpdateAsync(CsRole<U> csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);
            var role = await _dbContext.Role.FindAsync(csRole.Id, cancellationToken);
            _dbContext.Role.Update(role);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not update role: {csRole.Name}");
        }

        public async Task<IdentityResult> DeleteAsync(CsRole<U> csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);

            var role = await _dbContext.Role.FindAsync(csRole.Id, cancellationToken);
            _dbContext.Role.Remove(role);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not delete role: {csRole.Name}");
        }

        public Task<string> GetRoleIdAsync(CsRole<U> csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);
            return Task.FromResult(csRole.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(CsRole<U> csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);
            return Task.FromResult(csRole.Name);
        }

        public Task SetRoleNameAsync(CsRole<U> csRole, string roleName, CancellationToken cancellationToken)
        {
            csRole.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(CsRole<U> csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);
            return Task.FromResult(csRole.Name.ToLowerInvariant());
        }

        public Task SetNormalizedRoleNameAsync(CsRole<U> csRole, string normalizedName,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<CsRole<U>> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _dbContext.Role.FirstOrDefaultAsync(r => r.Id.ToString() == roleId,
                cancellationToken);
        }

        public async Task<CsRole<U>> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _dbContext.Role.FirstOrDefaultAsync(
                r => r.Name.Equals(normalizedRoleName, StringComparison.OrdinalIgnoreCase),
                cancellationToken);
        }
    }
}