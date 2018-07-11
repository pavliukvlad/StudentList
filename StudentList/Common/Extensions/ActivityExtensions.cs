using System.Threading.Tasks;
using Android.App;
using StudentList.Common.Dialogs;

namespace StudentList.Extensions
{
    public static class ActivityExtensions
    {
        public static async Task RunMethodWithLoaderAsync(this Activity activity, Task task)
        {
            var loadingDialog = new LoadingDialog(activity);

            loadingDialog.Show();
            await task;
            loadingDialog.Hide();
        }
    }
}
