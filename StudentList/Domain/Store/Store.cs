using System;
using System.Reactive.Subjects;

namespace StudentList.Domain.Store
{
    public class Store<TState> : IStore<TState>, IDisposable
    {
        private readonly BehaviorSubject<TState> subjectState;
        private readonly Reducer<TState> reducer;
        private readonly Dispatcher dispatcher;

        public Store(Reducer<TState> reducer, TState state, params Middleware<TState>[] middlewares)
        {
            this.reducer = reducer ?? throw new ArgumentNullException(nameof(reducer));
            this.subjectState = new BehaviorSubject<TState>(state);
            this.dispatcher = this.ApplyDispatcherToMiddleware(this.InitialDispatcher, middlewares);
        }

        public TState GetState()
        {
            if (this.subjectState.IsDisposed)
            {
                throw new ObjectDisposedException(nameof(Store<TState>));
            }

            return this.subjectState.Value;
        }

        public object InitialDispatcher(object action)
        {
            this.subjectState.OnNext(this.reducer(this.GetState(), action));
            return action;
        }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            if (this.subjectState.IsDisposed)
            {
                throw new ObjectDisposedException(nameof(Store<TState>));
            }

            return this.subjectState.Subscribe(observer);
        }

        public object Dispatch(object action)
        {
            return this.dispatcher(action);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.subjectState != null)
                {
                    this.subjectState.Dispose();
                }
            }
        }

        private Dispatcher ApplyDispatcherToMiddleware(Dispatcher dispatcher, params Middleware<TState>[] middlewares)
        {
            foreach (var middleware in middlewares)
            {
                dispatcher = middleware(this)(dispatcher);
            }

            return dispatcher;
        }
    }
}
