using Feast.Foundation.Core.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using Feast.Foundation.Core.Interface.Converting;

namespace Feast.Foundation.Server.Extensions
{
    public static class JwtExtension<TIdentity>
    {
        private static readonly Type Type = typeof(TIdentity);

        private static readonly IDictionary<PropertyInfo, IValueConverter> ReadableProps = typeof(TIdentity)
            .GetProperties()
            .Where(x => x.CanRead)
            .ToDictionary(p => p, p => typeof(string).Converter(p.PropertyType)!);
        private static readonly IDictionary<PropertyInfo, IValueConverter> WritableProps = typeof(TIdentity)
            .GetProperties()
            .Where(x => x.CanWrite)
            .ToDictionary(p => p, p => typeof(string).Converter(p.PropertyType)!);

        private static TIdentity SetFromClaim(TIdentity instance, Claim claim)
        {
            WritableProps
                .FirstOrDefault(x => x.Key.Name == claim.Type)
                .Key.SetValue(instance, claim.Value);
            return instance;
        }
        public static TIdentity? FromToken(string token)
        {
            try
            {
                return new JwtSecurityToken(token)
                    .Claims
                    .Aggregate((TIdentity)Type.RawInstance(), 
                        SetFromClaim);
            }
            catch
            {
                return default;
            }
        }

        public static IEnumerable<Claim> GetClaims(TIdentity source)
        {
            return ReadableProps
                .Select(x =>
                {
                    var val = (string)x.Value.Back(x.Key.GetValue(source))!;
                    return new Claim(x.Key.Name, val);
                });
        }
        public static TIdentity FromClaims(IEnumerable<Claim> claims)
        {
            return claims.Aggregate((TIdentity)Type.RawInstance(), (i, c) =>
            {
                var pair = WritableProps.FirstOrDefault(x => x.Key.Name == c.Type);
                if (pair.Key.IsNull()) return i;
                pair.Key.SetValue(i, pair.Value.To(c.Value));
                return i;
            });
        }
    }
}
