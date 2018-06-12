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
using StudentList;

namespace StudentList.Fragments
{
    public class StudentProfileFragment : Fragment
    {
        int StudentId => Arguments.GetInt("student_id", 0);
        bool NewStudent => Arguments.GetBoolean("new_student", false);

        StudentsProvider studentProvider;

        public static StudentProfileFragment NewInstance(int studentId, bool newStudent)
        {
            var bundle = new Bundle();
            bundle.PutInt("student_id", studentId);
            bundle.PutBoolean("new_student", newStudent);
            var obj = new StudentProfileFragment() { Arguments = bundle };
            return obj;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            studentProvider = new StudentsProvider();

            var view = LayoutInflater.Inflate(Resource.Layout.student_profile, null);

            var saveButton = view.FindViewById<Button>(Resource.Id.save_changes_btn);
            var nameEditText = view.FindViewById<EditText>(Resource.Id.name_edittext);
            var ageEditText = view.FindViewById<EditText>(Resource.Id.age_edittext);
            var uniEditText = view.FindViewById<EditText>(Resource.Id.uni_edittext);
            var groupEditText = view.FindViewById<EditText>(Resource.Id.group_edittext);


            nameEditText.Text = NewStudent ? "" : studentProvider[StudentId].Name;
            ageEditText.Text = NewStudent ? "" : studentProvider[StudentId].Age.ToString();
            uniEditText.Text = NewStudent ? "" : studentProvider[StudentId].University;
            groupEditText.Text = NewStudent ? "" : studentProvider[StudentId].GroupName;
                        
            saveButton.Click += (sender, e) =>
            {
                var name = nameEditText.Text;
                var age = Convert.ToInt32(ageEditText.Text);
                var uni = uniEditText.Text;
                var group = groupEditText.Text;

                if (NewStudent)
                {
                    var student = new Student() { Name = name, Age = age, University = uni, GroupName = group };
                    studentProvider.AddNewStudent(student);
                }
                else
                {
                    studentProvider[StudentId].Name = name;
                    studentProvider[StudentId].Age = age;
                    studentProvider[StudentId].GroupName = group;
                    studentProvider[StudentId].University = uni;
                }

                ShowStudentList();
            };

            return view;
        }

        private void ShowStudentList()
        {
            var intent = new Intent(Activity, typeof(MainActivity));
            Activity.StartActivity(intent);
        }
    }
}