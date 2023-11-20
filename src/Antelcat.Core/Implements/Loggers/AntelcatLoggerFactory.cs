using Antelcat.Core.Interface.Logging;
using Microsoft.Extensions.Logging;

namespace Antelcat.Core.Implements.Loggers;

internal class AntelcatLoggerFactory : LoggerConfig , IAntelcatLoggerFactory
{
    private readonly LoggerConfig config;
    internal AntelcatLoggerFactory(LoggerConfig? config)
    {
        this.config = config ?? new LoggerConfig();
    }
    internal override void Initialize(AntelcatLogger logger)
    {
        //TODO Extra initialize
        config.Initialize(logger);
    }

    public void Dispose()
    {
        
    }

    public ILogger CreateLogger(string categoryName) => new AntelcatLogger(this, categoryName);

    public void AddProvider(ILoggerProvider provider)
    {
        
    }
}