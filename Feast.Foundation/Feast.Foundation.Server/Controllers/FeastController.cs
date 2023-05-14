using Feast.Foundation.Server.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Feast.Foundation.Server.Controllers
{
    public abstract class FeastController : Controller
    {
        protected TIdentity Identity<TIdentity>() => (TIdentity)(identity ??= JwtExtension<TIdentity>.FromClaims(User.Claims)!);
        private object? identity;
    }

    public abstract class FeastController<TIdentity> : Controller
    {
        protected TIdentity Identity => identity ??= JwtExtension<TIdentity>.FromClaims(User.Claims);
        private TIdentity? identity;
    }
}
