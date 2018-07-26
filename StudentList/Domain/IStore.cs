using System;

namespace StudentList.Domain
{
    public interface IStore<out TState> : IObservable<TState>
    {
        TState GetState();

        object Dispatch(object action);
    }
}
