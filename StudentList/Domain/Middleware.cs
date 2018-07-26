using System;

namespace StudentList.Domain
{
    public delegate Func<Dispatcher, Dispatcher> Middleware<TState>(IStore<TState> state);
}
