using Antelcat.Core.Implements.Loggers;
using Antelcat.Core.Interface.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Antelcat.Core.Extensions;

/// <summary>
/// IAntelcatLogger extension methods for common scenarios.
/// </summary>
public static class LoggerExtension
{
    //------------------------------------DependencyInjection------------------------------------//
    public static IServiceCollection AddAntelcatLogger(this IServiceCollection collection,
        bool replaceILogger = false,
        LoggerConfig? config = null)
    {
        collection
            .AddSingleton<IAntelcatLoggerFactory>(_ => new AntelcatLoggerFactory(config))
            .AddScoped<IAntelcatLogger, AntelcatLogger>()
            .AddSingleton(typeof(IAntelcatLogger<>), typeof(AntelcatLogger<>));
        return replaceILogger
            ? collection.Replace(
                new ServiceDescriptor(typeof(ILogger<>), typeof(AntelcatLogger<>), ServiceLifetime.Singleton))
            : collection;
    }

    public static IServiceCollection AddAntelcatLogger(this IServiceCollection collection,
        Func<IServiceProvider, LoggerConfig> configFactory,
        bool replaceILogger = false)
    {
        collection
            .AddSingleton<IAntelcatLoggerFactory>(p => new AntelcatLoggerFactory(configFactory(p)))
            .AddScoped<IAntelcatLogger, AntelcatLogger>()
            .AddSingleton(typeof(IAntelcatLogger<>), typeof(AntelcatLogger<>));
        return replaceILogger
            ? collection.Replace(
                new ServiceDescriptor(typeof(ILogger<>), typeof(AntelcatLogger<>), ServiceLifetime.Singleton))
            : collection;
    }
}