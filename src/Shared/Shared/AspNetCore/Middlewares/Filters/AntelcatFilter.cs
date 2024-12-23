using Antelcat.Server.Configs;

namespace Antelcat.Server.Filters;

public abstract class AntelcatFilter(AntelcatFilterConfig config)
{
    protected AntelcatFilterConfig Config { get; } = config;
}