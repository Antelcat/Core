using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !NETSTANDARD && !NET
using System;
#endif
#nullable enable
namespace Antelcat.Extensions;

public static class EnumExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasAnyFlag<TEnum>(this TEnum e, TEnum flag) where TEnum : Enum =>
        (Unsafe.As<TEnum, uint>(ref e) & Unsafe.As<TEnum, uint>(ref flag)) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasFlag<TEnum>(this TEnum e, TEnum flag) where TEnum : Enum =>
        (Unsafe.As<TEnum, uint>(ref e) | Unsafe.As<TEnum, uint>(ref flag)) == Unsafe.As<TEnum, uint>(ref e);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TAttribute? GetAttribute<TAttribute>(this Enum e) where TAttribute : Attribute =>
        e
            .GetType()
            .GetField(e.ToString())?
            .GetCustomAttribute<TAttribute>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDescription<TDescription>(this Enum e, Func<TDescription, string> selector)
        where TDescription : Attribute
    {
        var attr = e.GetAttribute<TDescription>();
        return attr == null ? e.ToString() : selector(attr);
    }

    /// <summary>
    /// 查找该枚举上的 <see cref="DescriptionAttribute.Description"/>
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDescription(this Enum e) =>
        e.GetDescription<DescriptionAttribute>(attr => attr.Description);
}