#if !NET && !NETSTANDARD
using System;
#nullable enable
#endif
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Antelcat.Implements.Converters;

public class NoneConverter : TypeConverter
{
    public static readonly NoneConverter Instance = new ();

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => true;
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) => true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object ConvertFrom(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object value) => value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object? ConvertTo(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object? value,
        Type destinationType) => value;
}