using System.Runtime.CompilerServices;

namespace Antelcat.Core.Extensions;

public static class ObjectExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NotNull<T>(this T? o) 
        => o ??
#if DEBUG
           throw new NullReferenceException(typeof(T).FullName);
#else
               default!;
#endif
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object NotNull(this object? o) => o.NotNull<object>();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNull(this object? o) => o == null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    public static bool Has<T, TProperty>(this T o, Func<T, TProperty> property) => property.Invoke(o) != null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    public static T Then<T>(this T obj,Action<T> action) where T : notnull
    {
        action(obj);
        return obj;
    }
}