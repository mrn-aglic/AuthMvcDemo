using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.Interfaces;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders
{
    public class AuthDbContext<T, U, V> : DbContext where T : DbContext
    {
        public DbSet<IUser<U>> User { get; set; }
        public DbSet<IRole<V>> Role { get; set; }
        public DbSet<IUserRole<U, V>> UserRole { get; set; }

        private static DbContextOptions<AuthDbContext<T, U, V>> GetNewOptions(DbContextOptions<T> options)
        {
            var sqlExtensions = options.Extensions.OfType<SqlServerOptionsExtension>();
            var npgsqlExtensions = options.Extensions.OfType<NpgsqlOptionsExtension>();

            var builder = new DbContextOptionsBuilder<AuthDbContext<T, U, V>>();
            var sqlConnString = sqlExtensions.FirstOrDefault(e => e.ConnectionString != null)?.ConnectionString;
            var npgsqlConnString = npgsqlExtensions.FirstOrDefault(e => e.ConnectionString != null)?.ConnectionString;

            if (!string.IsNullOrEmpty(sqlConnString)) builder.UseSqlServer(sqlConnString);
            if (!string.IsNullOrEmpty(npgsqlConnString)) builder.UseNpgsql(npgsqlConnString);

            return builder.Options;
        }

        public AuthDbContext()
        {
        }

        public AuthDbContext(DbContextOptions<T> options)
            : base(GetNewOptions(options))
        {
        }
    }
}