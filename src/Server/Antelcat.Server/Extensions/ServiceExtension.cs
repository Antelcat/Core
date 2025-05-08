using System.Data;
using System.Net;
using Antelcat.Server.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Antelcat.Server.Extensions;

public static partial class ServiceExtension
{
    public static T Get<T>(this IServiceProvider serviceProvider) where T : notnull => serviceProvider.GetRequiredService<T>();
    
    public static IServiceCollection AddJwtSwaggerGen(this IServiceCollection collection,
        Action<SwaggerGenOptions>? config = null)
        => collection.AddSwaggerGen(o =>
        {
            o.OperationFilter<AuthorizationFilter>();
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Jwt Token Format like [ Bearer {Token} ]",
                Name        = nameof(Authorization),
                In          = ParameterLocation.Header,
                Type        = SecuritySchemeType.ApiKey
            });
            config?.Invoke(o);
        });

    public static IConfiguration GetConfiguration(this IServiceProvider serviceProvider) =>
        serviceProvider.GetRequiredService<IConfiguration>();

    public static IConfiguration GetConfiguration(this IServiceProvider serviceProvider, string field) =>
        serviceProvider.GetConfiguration().GetSection(field);

    public static IConfiguration GetConfiguration(this IServiceProvider serviceProvider, params string[] fields) =>
        serviceProvider.GetConfiguration(string.Join(':', fields));
    
    public static string GetString(this IServiceProvider serviceProvider, string field) =>
        serviceProvider.GetConfiguration()[field] 
        ?? throw new NoNullAllowedException($"Specified filed {field} is null");

    public static string GetString(this IServiceProvider serviceProvider, params string[] fields) =>
        serviceProvider.GetString(string.Join(':', fields));
    
    public static string GetString(this IConfiguration configuration, params string[] fields) =>
        configuration[string.Join(':', fields)]
        ?? throw new NoNullAllowedException($"Specified filed {string.Join(':', fields)} is null");

    public static IServiceCollection AddRegisteredHostedService<TService>(this IServiceCollection collection)
        where TService : class, IHostedService => collection.AddHostedService(x => x.GetRequiredService<TService>());
}