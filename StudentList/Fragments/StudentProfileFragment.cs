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
        StudentsProvider studentProvider;

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


            if(StudentId==-1)
            {
                nameEditText.Text = "";
                ageEditText.Text = "";
                uniEditText.Text = "";
                groupEditText.Text = "";
                saveButton.Text = "Add new student";
            }
            else
            {
                nameEditText.Text = studentProvider[StudentId].Name;
                ageEditText.Text = studentProvider[StudentId].Age.ToString();
                uniEditText.Text = studentProvider[StudentId].University;
                groupEditText.Text = studentProvider[StudentId].GroupName;
            }
            
            
            saveButton.Click += (sender, e) =>
            {
                var name = nameEditText.Text;
                var age = Convert.ToInt32(ageEditText.Text);
                var uni = uniEditText.Text;
                var group = groupEditText.Text;

                if (StudentId == -1)
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

                var intent = new Intent(Activity, typeof(MainActivity));
                Activity.StartActivity(intent);

            };

            return view;
        }

        public static StudentProfileFragment NewInstance(int? studentId)
        {
            var bundle = new Bundle();
            bundle.PutInt("student_id", studentId ?? -1);
            var obj = new StudentProfileFragment() { Arguments = bundle };
            return obj;
        }
    }
}