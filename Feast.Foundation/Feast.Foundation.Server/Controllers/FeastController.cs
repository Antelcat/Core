using Feast.Foundation.Core.Attributes;
using Feast.Foundation.Core.Interface.Logging;
using Feast.Foundation.Server.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Feast.Foundation.Server.Controllers
{
    public abstract class FeastController : Controller
    {
        protected TIdentity Identity<TIdentity>() => (TIdentity)(identity ??= JwtExtension<TIdentity>.FromClaims(User.Claims)!);
        private object? identity;

        [Autowired] protected IFeastLogger<FeastController> Logger { get; init; } = null!;
    }
    
    
    public abstract class FeastController<TIdentity> : FeastController
    {
        protected TIdentity Identity => identity ??= JwtExtension<TIdentity>.FromClaims(User.Claims);
        private TIdentity? identity;
    }
}
