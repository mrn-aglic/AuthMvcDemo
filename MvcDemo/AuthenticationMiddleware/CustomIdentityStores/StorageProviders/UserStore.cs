using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.Interfaces;

namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders
{
    public class UserStore<T, U, V> : StoreBase,
        IUserStore<IUser<U>>,
        IUserEmailStore<IUser<U>>,
        IUserPasswordStore<IUser<U>>,
        IUserRoleStore<IUser<U>> where T : DbContext
    {
        private readonly AuthDbContext<T, U, V> _dbContext;

        public UserStore(AuthDbContext<T, U, V> dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public async Task<IdentityResult> CreateAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            ThrowCheck(user, cancellationToken);

            await _dbContext.User.AddAsync(user, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not create user: {user.Email}");
        }

        public async Task<IdentityResult> DeleteAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            ThrowCheck(user, cancellationToken);
            var dbUser = await _dbContext.User.FindAsync(user.Id);
            _dbContext.Remove(dbUser);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not delete user {user.Email}");
        }

        public async Task<IUser<U>> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dbContext.User.SingleOrDefaultAsync(u =>
                u.Id.ToString() == userId, cancellationToken);
        }

        public async Task<IUser<U>> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await
                _dbContext.User
                    .SingleOrDefaultAsync(
                        u => u.Username.ToLowerInvariant() == normalizedUserName,
                        cancellationToken);
        }

        public async Task<string> GetNormalizedUserNameAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Username.ToLowerInvariant());
        }

        public async Task<string> GetUserIdAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Id.ToString());
        }

        public async Task<string> GetUserNameAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Username);
        }

        public Task SetNormalizedUserNameAsync(IUser<U> user, string normalizedName,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(IUser<U> user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            ThrowCheck(user, cancellationToken);
            _dbContext.User.Update(user);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return GetIdentityResult(result, $"Could not update user: {user.Email}");
        }

        public Task SetPasswordHashAsync(IUser<U> user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public async Task<string> GetPasswordHashAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetEmailAsync(IUser<U> user, string email, CancellationToken cancellationToken)
        {
            ThrowCheck(user, cancellationToken);
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            ThrowCheck(user, cancellationToken);
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            ThrowCheck(user, cancellationToken);
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(IUser<U> user, bool confirmed, CancellationToken cancellationToken)
        {
            ThrowCheck(user, cancellationToken);
            return Task.CompletedTask;
        }

        public async Task<IUser<U>> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _dbContext.User.SingleOrDefaultAsync(x =>
                x.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            ThrowCheck(user, cancellationToken);
            return Task.FromResult(user.Email.ToUpper());
        }

        public Task SetNormalizedEmailAsync(IUser<U> user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            // user.Email = normalizedEmail;
            // return Task.CompletedTask;
        }

        public async Task AddToRoleAsync(IUser<U> user, string roleName, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Role.FirstOrDefaultAsync(r => r.Name == roleName,
                cancellationToken);
            await _dbContext.AddAsync(new
            {
                RoleId = role.Id,
                UserId = user.Id
            }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveFromRoleAsync(IUser<U> user, string roleName, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Role.FirstOrDefaultAsync(p => p.Name == roleName,
                cancellationToken);
            _dbContext.Role.Remove(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IList<string>> GetRolesAsync(IUser<U> user, CancellationToken cancellationToken)
        {
            return await _dbContext.UserRole
                .Where(r => r.UserId.Equals(user.Id))
                .Include(r => r.Role)
                .Select(x => x.Role.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsInRoleAsync(IUser<U> user, string roleName, CancellationToken cancellationToken)
        {
            var roles = await GetRolesAsync(user, cancellationToken);
            return roles.Contains(roleName);
        }

        public async Task<IList<IUser<U>>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await _dbContext.UserRole
                .Include(r => r.Role)
                .Include(r => r.User)
                .Where(r => r.Role.Name == roleName)
                .Select(r => r.User)
                .ToListAsync(cancellationToken);
        }
    }
}