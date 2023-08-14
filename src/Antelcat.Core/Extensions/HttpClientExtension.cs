using System.Text;
using Antelcat.Extensions;

namespace Antelcat.Core.Extensions;

public static class HttpClientExtension
{
    public static Task<string> GetStringAsync(this HttpClient client,string requestUrl, Dictionary<string,string>? args)
    {
        var sb = new StringBuilder($"{requestUrl}?");
        args?.ForEach(kv => sb.Append($"{kv.Key}={kv.Value}&"));
        sb.Remove(sb.Length - 1, 1);
        return client.GetStringAsync(sb.ToString());
    }
}