using Feast.Foundation.Core.Implements.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Feast.Foundation.Server.Implements;

public class AutowiredControllerActivator : IControllerActivator
{
    public object Create(ControllerContext? context)
    {
        if (context == null)
            throw new ArgumentNullException($"{nameof(ControllerContext)} is null");
        var type = context.ActionDescriptor.ControllerTypeInfo.AsType();
        var provider = context.HttpContext.RequestServices;
        if (provider is not AutowiredServiceProvider)
            provider = new AutowiredServiceProvider(context.HttpContext.RequestServices);
        return provider.GetRequiredService(type);
    }

    public void Release(ControllerContext? context, object? controller)
    {
        if (context == null) throw new ArgumentNullException($"{nameof(ControllerContext)} is null");
        if (controller == null) throw new ArgumentNullException(nameof(controller));
        if (controller is IDisposable disposable) disposable.Dispose();
    }
}