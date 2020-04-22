using System;
using Android.App;
using Android.Runtime;
using Plugin.CurrentActivity;
using StudentList.Domain;
using StudentList.Domain.States;
using StudentList.Domain.Store;

namespace StudentList
{
#if DEBUG
    [Application(Debuggable = true)]
#else
    [Application(Debuggable = false)]
#endif
    public class MainApplication : Application
    {
        public static IStore<ApplicationState> Store { get; private set; }

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);

            Store = new Store<ApplicationState>(
                ApplicationReducer.Reducer,
                new ApplicationState(),
                Middlewares.ThunkMiddleware);
        }
    }
}
