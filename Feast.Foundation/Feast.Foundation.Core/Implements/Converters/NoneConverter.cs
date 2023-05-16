using System.Runtime.CompilerServices;
using Feast.Foundation.Core.Interface.Converting;

namespace Feast.Foundation.Core.Implements.Converters
{
    public class NoneConverter : IValueConverter
    {
        public static readonly NoneConverter Instance = new ();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object? To(object? input) => input;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object? Back(object? input) => input;
    }
}
