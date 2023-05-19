using Antelcat.Foundation.Core.Enums;

namespace Antelcat.Foundation.Core.Interface.Logging
{
    public interface IFeastLogger
    {
        void Log<TState>(LogLevel level, TState state, Exception exception, Func<TState, Exception, string> formatter);
    }
    public interface IFeastLogger<out TCategoryName> : IFeastLogger { }
}
