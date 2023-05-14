using Feast.Foundation.Server.Filters;
using Feast.Foundation.Server.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Feast.Foundation.Server.Extensions
{
    public static partial class ServiceExtension
    {
        public static void ConfigureJwt<TIdentity>(
           this IServiceCollection services,
           Action<TokenValidationParameters>? configure = null,
           Func<TIdentity, Task>? validation = null,
           Func<JwtBearerChallengeContext, string>? failed = null)
        {
            var config = new JwtConfigure<TIdentity>(configure);
            services
                .AddSingleton(config)
                .AddAuthentication(options =>
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
                                if (JwtExtension<TIdentity>.FromToken(token) == null)
                                {
                                    context.Fail(
                                        new NullReferenceException($"Cannot resolve {typeof(TIdentity)} from token"));
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

    public static partial class ServiceExtension
    {
        public static IServiceCollection AddJwtSwaggerGen(this IServiceCollection collection)
        {
            return collection.AddSwaggerGen(o =>
            {
                o.OperationFilter<AuthorizationFilter>();
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Jwt Token Format like [ Bearer {Token} ]",
                    Name = nameof(Authorization), 
                    In = ParameterLocation.Header, 
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }
    }
}
