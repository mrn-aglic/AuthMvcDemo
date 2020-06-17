using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;

namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders
{
    public class RoleStore<T, U> : StoreBase, IRoleStore<T>
        where T : class, CsRole<U>
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _roleSet;

        public RoleStore(DbContext dbContext, DbSet<T> roleSet)
        {
            _dbContext = dbContext;
            _roleSet = roleSet;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public async Task<IdentityResult> CreateAsync(T csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);
            await _roleSet.AddAsync(csRole, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not create role: {csRole.Name}");
        }

        public async Task<IdentityResult> UpdateAsync(T csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);
            var role = await _roleSet.FindAsync(csRole.Id, cancellationToken);
            _roleSet.Update(role);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not update role: {csRole.Name}");
        }

        public async Task<IdentityResult> DeleteAsync(T csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);

            var role = await _roleSet.FindAsync(csRole.Id, cancellationToken);
            _roleSet.Remove(role);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not delete role: {csRole.Name}");
        }

        public Task<string> GetRoleIdAsync(T csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);
            return Task.FromResult(csRole.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(T csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);
            return Task.FromResult(csRole.Name);
        }

        public Task SetRoleNameAsync(T csRole, string roleName, CancellationToken cancellationToken)
        {
            csRole.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(T csRole, CancellationToken cancellationToken)
        {
            ThrowCheck(csRole, cancellationToken);
            return Task.FromResult(csRole.Name.ToLowerInvariant());
        }

        public Task SetNormalizedRoleNameAsync(T csRole, string normalizedName,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<T> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _roleSet.FirstOrDefaultAsync(r => r.Id.ToString() == roleId,
                cancellationToken);
        }

        public async Task<T> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _roleSet.FirstOrDefaultAsync(
                r => r.Name.Equals(normalizedRoleName, StringComparison.OrdinalIgnoreCase),
                cancellationToken);
        }
    }
}