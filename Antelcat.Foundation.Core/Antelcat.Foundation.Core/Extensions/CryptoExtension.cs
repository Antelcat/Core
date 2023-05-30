namespace Antelcat.Foundation.Core.Extensions;

public static class CryptoExtension
{
    public static string Base64Encrypt(this string plain) => Convert.ToBase64String(Global.Encoding.GetBytes(plain));
    public static string Base64Decrypt(this string base64) => Global.Encoding.GetString(Convert.FromBase64String(base64));
}