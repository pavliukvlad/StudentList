using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;


namespace StudentList.Fragments
{
    public class StudentsListFragment : ListFragment
    {
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            ListAdapter = new StudentsListAdapter(Activity, StudentsProvider.students);
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            ShowStudentInfo(position);
        }

        public override void OnStart()
        {
            base.OnStart();
            ListAdapter = new StudentsListAdapter(Activity, StudentsProvider.students);
            //var addNewStudentButton = Activity.FindViewById<Button>(Resource.Id.addStudentButton);

            //addNewStudentButton.Click += (sender, e) =>
            //  {
            //      var studentProfile = StudentProfileFragment.NewInstance(null);
            //      FragmentManager.BeginTransaction().Replace(Android.Resource.Id.Content, studentProfile).Commit();
            //  };
        }
        private void ShowStudentInfo(int position)
        {
            var intent = new Intent(Activity, typeof(StudentProfileActivity));
            intent.PutExtra("student_id", position);
            StartActivity(intent);
        }
    }
}