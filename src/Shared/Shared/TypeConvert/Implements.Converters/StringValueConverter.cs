#if !NET && !NETSTANDARD
using System;
using System.Collections.Generic;
#nullable enable
#endif
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Antelcat.Core.Extensions;
using Antelcat.Extensions;

namespace Antelcat.Implements.Converters;
///<summary>
/// Convert between <see cref="string"/> and <see cref="char"/>
///</summary>
public class StringToCharConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToChar();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(char);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(char);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="sbyte"/>
///</summary>
public class StringToSByteConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToSByte();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(sbyte);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(sbyte);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="byte"/>
///</summary>
public class StringToByteConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToByte();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(byte);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(byte);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="bool"/>
///</summary>
public class StringToBoolConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToBoolean();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(bool);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(bool);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="int"/>
///</summary>
public class StringToIntConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToInt32();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(int);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(int);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="uint"/>
///</summary>
public class StringToUIntConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToUInt32();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(uint);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(uint);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="short"/>
///</summary>
public class StringToShortConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToInt16();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(short);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(short);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="ushort"/>
///</summary>
public class StringToUShortConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToUInt16();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(ushort);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(ushort);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="long"/>
///</summary>
public class StringToLongConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToInt64();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(long);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(long);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="ulong"/>
///</summary>
public class StringToULongConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToUInt64();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(ulong);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(ulong);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="double"/>
///</summary>
public class StringToDoubleConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToDouble();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(double);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(double);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="float"/>
///</summary>
public class StringToFloatConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToSingle();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(float);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(float);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="DateTime"/>
///</summary>
public class StringToDateTimeConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToDateTime();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(DateTime);
   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(DateTime);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="Guid"/>
///</summary>
public class StringToGuidConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToGuid();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(Guid);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(Guid);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="Version"/>
///</summary>
public class StringToVersionConverter : TypeConverter
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override object? ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => (value as string).ToVersion();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value) => value?.ToString();

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(Version);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(Version);
}


public static class StringValueConverters
{
	private static readonly Dictionary<Type,TypeConverter> Instances = new ()
	{
		{ typeof(char) , new StringToCharConverter() },
		{ typeof(sbyte) , new StringToSByteConverter() },
		{ typeof(byte) , new StringToByteConverter() },
		{ typeof(bool) , new StringToBoolConverter() },
		{ typeof(int) , new StringToIntConverter() },
		{ typeof(uint) , new StringToUIntConverter() },
		{ typeof(short) , new StringToShortConverter() },
		{ typeof(ushort) , new StringToUShortConverter() },
		{ typeof(long) , new StringToLongConverter() },
		{ typeof(ulong) , new StringToULongConverter() },
		{ typeof(double) , new StringToDoubleConverter() },
		{ typeof(float) , new StringToFloatConverter() },
		{ typeof(DateTime) , new StringToDateTimeConverter() },
		{ typeof(Guid) , new StringToGuidConverter() },
		{ typeof(Version) , new StringToVersionConverter() },
	};

	public static bool FindByType(Type type, out TypeConverter? converter) 
		=> Instances.TryGetValue(type, out converter); 
}