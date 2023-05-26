namespace Antelcat.Foundation.Core.Implements.Loggers;

internal class LoggerFactory : LoggerConfig
{
    private readonly LoggerConfig config;
    internal LoggerFactory(LoggerConfig? config)
    {
        this.config = config ?? new LoggerConfig();
    }
    internal override void Initialize<TCategory>(AntelcatLogger<TCategory> logger)
    {
        //TODO Extra initialize
        config.Initialize(logger);
    }
}