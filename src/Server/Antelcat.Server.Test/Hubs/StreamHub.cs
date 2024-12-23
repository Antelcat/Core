using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;

namespace Antelcat.Server.Test.Hubs;

public class StreamHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task<bool> OnStream(ChannelReader<int> stream)
    {
        while (await stream.WaitToReadAsync())
        {
            while (stream.TryRead(out var value))
            {
                Console.WriteLine(value);
            }
        }
        return true;
    }
}