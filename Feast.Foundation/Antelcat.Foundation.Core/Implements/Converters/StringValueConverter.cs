using Antelcat.Foundation.Core.Extensions;
using System.Globalization;
using Antelcat.Foundation.Core.Interface.Converting;

namespace Antelcat.Foundation.Core.Implements.Converters;
///<summary>
/// Convert between <see cref="string"/> and <see cref="sbyte"/>
///</summary>
public class StringToSbyteConverter : IValueConverter, IValueConverter<string,sbyte>
{
	public object? To(object? input) => (input as string).ToSbyte();
    public object? Back(object? input) => input?.ToString();
	public sbyte To(string? input) => input.ToSbyte();
    public string Back(sbyte input) => input.ToString(CultureInfo.InvariantCulture);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="byte"/>
///</summary>
public class StringToByteConverter : IValueConverter, IValueConverter<string,byte>
{
	public object? To(object? input) => (input as string).ToByte();
    public object? Back(object? input) => input?.ToString();
	public byte To(string? input) => input.ToByte();
    public string Back(byte input) => input.ToString(CultureInfo.InvariantCulture);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="bool"/>
///</summary>
public class StringToBoolConverter : IValueConverter, IValueConverter<string,bool>
{
	public object? To(object? input) => (input as string).ToBool();
    public object? Back(object? input) => input?.ToString();
	public bool To(string? input) => input.ToBool();
    public string Back(bool input) => input.ToString(CultureInfo.InvariantCulture);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="int"/>
///</summary>
public class StringToIntConverter : IValueConverter, IValueConverter<string,int>
{
	public object? To(object? input) => (input as string).ToInt();
    public object? Back(object? input) => input?.ToString();
	public int To(string? input) => input.ToInt();
    public string Back(int input) => input.ToString(CultureInfo.InvariantCulture);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="uint"/>
///</summary>
public class StringToUintConverter : IValueConverter, IValueConverter<string,uint>
{
	public object? To(object? input) => (input as string).ToUint();
    public object? Back(object? input) => input?.ToString();
	public uint To(string? input) => input.ToUint();
    public string Back(uint input) => input.ToString(CultureInfo.InvariantCulture);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="long"/>
///</summary>
public class StringToLongConverter : IValueConverter, IValueConverter<string,long>
{
	public object? To(object? input) => (input as string).ToLong();
    public object? Back(object? input) => input?.ToString();
	public long To(string? input) => input.ToLong();
    public string Back(long input) => input.ToString(CultureInfo.InvariantCulture);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="ulong"/>
///</summary>
public class StringToUlongConverter : IValueConverter, IValueConverter<string,ulong>
{
	public object? To(object? input) => (input as string).ToUlong();
    public object? Back(object? input) => input?.ToString();
	public ulong To(string? input) => input.ToUlong();
    public string Back(ulong input) => input.ToString(CultureInfo.InvariantCulture);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="double"/>
///</summary>
public class StringToDoubleConverter : IValueConverter, IValueConverter<string,double>
{
	public object? To(object? input) => (input as string).ToDouble();
    public object? Back(object? input) => input?.ToString();
	public double To(string? input) => input.ToDouble();
    public string Back(double input) => input.ToString(CultureInfo.InvariantCulture);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="float"/>
///</summary>
public class StringToFloatConverter : IValueConverter, IValueConverter<string,float>
{
	public object? To(object? input) => (input as string).ToFloat();
    public object? Back(object? input) => input?.ToString();
	public float To(string? input) => input.ToFloat();
    public string Back(float input) => input.ToString(CultureInfo.InvariantCulture);
}

///<summary>
/// Convert between <see cref="string"/> and <see cref="DateTime"/>
///</summary>
public class StringToDateTimeConverter : IValueConverter, IValueConverter<string,DateTime>
{
	public object? To(object? input) => (input as string).ToDateTime();
    public object? Back(object? input) => input?.ToString();
	public DateTime To(string? input) => input.ToDateTime();
    public string Back(DateTime input) => input.ToString(CultureInfo.InvariantCulture);
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