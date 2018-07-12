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

        public static async Task<T> RunMethodWithLoaderAsync<T>(this Activity activity, Task<T> task)
        {
            var loadingDialog = new LoadingDialog(activity);

            loadingDialog.Show();
            var result = await task;
            loadingDialog.Hide();

            return result;
        }
    }
}
