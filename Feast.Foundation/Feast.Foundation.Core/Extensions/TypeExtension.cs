using System.Runtime.Serialization;

namespace Feast.Foundation.Core.Extensions
{
    public static class TypeExtension
    {
        public static object RawInstance(this Type type) => FormatterServices.GetSafeUninitializedObject(type);
        public static T RawInstance<T>() => (T)FormatterServices.GetSafeUninitializedObject(typeof(T));
        public static object? NewInstance(this Type type) => Activator.CreateInstance(type);
        public static T? NewInstance<T>() => Activator.CreateInstance<T>();
    }
}
