using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Plugin.Permissions;
using StudentList.Fragments;
using StudentList.Models;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace StudentList
{
    [Activity(Label = "@string/app_name", Theme = "@style/CustomTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.activity_main);

            var toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.SetSupportActionBar(toolbar);
            this.ShowStudentList();
        }

        private void ShowStudentList()
        {
            var listFragment = new StudentListFragment(StudentFilter.Default);
            this.SupportFragmentManager.BeginTransaction().Add(Resource.Id.main_container, listFragment).Commit();
        }
    }
}
