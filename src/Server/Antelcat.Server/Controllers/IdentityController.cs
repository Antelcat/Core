using System.Security.Claims;
using Antelcat.ClaimSerialization;
using Antelcat.Core.Interface.Logging;
using Antelcat.DependencyInjectionEx.Autowired;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Controllers;

public abstract class IdentityController<TIdentity, TCategory> : Controller
{
    protected TIdentity? Identity => identity.Value;
    
    private readonly Lazy<TIdentity?> identity;

    protected IdentityController()
    {
        identity = new Lazy<TIdentity?>(() => ClaimSerializer.Deserialize<TIdentity>(User.Claims));
    }

    [Autowired] public required IAntelcatLogger<TCategory> Logger { get; init; }

    protected Task SignInAsync(TIdentity identity,
        string? authenticationType = "Identity.Application",
        AuthenticationProperties? properties = null,
        string scheme = CookieAuthenticationDefaults.AuthenticationScheme)

    {
        return Request.HttpContext.SignInAsync(scheme,
            new ClaimsPrincipal(new ClaimsIdentity(ClaimSerializer.Serialize(identity), authenticationType)),
            properties);
    }

    protected Task SignOutAsync(string scheme = CookieAuthenticationDefaults.AuthenticationScheme) =>
        Request.HttpContext.SignOutAsync(scheme);
}
