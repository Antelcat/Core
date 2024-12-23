#if !NET && !NETSTANDARD
using System.Collections.Generic;
#endif
using Comparers;

namespace Antelcat.Extensions;

public static partial class LinqExtension
{
    public static bool DeepEquals<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, 
        IDictionary<TKey, TValue> another) where TKey : notnull =>
        new DictionaryComparer<TKey, TValue>().Equals(dictionary, another);

    public static int GetDeepHashCode<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary) where TKey : notnull
        => new DictionaryComparer<TKey, TValue>().GetHashCode(dictionary);
}