using Antelcat.Server.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Antelcat.Server.Filters;

public class ExceptionHandlerFilter(ILogger<ExceptionHandlerFilter> logger, AntelcatFilterConfig config)
    : AntelcatFilter(config), IAsyncExceptionFilter
{
    private readonly ILogger<ExceptionHandlerFilter> logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var response = context.HttpContext.Response;
        if (response.HasStarted) return;
        response.Clear();
        response.StatusCode = StatusCodes.Status500InternalServerError;
        await Config.ExecuteHandler(context.Exception, response);
        await response.CompleteAsync();
        logger.LogError("{Exception}", context.Exception);
    }
    
}