using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;

namespace StudentList.Common.Dialogs
{
    public class LoadingDialog : Dialog
    {
        private Context context;

        public LoadingDialog(Context context)
            : base(context)
        {
            this.context = context;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetCancelable(false);
            this.SetCanceledOnTouchOutside(false);

            this.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            var progressBar = new ProgressBar(this.context)
            {
                Indeterminate = true
            };

            var frameLayout = new FrameLayout(this.context);
            frameLayout.AddView(progressBar);

            this.SetContentView(frameLayout);
        }
    }
}
