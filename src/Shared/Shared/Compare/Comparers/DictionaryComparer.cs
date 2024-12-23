#if !NET && !NETSTANDARD
using System.Collections.Generic;
#nullable enable
#endif

namespace Comparers;

public class DictionaryComparer<TKey,TValue> : IEqualityComparer<IDictionary<TKey, TValue>> where TKey : notnull
{
    public bool Equals(IDictionary<TKey, TValue>? x, IDictionary<TKey, TValue>? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        if (x.Count != y.Count) return false;
        foreach (var pair in x)
        {
            if (!y.TryGetValue(pair.Key, out var value)) return false;
            if (!Equals(value, pair.Value)) return false;
        }

        return true;
    }

    public int GetHashCode(IDictionary<TKey, TValue> obj)
    {
        var hash = 0;
        foreach (var pair in obj)
        {
            hash ^= pair.Key.GetHashCode();
            if (pair.Value != null)
            {
                hash ^= pair.Value.GetHashCode();
            }
        }
        return hash;
    }
}