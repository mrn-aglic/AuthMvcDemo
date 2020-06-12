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
            var dataProtector = serviceProvider.GetDataProtector(new[]
            {
                $"{appDiscriminator}-Auth1"
            });

            services.Configure<CookiePolicyOptions>(opt =>
            {
                opt.Secure = CookieSecurePolicy.SameAsRequest;
                opt.CheckConsentNeeded = context => true;
                opt.HttpOnly = HttpOnlyPolicy.Always;
                opt.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.DefaultSignInScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(opt =>
                {
                    opt.TicketDataFormat = JwtAuthTicketFormat.Create(jwtConfig, serializer, dataProtector);
                    opt.LoginPath = "/pmfst/login";
                    opt.LogoutPath = "/pmfst/logout";
                    opt.AccessDeniedPath = "/pmfst/login";
                    opt.ReturnUrlParameter = "returnUrl";
                    opt.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    opt.Cookie.Name = ".pma.vj12.jwt.Cookie";
                    // opt.Cookie = new CookieBuilder
                    // {
                    //     Name = ".pma.vj12.Cookie",
                    //     HttpOnly = true,
                    //     Path = "/jwt",
                    //     SameSite = SameSiteMode.Lax,
                    //     SecurePolicy = CookieSecurePolicy.SameAsRequest
                    // };
                });
        }
    }
}