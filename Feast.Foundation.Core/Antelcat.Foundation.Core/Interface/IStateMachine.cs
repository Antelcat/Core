using System.ComponentModel;

namespace Antelcat.Foundation.Core.Interface
{
    public interface IStateMachine<out TState> : INotifyPropertyChanged
    {
        TState State { get; }
    }
}
