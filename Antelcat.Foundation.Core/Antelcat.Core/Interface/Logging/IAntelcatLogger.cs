using Antelcat.Core.Enums;

namespace Antelcat.Core.Interface.Logging;

public interface IAntelcatLogger
{
    void Log<TState>(LogLevel level, TState state, Exception exception, Func<TState, Exception, string> formatter);
}
public interface IAntelcatLogger<out TCategoryName> : IAntelcatLogger { }