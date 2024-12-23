using System.Text.Json;
using System.Text.Json.Serialization;
using Antelcat.Core.Extensions;
using Antelcat.Extensions;

namespace Antelcat.Implements.Converters;

public class JsonDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type _, JsonSerializerOptions __) =>
        reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt64().ToLocalTime(),
            JsonTokenType.String => reader.GetString().ToDateTime(),
            JsonTokenType.Null => DateTime.MinValue,
            _ => reader.GetDateTime()
        };

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        if (value == DateTime.MinValue)
            writer.WriteNullValue();
        else writer.WriteNumberValue(value.ToTimestamp());
    }
}