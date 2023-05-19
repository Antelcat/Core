using Antelcat.Foundation.Core.Attributes;
using Antelcat.Foundation.Core.Interface.Logging;
using Antelcat.Foundation.Server.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Foundation.Server.Controllers
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
