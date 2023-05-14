using Feast.Foundation.Core.Interface.Converting;

namespace Feast.Foundation.Core.Implements.Converters
{
    public class NoneConverter : IValueConverter
    {
        public static readonly NoneConverter Instance = new ();

        public object? To(object? input) => input;
        public object? Back(object? input) => input;
    }
}
