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
    public class UserStore<T, U> : StoreBase,
        IUserStore<CsUser<T>>,
        IUserEmailStore<CsUser<T>>,
        IUserPasswordStore<CsUser<T>>,
        IUserRoleStore<CsUser<T>>,
        IQueryableUserStore<CsUser<T>>
    {
        private readonly AuthDbContext<T, U> _dbContext;

        public UserStore(AuthDbContext<T, U> dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public async Task<IdentityResult> CreateAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);

            await _dbContext.User.AddAsync(csUser, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not create user: {csUser.Email}");
        }

        public async Task<IdentityResult> DeleteAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            var dbUser = await _dbContext.User.FindAsync(csUser.Id);
            _dbContext.Remove(dbUser);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not delete user {csUser.Email}");
        }

        public async Task<CsUser<T>> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dbContext.User.SingleOrDefaultAsync(u =>
                u.Id.ToString() == userId, cancellationToken);
        }

        public async Task<CsUser<T>> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await
                _dbContext.User
                    .SingleOrDefaultAsync(
                        u => u.Username.ToLowerInvariant() == normalizedUserName,
                        cancellationToken);
        }

        public async Task<string> GetNormalizedUserNameAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            return await Task.FromResult(csUser.Username.ToLowerInvariant());
        }

        public async Task<string> GetUserIdAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            return await Task.FromResult(csUser.Id.ToString());
        }

        public async Task<string> GetUserNameAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            return await Task.FromResult(csUser.Username);
        }

        public Task SetNormalizedUserNameAsync(CsUser<T> csUser, string normalizedName,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(CsUser<T> csUser, string userName, CancellationToken cancellationToken)
        {
            csUser.Username = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            var user = await _dbContext.User.FindAsync(csUser.Id, cancellationToken);
            _dbContext.User.Update(user);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not update user: {csUser.Email}");
        }

        public Task SetPasswordHashAsync(CsUser<T> csUser, string passwordHash, CancellationToken cancellationToken)
        {
            csUser.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public async Task<string> GetPasswordHashAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            return await Task.FromResult(csUser.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            return await Task.FromResult(!string.IsNullOrWhiteSpace(csUser.PasswordHash));
        }

        public Task SetEmailAsync(CsUser<T> csUser, string email, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            csUser.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            return Task.FromResult(csUser.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(CsUser<T> csUser, bool confirmed, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            return Task.CompletedTask;
        }

        public async Task<CsUser<T>> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _dbContext.User.SingleOrDefaultAsync(x =>
                x.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            ThrowCheck(csUser, cancellationToken);
            return Task.FromResult(csUser.Email.ToUpper());
        }

        public Task SetNormalizedEmailAsync(CsUser<T> csUser, string normalizedEmail,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            // user.Email = normalizedEmail;
            // return Task.CompletedTask;
        }

        public async Task AddToRoleAsync(CsUser<T> csUser, string roleName, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Role.FirstOrDefaultAsync(r => r.Name == roleName,
                cancellationToken);
            await _dbContext.AddAsync(new
            {
                RoleId = role.Id,
                UserId = csUser.Id
            }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveFromRoleAsync(CsUser<T> csUser, string roleName, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Role.FirstOrDefaultAsync(p => p.Name == roleName,
                cancellationToken);
            _dbContext.Role.Remove(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IList<string>> GetRolesAsync(CsUser<T> csUser, CancellationToken cancellationToken)
        {
            return await _dbContext.UserRole
                .Where(r => r.UserId.Equals(csUser.Id))
                .Include(r => r.CsRole)
                .Select(x => x.CsRole.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsInRoleAsync(CsUser<T> csUser, string roleName, CancellationToken cancellationToken)
        {
            var roles = await GetRolesAsync(csUser, cancellationToken);
            return roles.Contains(roleName);
        }

        public async Task<IList<CsUser<T>>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await _dbContext.UserRole
                .Include(r => r.CsRole)
                .Include(r => r.CsUser)
                .Where(r => r.CsRole.Name == roleName)
                .Select(r => r.CsUser)
                .ToListAsync(cancellationToken);
        }

        public IQueryable<CsUser<T>> Users => _dbContext.User;
    }
}