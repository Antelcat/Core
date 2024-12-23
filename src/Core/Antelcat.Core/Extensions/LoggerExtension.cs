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
            .RegisterLogger();
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
            .AddSingleton<IAntelcatLoggerFactory>(x => new AntelcatLoggerFactory(configFactory(x)))
            .RegisterLogger();
        return replaceILogger
            ? collection.Replace(
                new ServiceDescriptor(typeof(ILogger<>), typeof(AntelcatLogger<>), ServiceLifetime.Singleton))
            : collection;
    }

    private static IServiceCollection RegisterLogger(this IServiceCollection collection) =>
        collection
            .AddSingleton(typeof(IAntelcatLogger<>), typeof(AntelcatLogger<>))
            .AddScoped<IAntelcatLogger, AntelcatLogger>();
}