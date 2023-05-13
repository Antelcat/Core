namespace Feast.Foundation.Core.Extensions
{
    public static class StringExtension
    {
        public static string ToUpperCamelCase(this string value) => $"{(char)(value[0] - 32)}{value[1..]}";
        public static string ToLowerCamelCase(this string value) => $"{(char)(value[0] + 32)}{value[1..]}";
        public static string AnotherCamelCase(this string value) =>
            value.Length == 0
                ? throw new ArgumentNullException($"At least one char in string {value}")
                : value[0] switch
                {
                    >= 'a' and <= 'z' => value.ToUpperCamelCase(),
                    >= 'A' and <= 'Z' => value.ToLowerCamelCase(),
                    _ => throw new ArgumentOutOfRangeException($"First char should be letter but {value[0]}")
                };

        public static bool IsNullOrEmpty(this string? str) => string.IsNullOrEmpty(str);
        public static bool IsNullOrWhiteSpace(this string? str) => string.IsNullOrWhiteSpace(str);

        public static bool ToByte(this string? str, out byte result) => byte.TryParse(str, out result);
        public static bool ToBool(this string? str, out bool result) => bool.TryParse(str, out result);
        public static bool ToShort(this string? str, out short result) => short.TryParse(str, out result);
        public static bool ToUShort(this string? str, out ushort result) => ushort.TryParse(str, out result);
        public static bool ToInt(this string? str, out int result) => int.TryParse(str, out result);
        public static bool ToUInt(this string? str, out uint result) => uint.TryParse(str, out result);
        public static bool ToLong(this string? str, out long result) => long.TryParse(str, out result);
        public static bool ToULong(this string? str, out ulong result) => ulong.TryParse(str, out result);
        public static bool ToFloat(this string? str, out float result) => float.TryParse(str, out result);
        public static bool ToDouble(this string? str, out double result) => double.TryParse(str, out result);
        public static bool ToDateTime(this string? str, out DateTime result) => DateTime.TryParse(str, out result);
        public static byte ToByte(this string? str) => str.ToByte(out var result) ? result : byte.MinValue;
        public static bool ToBool(this string? str) => str.ToBool(out var result) && result;
        public static short ToShort(this string? str) => str.ToShort(out var result) ? result : short.MinValue;
        public static ushort ToUShort(this string? str) => str.ToUShort(out var result) ? result : ushort.MinValue;
        public static int ToInt(this string? str) => str.ToInt(out var result) ? result : int.MinValue;
        public static uint ToUInt(this string? str) => str.ToUInt(out var result) ? result : uint.MinValue;
        public static long ToLong(this string? str) => str.ToLong(out var result) ? result : long.MaxValue;
        public static ulong ToULong(this string? str) => str.ToULong(out var result) ? result : ulong.MinValue;
        public static float ToFloat(this string? str) => str.ToFloat(out var result) ? result : float.NaN;
        public static double ToDouble(this string? str) => str.ToDouble(out var result) ? result : double.NaN;
        public static DateTime ToDateTime(this string? str) => str.ToDateTime(out var result) ? result : DateTime.MinValue;
    }
}
