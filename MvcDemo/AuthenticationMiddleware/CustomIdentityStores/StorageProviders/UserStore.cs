using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;

namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders
{
    public class UserStore<T, TType, U, UType, V> : StoreBase,
        IUserStore<T>,
        IUserEmailStore<T>,
        IUserPasswordStore<T>,
        IUserRoleStore<T>,
        IQueryableUserStore<T>
        where T : class, CsUser<TType>
        where U : class, CsRole<UType>
        where V : class, CsUserRole<T, TType, U, UType>
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _userSet;
        private readonly DbSet<U> _roleSet;
        private readonly DbSet<V> _userRoleSet;

        public UserStore(DbContext dbContext, DbSet<T> userSet, DbSet<U> roleSet, DbSet<V> userRoleSet)
        {
            _dbContext = dbContext;
            _userSet = userSet;
            _roleSet = roleSet;
            _userRoleSet = userRoleSet;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public async Task<IdentityResult> CreateAsync(T csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);

            await _userSet.AddAsync(csUser, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not create user: {csUser.Email}");
        }

        public async Task<IdentityResult> DeleteAsync(T csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            var dbUser = await _userSet.FindAsync(csUser.Id);
            _dbContext.Remove(dbUser);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not delete user {csUser.Email}");
        }

        public async Task<T> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userSet.SingleOrDefaultAsync(u =>
                u.Id.ToString() == userId, cancellationToken);
        }

        public async Task<T> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await
                _userSet
                    .SingleOrDefaultAsync(
                        u => u.Username.ToLower() == normalizedUserName.ToLower(),
                        cancellationToken);
        }

        public async Task<string> GetNormalizedUserNameAsync(T csUser, CancellationToken cancellationToken)
        {
            return await Task.FromResult(csUser.Username.ToLowerInvariant());
        }

        public async Task<string> GetUserIdAsync(T csUser, CancellationToken cancellationToken)
        {
            return await Task.FromResult(csUser.Id.ToString());
        }

        public async Task<string> GetUserNameAsync(T csUser, CancellationToken cancellationToken)
        {
            return await Task.FromResult(csUser.Username);
        }

        public Task SetNormalizedUserNameAsync(T csUser, string normalizedName,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(T csUser, string userName, CancellationToken cancellationToken)
        {
            csUser.Username = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(T csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            var user = await _userSet.FindAsync(csUser.Id, cancellationToken);
            _userSet.Update(user);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not update user: {csUser.Email}");
        }

        public Task SetPasswordHashAsync(T csUser, string passwordHash, CancellationToken cancellationToken)
        {
            csUser.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public async Task<string> GetPasswordHashAsync(T csUser, CancellationToken cancellationToken)
        {
            return await Task.FromResult(csUser.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(T csUser, CancellationToken cancellationToken)
        {
            return await Task.FromResult(!string.IsNullOrWhiteSpace(csUser.PasswordHash));
        }

        public Task SetEmailAsync(T csUser, string email, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            csUser.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(T csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            return Task.FromResult(csUser.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(T csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(T csUser, bool confirmed, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            return Task.CompletedTask;
        }

        public async Task<T> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _userSet.SingleOrDefaultAsync(x =>
                x.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(T csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            return Task.FromResult(csUser.Email.ToUpper());
        }

        public Task SetNormalizedEmailAsync(T csUser, string normalizedEmail,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            // user.Email = normalizedEmail;
            // return Task.CompletedTask;
        }

        public async Task AddToRoleAsync(T csUser, string roleName, CancellationToken cancellationToken)
        {
            var role = await _roleSet.FirstOrDefaultAsync(r => r.Name == roleName,
                cancellationToken);
            await _dbContext.AddAsync(new
            {
                RoleId = role.Id,
                UserId = csUser.Id
            }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveFromRoleAsync(T csUser, string roleName, CancellationToken cancellationToken)
        {
            var role = await _roleSet.FirstOrDefaultAsync(p => p.Name == roleName,
                cancellationToken);
            _roleSet.Remove(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IList<string>> GetRolesAsync(T csUser, CancellationToken cancellationToken)
        {
            return await _userRoleSet
                .Where(r => r.UserId.Equals(csUser.Id))
                .Include(r => r.CsRole)
                .Select(x => x.CsRole.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsInRoleAsync(T csUser, string roleName, CancellationToken cancellationToken)
        {
            var roles = await GetRolesAsync(csUser, cancellationToken);
            return roles.Contains(roleName);
        }

        public async Task<IList<T>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await _userRoleSet
                .Include(r => r.CsRole)
                .Include(r => r.CsUser)
                .Where(r => r.CsRole.Name == roleName)
                .Select(r => r.CsUser)
                .ToListAsync(cancellationToken);
        }

        public IQueryable<T> Users => _userSet;
    }
}