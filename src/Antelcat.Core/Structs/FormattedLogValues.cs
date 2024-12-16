using System.Collections;
using System.Collections.Concurrent;
using Antelcat.Core.Utils;

namespace Antelcat.Core.Structs;

/// <summary>
/// LogValues to enable formatting options supported by <see cref="M:string.Format"/>.
/// This also enables using {NamedFormatItem} in the format string.
/// </summary>
internal readonly struct FormattedLogValues : IReadOnlyList<KeyValuePair<string, object>>
{
    private const int MaxCachedFormatters = 1024;
    private const string NullFormat = "[null]";
    private static int count;
    private static readonly ConcurrentDictionary<string, LogValuesFormatter> Formatters = new();
    private readonly LogValuesFormatter? formatter;
    private readonly object[]? values;
    private readonly string originalMessage;

    // for testing purposes
    internal LogValuesFormatter? Formatter => formatter;

    public FormattedLogValues(string? format, params object[]? values)
    {
        if (values != null && values.Length != 0 && format != null)
        {
            if (count >= MaxCachedFormatters)
            {
                if (!Formatters.TryGetValue(format, out formatter))
                {
                    formatter = new LogValuesFormatter(format);
                }
            }
            else
            {
                formatter = Formatters.GetOrAdd(format, f =>
                {
                    Interlocked.Increment(ref count);
                    return new LogValuesFormatter(f);
                });
            }
        }
        else
        {
            formatter = null;
        }

        originalMessage = format ?? NullFormat;
        this.values = values;
    }

    public KeyValuePair<string, object> this[int index]
    {
        get
        {
            if (formatter == null || values == null)
            {
                if (index == 0)
                {
                    return new KeyValuePair<string, object>("{OriginalFormat}", originalMessage);
                }

                throw new IndexOutOfRangeException(nameof(index));
            }
            
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            return index == Count - 1 
                ? new KeyValuePair<string, object>("{OriginalFormat}", originalMessage) 
                : formatter.GetValue(values, index);
        }
    }

    public int Count
    {
        get
        {
            if (formatter == null)
            {
                return 1;
            }

            return formatter.ValueNames.Count + 1;
        }
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
        {
            yield return this[i];
        }
    }

    public override string ToString()
    {
        return formatter == null 
            ? originalMessage 
            : formatter.Format(values);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}