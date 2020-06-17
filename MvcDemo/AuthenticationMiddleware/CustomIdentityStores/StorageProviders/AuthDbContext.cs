using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders
{
    public class AuthDbContext<T, U> : DbContext
    {
        public virtual DbSet<CsUser<T>> User { get; set; }
        public virtual DbSet<CsRole<U>> Role { get; set; }
        public virtual DbSet<CsUserRole<CsUser<T>, T, CsRole<U>, U>> UserRole { get; set; }

        // public static DbContextOptions<AuthDbContext<T, U>> GetNewOptions<T>(
        //     DbContextOptions<T> options) where T : DbContext
        // {
        //     var sqlExtensions = options.Extensions.OfType<SqlServerOptionsExtension>();
        //     var npgsqlExtensions = options.Extensions.OfType<NpgsqlOptionsExtension>();
        //
        //     var builder = new DbContextOptionsBuilder<AuthDbContext<T, U>>();
        //     var sqlConnString = sqlExtensions.FirstOrDefault(e => e.ConnectionString != null)?.ConnectionString;
        //     var npgsqlConnString = npgsqlExtensions.FirstOrDefault(e => e.ConnectionString != null)?.ConnectionString;
        //
        //     if (!string.IsNullOrEmpty(sqlConnString)) builder.UseSqlServer(sqlConnString);
        //     if (!string.IsNullOrEmpty(npgsqlConnString)) builder.UseNpgsql(npgsqlConnString);
        //
        //     return builder.Options;
        // }

        public AuthDbContext()
        {
        }

        public AuthDbContext(DbContextOptions<AuthDbContext<int, int>> options) : base(options)
        {
        }

        protected AuthDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<CsUserRole<T, U>>(entity =>
            // {
            //     entity.HasKey(e => new { e.UserId, e.RoleId })
            //         .HasName("user_role_pk");
            // });
            base.OnModelCreating(modelBuilder);
        }
    }
}