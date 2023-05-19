namespace Antelcat.Foundation.Core.Extensions
{
    public static partial class StringExtension
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

        public static bool ToBool(this string? str, out bool result) => bool.TryParse(str, out result);
        public static bool ToBool(this string? str) => bool.TryParse(str, out var result) && result;
    }
}
