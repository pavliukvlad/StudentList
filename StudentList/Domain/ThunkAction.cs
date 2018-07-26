using System;

namespace StudentList.Domain
{
    public delegate void ThunkAction<in TState>(Dispatcher dispatcher, Func<TState> getState);
}
