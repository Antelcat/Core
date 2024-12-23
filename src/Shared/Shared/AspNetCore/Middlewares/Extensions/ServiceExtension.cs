using Antelcat.Server.Configs;
using Antelcat.Server.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Server.Extensions;

public partial class ServiceExtension
{
    public static IMvcBuilder AddAntelcatFilters(this IMvcBuilder builder)
    {
        var filterConfig = AntelcatFilterConfig.Default;
        builder.Services.AddSingleton(filterConfig);
        return builder.AddAntelcatFiltersInternal();
    }

    public static IMvcBuilder AddAntelcatFilters(this IMvcBuilder builder,
        Action<AntelcatFilterConfig> config)
    {
        var filterConfig = AntelcatFilterConfig.Default;
        config.Invoke(filterConfig);
        builder.Services.AddSingleton(filterConfig);
        return builder.AddAntelcatFiltersInternal();
    }

    public static IMvcBuilder AddAntelcatFilters(this IMvcBuilder builder,
        Func<AntelcatFilterConfig> config)
    {
        builder.Services.AddSingleton(config());
        return builder.AddAntelcatFiltersInternal();
    }

    public static IMvcBuilder AddAntelcatFilters(this IMvcBuilder builder,
        Func<IServiceProvider, AntelcatFilterConfig> config)
    {
        builder.Services.AddSingleton(config);
        return builder.AddAntelcatFiltersInternal();
    }

    public static IMvcBuilder AddAntelcatFilters(this IMvcBuilder builder,
        Action<AntelcatFilterConfig, IServiceProvider> config)
    {
        var filterConfig = AntelcatFilterConfig.Default;
        builder.Services.AddSingleton(x =>
        {
            config.Invoke(filterConfig, x);
            return filterConfig;
        });
        return builder.AddAntelcatFiltersInternal();
    }

    private static IMvcBuilder AddAntelcatFiltersInternal(this IMvcBuilder builder) =>
        builder.AddMvcOptions(static x => { x.Filters.Add<ExceptionHandlerFilter>(); });
}