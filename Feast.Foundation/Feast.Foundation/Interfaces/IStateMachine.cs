using System.ComponentModel;

namespace Feast.Foundation.Interfaces
{
    public interface IStateMachine<out TState> : INotifyPropertyChanged
    {
        TState State { get; }
    }
}
