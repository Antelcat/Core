using System.Net.Mime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Antelcat.Server.Utils;

public class CookieConfigure : CookieBuilder
{
    internal Func<CookieValidatePrincipalContext, Task> Validate { get; set; } = _ => Task.CompletedTask;

    internal Func<RedirectContext<CookieAuthenticationOptions>, Task> Denied { get; set; } = async context =>
    {
        context.Response.Clear();
        context.Response.Headers.Clear();
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode  = StatusCodes.Status409Conflict;
    };

    internal Func<RedirectContext<CookieAuthenticationOptions>, Task> Failed { get; set; } = async context =>
    {
        context.Response.Clear();
        context.Response.Headers.Clear();
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode  = StatusCodes.Status401Unauthorized;
    };

    public CookieConfigure OnValidate(Func<CookieValidatePrincipalContext, Task> validate)
    {
        Validate = validate;
        return this;
    }
    
    public CookieConfigure OnDenied(Func<RedirectContext<CookieAuthenticationOptions>, Task> denied)
    {
        Denied = denied;
        return this;
    }
    
    public CookieConfigure OnDeniedJson<T>(Func<HttpResponse, Task<T>> denied) => OnDenied(async context =>
    {
        var response = context.Response;
        response.Clear();
        response.Headers.Clear();
        response.ContentType = MediaTypeNames.Application.Json;
        response.StatusCode  = StatusCodes.Status409Conflict;
        await response.WriteAsJsonAsync(await denied(response));
    });

    
    public CookieConfigure OnFailed(Func<RedirectContext<CookieAuthenticationOptions>, Task> failed)
    {
        Failed = failed;
        return this;
    }

    public CookieConfigure OnFailedJson<T>(Func<HttpResponse, Task<T>> failed) => OnFailed(async context =>
    {
        var response = context.Response;
        response.Clear();
        response.Headers.Clear();
        response.ContentType = MediaTypeNames.Application.Json;
        response.StatusCode  = StatusCodes.Status401Unauthorized;
        await response.WriteAsJsonAsync(await failed(response));
    });
}

