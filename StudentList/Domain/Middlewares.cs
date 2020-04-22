using System;

namespace StudentList.Domain
{
    public static class Middlewares
    {
        public static Func<Dispatcher, Dispatcher> ThunkMiddleware<TState>(IStore<TState> store)
        {
            return (Dispatcher next) => (object action) =>
            {
                if (action is ThunkAction<TState> thunkAction)
                {
                    thunkAction?.Invoke(next, store.GetState);
                    return thunkAction;
                }
              
                return next(action);
            };
        }
    }
}
