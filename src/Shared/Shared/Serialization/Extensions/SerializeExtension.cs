using Antelcat.Enums;
using Antelcat.Implements.Converters;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Antelcat.Extensions;

public static partial class SerializeExtension
{
    #region Privates

    private static JsonSerializerOptions Default => new()
    {
        WriteIndented = false,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        AllowTrailingCommas = true
    };
    
    private static readonly Dictionary<SerializeOptions, JsonSerializerOptions> Cache = new()
    {
        { SerializeOptions.None, Default }
    };
    
    private static JsonSerializerOptions GetSerializeOptions(SerializeOptions options)
    {
        if (Cache.TryGetValue(options, out var ret)) return ret;
        ret = Default;
        if (options.HasFlag(SerializeOptions.EnumToString))
        {
            ret.Converters.Add(new JsonStringEnumConverter());
        }

        if (options.HasFlag(SerializeOptions.DateTimeToLong))
        {
            ret.Converters.Add(new JsonDateTimeConverter());
        }

        if (options.HasFlag(SerializeOptions.LowerCamelCase))
        {
            ret.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }

        ret.WriteIndented       = options.HasFlag(SerializeOptions.WriteIndented);
        
        Cache[options] = ret;
        return ret;
    }
    #endregion

    public static string Serialize<T>(this T o,
        SerializeOptions options = SerializeOptions.Default) =>
        JsonSerializer.Serialize(o, GetSerializeOptions(options));

    public static string Serialize(this IEnumerable<object> args, 
        SerializeOptions options = SerializeOptions.Default) =>
        args.Aggregate(new List<KeyValuePair<string, object>>(),
            (l, p) =>
            {
                var type = p.GetType();
                l.Add(new KeyValuePair<string, object>($"{type.Namespace}.{type.Name}", p));
                return l;
            }).Serialize(options);

    public static string Serialize(this Dictionary<string, object> args,
        SerializeOptions options = SerializeOptions.Default) => 
        ((object)args).Serialize(options);

    public static T? Deserialize<T>(this string json, 
        SerializeOptions options = SerializeOptions.Default) =>
        JsonSerializer.Deserialize<T>(json, GetSerializeOptions(options));

    public static TOut? ReSerialize<TSource, TOut>(this TSource source) => 
        source.Serialize().Deserialize<TOut>();

    public static TOut? ReSerialize<TOut>(this object source, 
        SerializeOptions options = SerializeOptions.Default) => 
        source.Serialize(options).Deserialize<TOut>(options);

}