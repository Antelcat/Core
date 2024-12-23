using System.Text.Json;

namespace Antelcat.Extensions;

public static partial class JsonElementExtension
{
    public static JsonElement? GetJsonElementOrNull(this JsonElement jsonElement, string propertyName)
    {
        return jsonElement.TryGetProperty(propertyName, out var value) ? value : null;
    }

    public static bool? GetBoolOrNull(this JsonElement jsonElement)
    {
        return jsonElement.ValueKind == JsonValueKind.Null ? null : jsonElement.GetBoolean();
    }
    
    public static bool IsNumericType(this object o)
    {
        if (o is JsonElement jsonElement)
        {
            return jsonElement.ValueKind == JsonValueKind.Number;
        }

        return Type.GetTypeCode(o.GetType()) switch
        {
            TypeCode.Byte => true,
            TypeCode.SByte => true,
            TypeCode.UInt16 => true,
            TypeCode.UInt32 => true,
            TypeCode.UInt64 => true,
            TypeCode.Int16 => true,
            TypeCode.Int32 => true,
            TypeCode.Int64 => true,
            TypeCode.Decimal => true,
            TypeCode.Double => true,
            TypeCode.Single => true,
            _ => false
        };
    }
    
    public static bool IsStringType(this object o) => 
        o is JsonElement jsonElement 
            ? jsonElement.ValueKind == JsonValueKind.String 
            : o is string;
}