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
    [Activity(Label = "@string/app_name", Theme = "@style/CustomTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            ShowStudentList();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        private void ShowStudentList()
        {
            var listFragment = new StudentListFragment();
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.main_container, listFragment).Commit();
        }
    }
}
