using System.Security.Claims;
using Antelcat.ClaimSerialization;
using Microsoft.AspNetCore.SignalR;

namespace Antelcat.Server.Hubs;

public abstract class BaseHub : Hub
{
    protected TIdentity? Identity<TIdentity>() =>
        ClaimSerializer.Deserialize<TIdentity>(Context.User?.Claims ?? new List<Claim>());
}

public abstract class BaseHub<T> : Hub<T> where T : class
{
    protected TIdentity? Identity<TIdentity>() => ClaimSerializer.Deserialize<TIdentity>(Context.User?.Claims ?? new List<Claim>());
}