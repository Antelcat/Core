using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Antelcat.Core.Extensions;
using Antelcat.Extensions;

namespace Antelcat.Exceptions;

#nullable enable

public class StackTraceException(string? message = null) : Exception(message)
{
    private readonly StackTrace stack = new(1, true);
    
    private readonly DateTime time = DateTime.Now;
    
    public StackFrame? ThrowFrame => TargetSite is not null
        ? frame ??= stack.GetFrames()?.FirstOrDefault(x => x.GetMethod() == TargetSite)
        : null;

    private StackFrame? frame;

    private string? toString;

    public override string ToString() => toString ??= ToJson();

    private string ToJson() =>
        ThrowFrame is null
            ? string.Empty
            : new StringBuilder("{\n")
                .AppendLine($"    \"file\"   : {ProvideJsonValue(ThrowFrame.GetFileName()?.Replace('\\','/'))},")
                .AppendLine($"    \"row\"    : {ThrowFrame.GetFileLineNumber()},")
                .AppendLine($"    \"method\" : {ProvideJsonValue(ThrowFrame.GetMethod()?.ToString())},")
                .AppendLine($"    \"time\"   : {ProvideJsonValue(time.ToString(CultureInfo.InvariantCulture))},")
                .AppendLine($"    \"message\": \"{new StringBuilder(Message)
                    .Replace("\r\n", "\\n")
                    .Replace("\n", "\\n")
                    .Replace("\t", "    ")
                    .Replace("\"", "\\\"")
                }\",")
                .AppendLineForEach(WriteExtraData(),static pair => $"    \"{pair.key}\" : {ProvideJsonValue(pair.value)},")
                .AppendLine("}")
                .ToString();

    protected virtual IEnumerable<(string key, object? value)> WriteExtraData() => Array.Empty<(string, object?)>();

    private static string ProvideJsonValue(object? value) => value switch
    {
        null                                      => "null",
        string or char                            => $"\"{value}\"",
        not null when value.GetType().IsPrimitive => $"{value}",
        _ =>
#if NET
            value.Serialize()
#else
            value.ToString() ?? "null"
#endif
    };
}