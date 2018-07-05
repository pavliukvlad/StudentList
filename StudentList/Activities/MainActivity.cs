using Android.App;
using Android.OS;
using Android.Support.V7.App;
using StudentList.Fragments;
using StudentList.Model;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace StudentList
{
    [Activity(Label = "@string/app_name", Theme = "@style/CustomTheme", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
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
