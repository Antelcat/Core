using Feast.Foundation.Core.Extensions;
using Feast.Foundation.Core.Interface.Converting;
using System.Reflection;

namespace Feast.Foundation.Core.Implements.Converters
{
    internal abstract class StringConverterBase
    {
        private static readonly Dictionary<Type, StringValueConverter> Converters = new();

        public static IValueConverter ConvertTo(Type toType) =>
            Converters.TryGetValue(toType, out var converter)
                ? converter
                : new StringValueConverter(toType);
        protected static void Instantiate(StringValueConverter converter) => 
            Converters.TryAdd(converter.TargetType, converter);
    }

    internal class StringValueConverter : StringConverterBase, IValueConverter
    {
        private static readonly IEnumerable<MethodInfo> Extensions =
            typeof(StringExtension)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.GetParameters().Length == 1
                            && x.GetParameters()[0].ParameterType == typeof(string)
                            && x.Name.StartsWith("To"));

        public readonly Type TargetType;
        
        public StringValueConverter(Type toType)
        {
            TargetType = toType;
            Instantiate(this);
            var method = Extensions.FirstOrDefault(x => x.ReturnType == toType)
                         ?? throw new NotSupportedException(
                             $"{toType.Name} convert from {nameof(String)} was not supported");
            var invoker = method.CreateInvoker();
            Converter = s => invoker.Invoker.Invoke(null, s);
        }

        protected readonly Func<object?, object> Converter;
        public object? To(object? input) => Converter(input);
        public object? Back(object? input) => input?.ToString();
    }

    internal class StringValueConverter<TOut> : StringValueConverter, IValueConverter<string, TOut> where TOut : class
    {
        public StringValueConverter() : base(typeof(TOut)) { }
        public TOut? To(string? input) => base.To(input) as TOut;
        public string? Back(TOut? input) => base.Back(input) as string;
    }
}
