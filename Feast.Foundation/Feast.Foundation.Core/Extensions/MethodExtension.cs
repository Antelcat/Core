using Feast.Foundation.Core.Structs.IL;
using System.Reflection;

namespace Feast.Foundation.Core.Extensions
{
    public static class MethodExtension
    {
        public static ILInvoker CreateInvoker(this MethodInfo method) => new(method);
        public static ILInvoker CreateGetter(this PropertyInfo property) =>
            property.CanRead 
                ? CreateInvoker(property.GetGetMethod(true)!) 
                : throw new NotSupportedException($"Property {property} can not read");
        public static ILInvoker CreateSetter(this PropertyInfo property) =>
            property.CanWrite
                ? CreateInvoker(property.GetSetMethod(true)!)
                : throw new NotSupportedException($"Property {property} can not write");
        
        public static IEnumerable<ILInvoker> CreateInvokers(this IEnumerable<MethodInfo> methods) =>
            methods.Select(x => CreateInvoker(x));

        public static IEnumerable<ILInvoker> CreateGetters(this IEnumerable<PropertyInfo> properties) =>
            properties.Select(x => CreateGetter(x));

        public static IEnumerable<ILInvoker> CreateSetters(this IEnumerable<PropertyInfo> properties) =>
            properties.Select(x => CreateSetter(x));

    }
}
