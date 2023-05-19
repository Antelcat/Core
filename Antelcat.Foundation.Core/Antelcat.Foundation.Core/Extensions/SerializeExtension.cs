using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Antelcat.Foundation.Core.Enums;
using Antelcat.Foundation.Core.Implements.Converters;

namespace Antelcat.Foundation.Core.Extensions
{
    public static class SerializeExtension
    {
        #region Privates
        private static JsonSerializerOptions Default => new()
        {
#if DEBUG
            WriteIndented = true,
#else
            WriteIndented = false,
#endif
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
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

            Cache[options] = ret;
            return ret;
        }
        #endregion

        public static string Serialize<T>(this T o, 
            SerializeOptions options = SerializeOptions.All) =>
            JsonSerializer.Serialize(o, GetSerializeOptions(options));

        public static string Serialize(this object[] args, 
            SerializeOptions options = SerializeOptions.All) =>
            args.Aggregate(new List<KeyValuePair<string, object>>(),
                (l, p) =>
                {
                    l.Add(new KeyValuePair<string, object>($"{p.GetType().Namespace}.{p.GetType().Name}", p));
                    return l;
                }).Serialize(options);

        public static string Serialize(this Dictionary<string, object> args,
            SerializeOptions options = SerializeOptions.All) => 
            ((object)args).Serialize(options);

        public static T? Deserialize<T>(this string json, 
            SerializeOptions options = SerializeOptions.All) =>
            JsonSerializer.Deserialize<T>(json, GetSerializeOptions(options));

        public static TOut? ReSerialize<TSource, TOut>(this TSource source) => 
            source.Serialize().Deserialize<TOut>();

        public static TOut? ReSerialize<TOut>(this object source, 
            SerializeOptions options = SerializeOptions.All) => 
            source.Serialize(options).Deserialize<TOut>(options);

    }
}
