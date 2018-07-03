using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using StudentList.Fragments;
using StudentList.Providers.Interfaces;
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
            var listFragment = new StudentListFragment(null);
            this.SupportFragmentManager.BeginTransaction().Add(Resource.Id.main_container, listFragment).Commit();
        }
    }
}
