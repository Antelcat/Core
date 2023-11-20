using Antelcat.Core.Implements.Loggers;
using Antelcat.Core.Interface.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Core.Extensions;

/// <summary>
/// IAntelcatLogger extension methods for common scenarios.
/// </summary>
public static class LoggerExtension
{
    //------------------------------------DependencyInjection------------------------------------//
    public static IServiceCollection AddAntelcatLogger(this IServiceCollection collection,
        LoggerConfig? config = null) => collection
        .AddSingleton<IAntelcatLoggerFactory>(_ => new AntelcatLoggerFactory(config))
        .AddScoped<IAntelcatLogger, AntelcatLogger>()
        .AddSingleton(typeof(IAntelcatLogger<>), typeof(AntelcatLogger<>));

    public static IServiceCollection AddAntelcatLogger(this IServiceCollection collection,
        Func<IServiceProvider, LoggerConfig> configFactory) => collection
        .AddSingleton<IAntelcatLoggerFactory>(p => new AntelcatLoggerFactory(configFactory(p)))
        .AddScoped<IAntelcatLogger, AntelcatLogger>()
        .AddSingleton(typeof(IAntelcatLogger<>), typeof(AntelcatLogger<>));
}