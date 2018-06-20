using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Collections.Generic;
using Android.Support.V7.App;
using StudentList.Fragments;
using System.Threading.Tasks;

namespace StudentList
{
    //"@android:style/Theme.Material.Light"
    [Activity(Label = "@string/app_name", Theme = "@style/CustomTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            toolbar.InflateMenu(Resource.Menu.top_menu);
            var studentList = new StudentListFragment();

            SupportFragmentManager.BeginTransaction().Add(Resource.Id.main_container, studentList).Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
            ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
    }
}


