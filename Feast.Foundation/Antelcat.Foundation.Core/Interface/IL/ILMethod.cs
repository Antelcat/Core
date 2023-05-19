using System.Reflection;

namespace Antelcat.Foundation.Core.Interface.IL;

public interface ILMethod
{
    object? Invoke(object? target, params object?[]? parameters);
}