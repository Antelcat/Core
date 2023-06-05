using System.Reflection;
using Antelcat.Core.Extensions;
using Antelcat.Core.Interface.IL;
using Antelcat.Extensions;

namespace Antelcat.Core.Structs.IL;

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