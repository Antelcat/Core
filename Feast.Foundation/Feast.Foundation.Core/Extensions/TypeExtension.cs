using Feast.Foundation.Core.Implements.Converters;
using Feast.Foundation.Core.Interface.Converting;
using System.Runtime.Serialization;

namespace Feast.Foundation.Core.Extensions
{
    public static class TypeExtension
    {
        public static object RawInstance(this Type type) => FormatterServices.GetSafeUninitializedObject(type);
        public static T RawInstance<T>() => (T)FormatterServices.GetSafeUninitializedObject(typeof(T));
        public static object? NewInstance(this Type type) => Activator.CreateInstance(type);
        public static T? NewInstance<T>() => Activator.CreateInstance<T>();

        public static IValueConverter? Converter(this Type type, Type toType)
        {
            return type == toType ? NoneConverter.Instance
                : type == typeof(string) 
                ? StringConverterBase.ConvertTo(toType)
                : null!;
        }
    }
}
