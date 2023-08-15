using System.Net.Http.Json;
using System.Text;
using Antelcat.Core.Models;
using Antelcat.Extensions;

namespace Antelcat.Core.Extensions;

public static class HttpClientExtension
{
    public static Task<string> GetStringAsync(this HttpClient client,string requestUrl, Dictionary<string,string>? args)
    {
        if (args != null) requestUrl = MergeArgs(requestUrl, args);
        return client.GetStringAsync(requestUrl);
    }
    
    public static async Task<string> PutStringAsync(this HttpClient client,
        string requestUrl, 
        Dictionary<string,string>? args,
        object? json)
    {
        if (args != null) requestUrl = MergeArgs(requestUrl, args);
        return await (
                await client.PutAsync(
                    requestUrl,
                    new StringContent(json?.Serialize() ?? string.Empty)))
            .Content
            .ReadAsStringAsync();
    }

    private static string MergeArgs(string prefix,Dictionary<string,string> args)
    {
        var sb = new StringBuilder($"{prefix}?");
        args?.ForEach(kv => sb.Append($"{kv.Key}={kv.Value}&"));
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }
}