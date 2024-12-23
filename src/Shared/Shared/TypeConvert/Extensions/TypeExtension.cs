#if !NET && !NETSTANDARD
using System;
using System.Collections.Generic;
#nullable enable
#endif
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Antelcat.Implements.Converters;
namespace Antelcat.Extensions;

public static partial class TypeExtension
{
    public static bool TryGetConverter(this Type thisType, Type toType,
#if NET || NETSTANDARD
        [NotNullWhen(true)]
 #endif
        out TypeConverter? converter)
    {
        converter = null;
        if (thisType == toType)
        {
            converter = NoneConverter.Instance;
            return true;
        }

        if (thisType == typeof(string))
        {
            if (StringValueConverters.FindByType(toType, out converter!)) return true;
            converter = new StringConverter();
            return true;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisType"></param>
    /// <param name="toType"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static TypeConverter GetConverter(this Type thisType, Type toType) =>
        TryGetConverter(thisType, toType, out var ret)
            ? ret
            : throw new NotSupportedException("Not support this type yet");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? ConvertTo(this TypeConverter converter, object value) =>
        converter.ConvertTo(value, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? ConvertTo(this TypeConverter converter, object value, Type destinationType) =>
        converter.ConvertTo(null, null, value, destinationType);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? ConvertFrom(this TypeConverter converter, object value) =>
        converter.ConvertFrom(null!, null!, value);
}