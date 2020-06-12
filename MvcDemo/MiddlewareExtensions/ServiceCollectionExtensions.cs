using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MvcDemo.MiddlewareExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAuthentication(this IServiceCollection services)
        {
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