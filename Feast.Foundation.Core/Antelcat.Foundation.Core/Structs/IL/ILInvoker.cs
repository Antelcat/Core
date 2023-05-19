using System.Reflection;
using Antelcat.Foundation.Core.Extensions;
using Antelcat.Foundation.Core.Interface.IL;

namespace Antelcat.Foundation.Core.Structs.IL;

public readonly struct ILInvoker : ILMethod
{
    public readonly MethodInfo Method;
    public readonly Invoker<object, object> Invoker;

    public ILInvoker(MethodInfo method)
    {
        Method = method;
        Invoker = method.CreateInvoker();
    }

    public static implicit operator ILInvoker(MethodInfo method) => new ILInvoker(method);
    public object? Invoke(object? target, params object?[]? parameters) => Invoker(target, parameters);
}