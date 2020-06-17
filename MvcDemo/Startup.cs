using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders;
using MvcDemo.DbModels;
using MvcDemo.MiddlewareExtensions;

namespace MvcDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.RegisterAuthentication("app", Configuration);
        }

        private void SetupDbContext(IServiceCollection services)
        {
            // services.AddDbContext<AuthDbContext<int, int>>(opt =>
            //     opt.UseInMemoryDatabase("pma"));
            // services.AddDbContext<pma_pmfContext>(opt =>
            //     opt.UseInMemoryDatabase("pma"));

            var connString = Configuration.GetConnectionString("pma_postgres");
            services.AddDbContext<AuthDbContext<int, int>>(opt =>
                opt.UseNpgsql(connString));
            services.AddDbContext<pma_pmfContext>(opt => opt.UseNpgsql(connString));

            // services.AddScoped<CsUser<int>>();
            // services.AddScoped<CsRole<int>>();

            services.AddIdentity<Appuser, Role>();
            services.AddTransient<IUserStore<Appuser>>(opt =>
            {
                var dbContext = opt.GetService<pma_pmfContext>();
                return new UserStore<Appuser, int, Role, int, UserRole>
                    (dbContext, dbContext.Appuser, dbContext.Role, dbContext.UserRole);
            });
            // services.AddTransient<IUserRoleStore<IUserRole<int, int>>, UserRoleStore<pma_pmfContext, int, int>>()
            services.AddTransient<IRoleStore<Role>>(opt =>
            {
                var dbContext = opt.GetService<pma_pmfContext>();
                return new RoleStore<Role, int>(dbContext, dbContext.Role);
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            ConfigureAuthentication(services);

            SetupDbContext(services);

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication(); // Enable authentication utilities
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "PmfKolegiji",
                    "pmfst/kolegiji",
                    new {controller = "courses", action = "courses"}
                );
                endpoints.MapControllerRoute(
                    name: "PmfOdjeli",
                    pattern: "pmfst/odjeli",
                    defaults: new {controller = "pmf", action = "odjeli"}
                );
                endpoints.MapControllerRoute(
                    "PmfLogin",
                    "pmfst/login",
                    new {controller = "authentication", action = "login"}
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}