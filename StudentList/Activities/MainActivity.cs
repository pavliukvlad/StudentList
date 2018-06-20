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
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            //toolbar.InflateMenu(Resource.Menu.top_menu);
            var studentList = new StudentListFragment();

            SupportFragmentManager.BeginTransaction().Add(Resource.Id.main_container, studentList).Commit();
        }

       
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_add_student:
                    ShowStudentInfo(string.Empty, true);
                    break;
                case Resource.Id.menu_search:
                    FilterStudents();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void ShowStudentInfo(string studentId, bool newStudent = false)
        {
            var studentDetails = StudentProfileFragment.NewInstance(studentId, newStudent);
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.main_container, studentDetails).AddToBackStack(null).Commit();
        }

        private void FilterStudents()
        {
            FilterStudentsFragment filterStudents = new FilterStudentsFragment();
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.main_container, filterStudents).AddToBackStack(null).Commit();
        }
    }
}


