using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using Antelcat.Server.Utils;
using Antelcat.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Extensions;

public static partial class ServiceExtension
{
    public static IServiceCollection ConfigureCookie(
        this IServiceCollection services,
        string scheme = CookieAuthenticationDefaults.AuthenticationScheme,
        Action<CookieConfigure>? configure = null)
    {
        services
            .AddAuthentication()
            .AddCookie(scheme,o =>
            {
                o.Cookie.SameSite = SameSiteMode.None;
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                var config = new CookieConfigure();
                configure?.Invoke(config);
                o.Cookie = config;
                o.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = config.Validate,
                    OnRedirectToAccessDenied = config.Denied,
                    OnRedirectToLogin = config.Failed
                };
            });
        return services;
    }

    public static IServiceCollection ConfigureSharedCookie(
        this IServiceCollection services,
        string scheme = CookieAuthenticationDefaults.AuthenticationScheme,
        Action<CookieConfigure>? configure = null)
    {
        services
            .AddDataProtection()
            .SetApplicationName("SharedCookieApp");
        return services.ConfigureCookie(scheme, c =>
        {
            c.Name = ".AspNet.SharedCookie";
            configure?.Invoke(c);
        });
    }
}