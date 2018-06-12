using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StudentList
{
    [Activity(Label = "StudentProfileActivity")]
    public class StudentProfileActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var studentId = Intent.Extras.GetInt("student_id", 0);
            var newStudent = Intent.Extras.GetBoolean("new_student", false);
            var studentDetails = Fragments.StudentProfileFragment.NewInstance(studentId, newStudent);
            FragmentManager.BeginTransaction().Replace(Android.Resource.Id.Content, studentDetails).Commit();
        }
    }
}