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
using StudentList.Constants;
using StudentList.Model;
using StudentList.Providers.Interfaces;

namespace StudentList.Fragments
{
    public class StudentProfileFragment : Android.Support.V4.App.Fragment
    {
        private int StudentId => Arguments.GetInt(IntentConstant.StudentId, 0);
        private bool NewStudent => Arguments.GetBoolean(IntentConstant.NewStudent, false);

        private IStudentRepository studentProvider;

        private Button saveButton;
        private EditText nameEditText;
        private EditText ageEditText;
        private EditText uniEditText;
        private EditText groupEditText;

        public static StudentProfileFragment NewInstance(int studentId, bool newStudent)
        {
            var bundle = new Bundle();
            bundle.PutInt(IntentConstant.StudentId, studentId);
            bundle.PutBoolean(IntentConstant.NewStudent, newStudent);
            var obj = new StudentProfileFragment() { Arguments = bundle };
            return obj;
        }
  
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.student_profile, null); 
        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
             saveButton = view.FindViewById<Button>(Resource.Id.save_changes_btn);
             nameEditText = view.FindViewById<EditText>(Resource.Id.name_edittext);
             ageEditText = view.FindViewById<EditText>(Resource.Id.age_edittext);
             uniEditText = view.FindViewById<EditText>(Resource.Id.uni_edittext);
             groupEditText = view.FindViewById<EditText>(Resource.Id.group_edittext);

            studentProvider = new StudentsRepository();

            nameEditText.Text = NewStudent ? "" : studentProvider[StudentId].Name;
            ageEditText.Text = NewStudent ? "" : studentProvider[StudentId].Age.ToString();
            uniEditText.Text = NewStudent ? "" : studentProvider[StudentId].University;
            groupEditText.Text = NewStudent ? "" : studentProvider[StudentId].GroupName;
        }

        public override void OnStart()
        {
            base.OnStart();
            saveButton.Click += SaveButton_Click;
        }
        public override void OnStop()
        {
            base.OnStop();
            saveButton.Click -= SaveButton_Click;
        }

        private void SaveButton_Click(object sender, EventArgs e)
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
        }

        private void ShowStudentList()
        {
            var intent = new Intent(Activity, typeof(MainActivity));
            Activity.StartActivity(intent);
        }
    }
}