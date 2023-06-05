using System.Runtime.CompilerServices;
using Antelcat.Core.Interface.Converting;

namespace Antelcat.Core.Implements.Converters;

public class NoneConverter : IValueConverter
{
    public static readonly NoneConverter Instance = new ();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? To(object? input) => input;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? From(object? input) => input;
}