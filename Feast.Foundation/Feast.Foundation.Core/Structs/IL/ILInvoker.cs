using System.Reflection;
using Feast.Foundation.Core.Extensions;
using Feast.Foundation.Core.Interface.IL;

namespace Feast.Foundation.Core.Structs.IL;

public readonly struct ILInvoker : ILMethod
{
    public readonly MethodInfo Method;
    private readonly Invoker<object, object> invoker;

    public ILInvoker(MethodInfo method)
    {
        Method = method;
        invoker = method.CreateInvoker();
    }

    public static implicit operator ILInvoker(MethodInfo method) => new ILInvoker(method);
    public object? Invoke(object? target, params object?[]? parameters) => invoker(target, parameters);
}