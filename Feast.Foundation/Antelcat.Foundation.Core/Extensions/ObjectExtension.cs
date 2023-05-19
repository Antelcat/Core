namespace Antelcat.Foundation.Core.Extensions
{
    public static class ObjectExtension
    {
        public static T NotNull<T>(this T? o) 
            => o ??
#if DEBUG
               throw new NullReferenceException(typeof(T).FullName);
#else
               default!;
#endif
        public static object NotNull(this object? o) => o.NotNull<object>();
        public static bool IsNull(this object? o) => o == null;
        public static bool Has<T, TProperty>(this T o, Func<T, TProperty> property) => property.Invoke(o) != null;
        public static T Then<T>(this T obj,Action<T> action) where T : notnull
        {
            action(obj);
            return obj;
        }
    }
}
