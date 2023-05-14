using System.ComponentModel;

namespace Feast.Foundation.Core.Interface
{
    public interface IStateMachine<out TState> : INotifyPropertyChanged
    {
        TState State { get; }
    }
}
