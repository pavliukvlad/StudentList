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
        private string StudentId => Arguments.GetString(IntentConstant.StudentId, "");
        private bool NewStudent => Arguments.GetBoolean(IntentConstant.NewStudent, false);

        private IStudentRepository studentRepository;

        private Button saveButton;
        private EditText nameEditText;
        private EditText birthdateEditText;
        private EditText uniEditText;
        private EditText groupEditText;

        public static StudentProfileFragment NewInstance(string studentId, bool newStudent)
        {
            var bundle = new Bundle();
            bundle.PutString(IntentConstant.StudentId, studentId);
            bundle.PutBoolean(IntentConstant.NewStudent, newStudent);
            var obj = new StudentProfileFragment() { Arguments = bundle };
            return obj;
        }
  
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view =  inflater.Inflate(Resource.Layout.student_profile, null);

            saveButton = view.FindViewById<Button>(Resource.Id.save_changes_btn);
            nameEditText = view.FindViewById<EditText>(Resource.Id.name_edittext);
            birthdateEditText = view.FindViewById<EditText>(Resource.Id.birthdate_edittext);
            uniEditText = view.FindViewById<EditText>(Resource.Id.uni_edittext);
            groupEditText = view.FindViewById<EditText>(Resource.Id.group_edittext);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            studentRepository = new StudentsRepository();

            nameEditText.Text = NewStudent ? "" : studentRepository[StudentId].Name;
            birthdateEditText.Text = NewStudent ? "" : studentRepository[StudentId].Birthdate.ToShortDateString();
            uniEditText.Text = NewStudent ? "" : studentRepository[StudentId].University;
            groupEditText.Text = NewStudent ? "" : studentRepository[StudentId].GroupName;
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
            var id = Guid.NewGuid().ToString();
            var name = nameEditText.Text;
            var birthdate = Convert.ToDateTime(birthdateEditText.Text);
            var uni = uniEditText.Text;
            var group = groupEditText.Text;

            if (NewStudent)
            {
                var student = new Student() { Id = id, Name = name, Birthdate = birthdate, University = uni, GroupName = group };
                studentRepository.AddNewStudent(student);
            }
            else
            {
                studentRepository.ChangeStudentById(StudentId, name, birthdate, group, uni);
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