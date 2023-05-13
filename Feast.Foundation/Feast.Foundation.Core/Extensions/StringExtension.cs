namespace Feast.Foundation.Extensions
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
        public static int ToInt(this string? str) =>
            int.TryParse(str, out var result) ? result : 0;
        public static long ToLong(this string? str) => 
            long.TryParse(str, out var result) ? result : 0L;
        public static bool ToBool(this string? str) =>
            bool.TryParse(str, out var result) && result;
        public static float ToFloat(this string? str) =>
            float.TryParse(str, out var result) ? result : float.NaN;
        public static double ToDouble(this string? str) =>
            double.TryParse(str, out var result) ? result : double.NaN;
        public static DateTime ToDateTime(this string? str) => 
            DateTime.TryParse(str, out var result) ? result : DateTime.MinValue;
    }
}
