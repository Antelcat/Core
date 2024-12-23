using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Antelcat.Server.Utils;

public class CookieConfigure : CookieBuilder
{
    public Func<CookieValidatePrincipalContext, Task> OnValidate { get; set; } = _ => Task.CompletedTask;
    public Func<RedirectContext<CookieAuthenticationOptions>, string>? OnDenied { get; set; }
    public Func<RedirectContext<CookieAuthenticationOptions>, string>? OnFailed { get; set; }
}