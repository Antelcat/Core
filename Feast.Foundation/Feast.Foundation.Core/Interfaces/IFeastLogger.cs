using Feast.Foundation.Enums;

namespace Feast.Foundation.Interfaces
{
    public interface IFeastLogger
    {
        void Log<TState>(LogLevel level, TState state, Exception exception, Func<TState, Exception, string> formatter);
    }
    public interface IFeastLogger<out TCategoryName> : IFeastLogger { }
}
