using Microsoft.AspNetCore.SignalR;

namespace Antelcat.Server.Extensions;

public static class HubExtension
{
    public static Task AddToGroupAsync(this Hub hub, string groupName) =>
        hub.Groups.AddToGroupAsync(hub.Context.ConnectionId, groupName);

    public static Task RemoveFromGroupAsync(this Hub hub, string groupName) =>
        hub.Groups.RemoveFromGroupAsync(hub.Context.ConnectionId, groupName);
}