using Antelcat.ClaimSerialization;

namespace Antelcat.Utils;

public class JwtConfigureFactory
{
    internal readonly Dictionary<string, JwtConfigure> Configs = new();

    internal JwtConfigureFactory(ClaimSerializerContext? claimSerializerContext = null)
    {
        ClaimSerializerContext = claimSerializerContext;
    }
    
    internal ClaimSerializerContext? ClaimSerializerContext { get; }
}