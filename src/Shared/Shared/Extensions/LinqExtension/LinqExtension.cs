#if !NET && !NETSTANDARD
using System;
using System.Collections.Generic;
using System.Linq;
#endif
using System.Collections;
using System.Runtime.CompilerServices;

namespace Antelcat.Extensions;

public static partial class LinqExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source) action(item);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable) => enumerable == null || !enumerable.Any();

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        this IDictionary<TKey, TValue> first,
        IDictionary<TKey, TValue> second)
        where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>();
        foreach (var key in first.Keys)
        {
            result[key] = first[key];
        }

        foreach (var key in second.Keys)
        {
            result[key] = second[key];
        }

        return result;
    }

    public static string Join<T>(this IEnumerable<T> source, string separator) => string.Join(separator, source);

    public static IEnumerable<
#if !NET && !NETSTANDARD
        Tuple<T,int>
#else
        (T, int)
#endif
    > WithIndex<T>(this IEnumerable<T> source)
    {
        var i = 0;
        foreach (var item in source)
        {
            yield return
#if !NET && !NETSTANDARD
                 Tuple.Create
#endif
                (item, i++);
        }
    }
}