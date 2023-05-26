using Antelcat.Foundation.Core.Enums;
using Antelcat.Foundation.Core.Implements.Loggers;
using Antelcat.Foundation.Core.Interface.Logging;
using Antelcat.Foundation.Core.Structs;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Foundation.Core.Extensions;

/// <summary>
/// IAntelcatLogger extension methods for common scenarios.
/// </summary>
public static class LoggerExtension
{
    private static readonly Func<FormattedLogValues, Exception, string> MessageFormatter = MessageFormatterHandler;

    //------------------------------------------DEBUG------------------------------------------//
       
    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogDebug(0, "Processing request from {Address}", address)</example>
    public static void LogDebug(this IAntelcatLogger logger,  string message, params object[] args)
    {
        logger.Log(LogLevel.Debug, message, args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogDebug(exception, "Error while processing request from {Address}", address)</example>
    public static void LogDebug(this IAntelcatLogger logger, Exception exception, string message, params object[] args)
    {
        logger.Log(LogLevel.Debug, exception, message, args);
    }

    //------------------------------------------TRACE------------------------------------------//

    /// <summary>
    /// Formats and writes a trace log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", address)</example>
    public static void LogTrace(this IAntelcatLogger logger, Exception exception, string message, params object[] args)
    {
        logger.Log(LogLevel.Trace, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a trace log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogTrace("Processing request from {Address}", address)</example>
    public static void LogTrace(this IAntelcatLogger logger, string message, params object[] args)
    {
        logger.Log(LogLevel.Trace, message, args);
    }

    //------------------------------------------INFORMATION------------------------------------------//

    /// <summary>
    /// Formats and writes an informational log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
    public static void LogInformation(this IAntelcatLogger logger, Exception exception, string message, params object[] args)
    {
        logger.Log(LogLevel.Information, exception, message, args);
    }

    /// <summary>
    /// Formats and writes an informational log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
    public static void LogInformation(this IAntelcatLogger logger, string message, params object[] args)
    {
        logger.Log(LogLevel.Information, message, args);
    }

    //------------------------------------------WARNING------------------------------------------//

    /// <summary>
    /// Formats and writes a warning log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogWarning(exception, "Error while processing request from {Address}", address)</example>
    public static void LogWarning(this IAntelcatLogger logger, Exception exception, string message, params object[] args)
    {
        logger.Log(LogLevel.Warning, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a warning log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogWarning("Processing request from {Address}", address)</example>
    public static void LogWarning(this IAntelcatLogger logger, string message, params object[] args)
    {
        logger.Log(LogLevel.Warning, message, args);
    }

    //------------------------------------------ERROR------------------------------------------//

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogError(exception, "Error while processing request from {Address}", address)</example>
    public static void LogError(this IAntelcatLogger logger, Exception exception, string message, params object[] args)
    {
        logger.Log(LogLevel.Error, exception, message, args);
    }

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogError("Processing request from {Address}", address)</example>
    public static void LogError(this IAntelcatLogger logger, string message, params object[] args)
    {
        logger.Log(LogLevel.Error, message, args);
    }

    //------------------------------------------CRITICAL------------------------------------------//

    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogCritical(exception, "Error while processing request from {Address}", address)</example>
    public static void LogCritical(this IAntelcatLogger logger, Exception exception, string message, params object[] args)
    {
        logger.Log(LogLevel.Critical, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogCritical("Processing request from {Address}", address)</example>
    public static void LogCritical(this IAntelcatLogger logger, string message, params object[] args)
    {
        logger.Log(LogLevel.Critical, message, args);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public static void Log(this IAntelcatLogger logger, LogLevel logLevel,  string message, params object[] args)
    {
        logger.Log(logLevel, null!, message, args);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logger">The <see cref="IAntelcatLogger"/> to write to.</param>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public static void Log(this IAntelcatLogger logger, LogLevel logLevel,Exception exception, string message, params object[] args)
    {
        if (logger == null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        logger.Log(logLevel,  new FormattedLogValues(message, args), exception, MessageFormatter);
    }


    //------------------------------------------HELPERS------------------------------------------//

    private static string MessageFormatterHandler(FormattedLogValues state, Exception error)
    {
        return state.ToString();
    }


    //--------------------------- --------DependencyInjection------------------------------------//

    public static IServiceCollection AddAntelcatLogger(this IServiceCollection collection, LoggerConfig? config = null) => 
        collection.AddSingleton( _ => new LoggerFactory(config))
            .AddSingleton(typeof(IAntelcatLogger<>), typeof(AntelcatLogger<>));

    public static IServiceCollection AddAntelcatLogger(this IServiceCollection collection,
        Func<IServiceProvider, LoggerConfig> configFactory) =>
        collection.AddSingleton(p => new LoggerFactory(configFactory(p)))
            .AddSingleton(typeof(IAntelcatLogger<>), typeof(AntelcatLogger<>));
}