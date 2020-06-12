using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcDemo.AuthenticationMiddleware.JwtService;

namespace MvcDemo.MiddlewareExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAuthentication(this IServiceCollection services, string appDiscriminator,
            IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig").Get<JwtConfig>();

            services.AddDataProtection(options =>
                    options.ApplicationDiscriminator = appDiscriminator)
                .SetApplicationName(appDiscriminator);

            services.AddScoped<IDataSerializer<AuthenticationTicket>, TicketSerializer>();

            services.AddScoped(sp => new JwtGenerator(jwtConfig));

            var serviceProvider = services.BuildServiceProvider();
            var serializer = serviceProvider.GetService<IDataSerializer<AuthenticationTicket>>();
            var dataProtector = serviceProvider.GetService<IDataProtector>();

            services.Configure<CookiePolicyOptions>(opt =>
            {
                opt.Secure = CookieSecurePolicy.SameAsRequest;
                opt.CheckConsentNeeded = context => true;
                opt.HttpOnly = HttpOnlyPolicy.Always;
                opt.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.TicketDataFormat = JwtAuthTicketFormat.Create(jwtConfig, serializer, dataProtector);
                    opt.LoginPath = "/login";
                    opt.LogoutPath = "/logout";
                    opt.AccessDeniedPath = "/login";
                    opt.ReturnUrlParameter = "returnUrl";
                    opt.Cookie = new CookieBuilder
                    {
                        Name = ".pma.vj12.Cookie",
                        Expiration = TimeSpan.FromMinutes(20)
                    };
                });
        }
    }
}