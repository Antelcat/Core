using System.ComponentModel;

namespace Antelcat.Core.Interface;

public interface IStateMachine<out TState> : INotifyPropertyChanged
{
    TState State { get; }
}