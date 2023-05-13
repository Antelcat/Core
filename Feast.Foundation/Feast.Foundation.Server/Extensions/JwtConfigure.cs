using Feast.Foundation.Core.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Feast.Foundation.Server.Extensions
{
    public static class JwtExtension
    {
        public static void ConfigureJwt<TIdentity>(
            this IServiceCollection services, 
            Action<TokenValidationParameters>? configure = null,
            Func<TIdentity,Task>? validation = null,
            Func<JwtBearerChallengeContext,string>? failed = null)
        {
            var config = new JwtConfigure<TIdentity>();
            configure?.Invoke(config.Parameters);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.IncludeErrorDetails = true;
                o.TokenValidationParameters = config.Parameters;
                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = validation == null
                        ? _ => Task.CompletedTask
                        : async context =>
                        {
                            var token = (context.SecurityToken as JwtSecurityToken)!.RawData;
                            var identity = JwtConfigure<TIdentity>.FromToken(token);
                            if (identity == null)
                            {
                                context.Fail(new NullReferenceException($"Cannot resolve {typeof(TIdentity)} from token"));
                            }
                            await Task.CompletedTask;
                        },

                    OnChallenge = async context =>
                    {
                        if (failed == null) return;
                        context.HandleResponse();
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync(failed(context));
                    }
                };
            });
        }
    }

    public class JwtConfigure<TIdentity>
    {
        private static readonly Type Type = typeof(TIdentity);
        private static readonly IEnumerable<PropertyInfo> ReadableProps = typeof(TIdentity).GetProperties().Where(x=>x.CanRead);
        private static readonly IEnumerable<PropertyInfo> WritableProps = typeof(TIdentity).GetProperties().Where(x=>x.CanWrite);

        private static TIdentity SetClaimValue(TIdentity instance, Claim claim)
        {
            WritableProps.FirstOrDefault(x => x.Name == claim.Type)?.SetValue(instance, claim.Value);
            return instance;
        }

        public TokenValidationParameters Parameters { get; init; } = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

        public string CreateToken(TIdentity source, IConfiguration configuration)
        {
            try
            {
                var authClaims = ReadableProps
                    .Select(x =>
                    {
                        var val = x.GetValue(source)?.ToString();
                        return val != null ? new Claim(x.Name, val) : null;
                    }).Where(x => x != null);

                //签名秘钥 可以放到json文件中
                var authSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["Authentication:JwtBearer:Issuer"],
                    audience: configuration["Authentication:JwtBearer:Audience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                    notBefore: DateTime.Now
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                //ignore
            }
            return null!;
        }
        public static TIdentity? FromToken(string token) 
        {
            try
            {
                return new JwtSecurityToken(token)
                    .Claims
                    .Aggregate((TIdentity)Type.RawInstance(), SetClaimValue);
            }
            catch
            {
                return default;
            }
        }
    }
}
