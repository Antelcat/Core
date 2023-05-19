using System.Reflection;

namespace Feast.Foundation.Core.Interface.IL;

public interface ILMethod
{
    object? Invoke(object? target, params object?[]? parameters);
}