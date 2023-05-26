namespace Antelcat.Foundation.Core.Extensions;

public static class CryptoExtension
{
    public static string Base64Encrypt(this string source) => Convert.ToBase64String(Global.Encoding.GetBytes(source));
    public static string Base64Decrypt(string base64) => Global.Encoding.GetString(Convert.FromBase64String(base64));
}