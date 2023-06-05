using Antelcat.Core.Extensions;
using System.ComponentModel;
using System.Globalization;
using Antelcat.Core.Interface.Converting;

namespace Antelcat.Core.Implements.Converters;
///<summary>
/// Convert between <see cref="string"/> and <see cref="sbyte"/>
///</summary>
public class StringToSbyteConverter : TypeConverter, IValueConverter, IValueConverter<string,sbyte>
{
	public object To(object? input) => (input as string).ToSbyte();
    public object? From(object? input) => input?.ToString();
	public sbyte To(string? input) => input.ToSbyte();
    public string From(sbyte input) => input.ToString(CultureInfo.InvariantCulture);
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => To(value);

   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object value) => From(value);

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
public class StringToByteConverter : TypeConverter, IValueConverter, IValueConverter<string,byte>
{
	public object To(object? input) => (input as string).ToByte();
    public object? From(object? input) => input?.ToString();
	public byte To(string? input) => input.ToByte();
    public string From(byte input) => input.ToString(CultureInfo.InvariantCulture);
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => To(value);

   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object value) => From(value);

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
public class StringToBoolConverter : TypeConverter, IValueConverter, IValueConverter<string,bool>
{
	public object To(object? input) => (input as string).ToBool();
    public object? From(object? input) => input?.ToString();
	public bool To(string? input) => input.ToBool();
    public string From(bool input) => input.ToString(CultureInfo.InvariantCulture);
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => To(value);

   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object value) => From(value);

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
public class StringToIntConverter : TypeConverter, IValueConverter, IValueConverter<string,int>
{
	public object To(object? input) => (input as string).ToInt();
    public object? From(object? input) => input?.ToString();
	public int To(string? input) => input.ToInt();
    public string From(int input) => input.ToString(CultureInfo.InvariantCulture);
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => To(value);

   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object value) => From(value);

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
public class StringToUintConverter : TypeConverter, IValueConverter, IValueConverter<string,uint>
{
	public object To(object? input) => (input as string).ToUint();
    public object? From(object? input) => input?.ToString();
	public uint To(string? input) => input.ToUint();
    public string From(uint input) => input.ToString(CultureInfo.InvariantCulture);
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => To(value);

   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object value) => From(value);

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(uint);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(uint);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="long"/>
///</summary>
public class StringToLongConverter : TypeConverter, IValueConverter, IValueConverter<string,long>
{
	public object To(object? input) => (input as string).ToLong();
    public object? From(object? input) => input?.ToString();
	public long To(string? input) => input.ToLong();
    public string From(long input) => input.ToString(CultureInfo.InvariantCulture);
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => To(value);

   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object value) => From(value);

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
public class StringToUlongConverter : TypeConverter, IValueConverter, IValueConverter<string,ulong>
{
	public object To(object? input) => (input as string).ToUlong();
    public object? From(object? input) => input?.ToString();
	public ulong To(string? input) => input.ToUlong();
    public string From(ulong input) => input.ToString(CultureInfo.InvariantCulture);
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => To(value);

   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object value) => From(value);

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
public class StringToDoubleConverter : TypeConverter, IValueConverter, IValueConverter<string,double>
{
	public object To(object? input) => (input as string).ToDouble();
    public object? From(object? input) => input?.ToString();
	public double To(string? input) => input.ToDouble();
    public string From(double input) => input.ToString(CultureInfo.InvariantCulture);
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => To(value);

   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object value) => From(value);

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
public class StringToFloatConverter : TypeConverter, IValueConverter, IValueConverter<string,float>
{
	public object To(object? input) => (input as string).ToFloat();
    public object? From(object? input) => input?.ToString();
	public float To(string? input) => input.ToFloat();
    public string From(float input) => input.ToString(CultureInfo.InvariantCulture);
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => To(value);

   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object value) => From(value);

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
public class StringToDateTimeConverter : TypeConverter, IValueConverter, IValueConverter<string,DateTime>
{
	public object To(object? input) => (input as string).ToDateTime();
    public object? From(object? input) => input?.ToString();
	public DateTime To(string? input) => input.ToDateTime();
    public string From(DateTime input) => input.ToString(CultureInfo.InvariantCulture);
	public override object ConvertTo(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object? value, 
		Type ___) => To(value);

   	public override object? ConvertFrom(
		ITypeDescriptorContext? _, 
		CultureInfo? __, 
		object value) => From(value);

   	public override bool CanConvertTo(
		ITypeDescriptorContext? _, 
		Type? destinationType) => destinationType == typeof(DateTime);

   	public override bool CanConvertFrom(
		ITypeDescriptorContext? _, 
		Type sourceType) => sourceType == typeof(DateTime);
}


public static class StringValueConverters
{
	private static readonly Dictionary<Type,IValueConverter> Instances = new ()
	{
		{ typeof(sbyte) , new StringToSbyteConverter() },
		{ typeof(byte) , new StringToByteConverter() },
		{ typeof(bool) , new StringToBoolConverter() },
		{ typeof(int) , new StringToIntConverter() },
		{ typeof(uint) , new StringToUintConverter() },
		{ typeof(long) , new StringToLongConverter() },
		{ typeof(ulong) , new StringToUlongConverter() },
		{ typeof(double) , new StringToDoubleConverter() },
		{ typeof(float) , new StringToFloatConverter() },
		{ typeof(DateTime) , new StringToDateTimeConverter() },
	};

	public static IValueConverter FindByType(Type type) => Instances.TryGetValue(type,out var ret) 
	? ret 
	: throw new NotSupportedException($"Specified type {type} not supported");
}