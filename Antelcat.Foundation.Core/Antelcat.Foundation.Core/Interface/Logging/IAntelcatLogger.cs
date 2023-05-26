using Antelcat.Foundation.Core.Enums;

namespace Antelcat.Foundation.Core.Interface.Logging;

public interface IAntelcatLogger
{
    void Log<TState>(LogLevel level, TState state, Exception exception, Func<TState, Exception, string> formatter);
}
public interface IAntelcatLogger<out TCategoryName> : IAntelcatLogger { }