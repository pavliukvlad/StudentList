using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using StudentList;
using StudentList.Constants;
using StudentList.Model;
using StudentList.Providers.Interfaces;
using System.Threading.Tasks;

namespace StudentList.Fragments
{
    public class StudentProfileFragment : Android.Support.V4.App.Fragment
    {
        private string StudentId => Arguments.GetString(IntentConstant.StudentId, string.Empty);
        private bool NewStudent => Arguments.GetBoolean(IntentConstant.NewStudent, false);

        private IStudentRepository studentRepository;

        private Button saveButton;
        private TextInputLayout nameEditText;
        private TextInputLayout birthdateEditText;
        private TextInputLayout universityEditText;
        private TextInputLayout groupEditText;
        private TextInputLayout phoneEditText;

        public static StudentProfileFragment NewInstance(string studentId, bool newStudent)
        {
            var bundle = new Bundle();
            bundle.PutString(IntentConstant.StudentId, studentId);
            bundle.PutBoolean(IntentConstant.NewStudent, newStudent);
            var obj = new StudentProfileFragment() { Arguments = bundle };
            return obj;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.student_profile, null);

            saveButton = view.FindViewById<Button>(Resource.Id.save_changes_btn);
            nameEditText = view.FindViewById<TextInputLayout>(Resource.Id.name_layout);
            birthdateEditText = view.FindViewById<TextInputLayout>(Resource.Id.birthdate_layout);
            universityEditText = view.FindViewById<TextInputLayout>(Resource.Id.uni_layout);
            groupEditText = view.FindViewById<TextInputLayout>(Resource.Id.group_layout);
            phoneEditText = view.FindViewById<TextInputLayout>(Resource.Id.phone_layout);

            return view;
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            studentRepository = new StudentsRepository();
            var selectedStudent = await this.studentRepository.GetStudentById(StudentId);

            ((AppCompatActivity)Activity).SupportActionBar.Title = NewStudent ? GetString(Resource.String.create_student_title) : 
                GetString(Resource.String.edit_student_title) + "" + selectedStudent.Name;

            saveButton.Text = NewStudent ? GetString(Resource.String.add_new_student_text) : GetString(Resource.String.save_changes_text);
            nameEditText.EditText.Text = NewStudent ? "" : selectedStudent.Name;
            birthdateEditText.EditText.Text = NewStudent ? "" : selectedStudent.Birthdate.ToShortDateString();
            universityEditText.EditText.Text = NewStudent ? "" : selectedStudent.University;
            groupEditText.EditText.Text = NewStudent ? "" : selectedStudent.GroupName;
            phoneEditText.EditText.Text = NewStudent ? "" : selectedStudent.Phone;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();
            inflater.Inflate(Resource.Menu.confirm_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_confirm:
                    Confirm();
                    return true;
                case Resource.Id.action_reset:
                    Reset();
                    return true;
                case Android.Resource.Id.Home:
                    Activity.OnBackPressed();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnStart()
        {
            base.OnStart();
            saveButton.Click += SaveButton_Click;
            birthdateEditText.EditText.Touch += OnBirthdateEditTextTouch;
            birthdateEditText.EditText.FocusChange += OnBirthdateEditTextFocus;
            DisplayHomeUp(true);
        }

        public override void OnStop()
        {
            base.OnStop();
            saveButton.Click -= SaveButton_Click;
            birthdateEditText.Touch -= OnBirthdateEditTextTouch;
            birthdateEditText.EditText.FocusChange -= OnBirthdateEditTextFocus;
            DisplayHomeUp(false);
        }

        private void OnBirthdateEditTextTouch(object sender, View.TouchEventArgs e)
        {
            var datePicker = new DatePickerDialog(Context, DataSetPickerDialog, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
            if (e.Event.Action == MotionEventActions.Down)
            {
                datePicker.Show();
            }
        }

        private void OnBirthdateEditTextFocus(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                var datePicker = new DatePickerDialog(Context, DataSetPickerDialog, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
                datePicker.Show();
            }
        }

        private void DataSetPickerDialog(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            birthdateEditText.EditText.Text = e.Date.ToShortDateString();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Confirm();
        }

        private void Confirm()
        {
            if (Validate(nameEditText, birthdateEditText, groupEditText, universityEditText))
                return;
            string id = Guid.NewGuid().ToString();
            string name = nameEditText.EditText.Text.TrimEnd();
            DateTime birthdate = Convert.ToDateTime(birthdateEditText.EditText.Text);
            string uni = universityEditText.EditText.Text.TrimEnd();
            string group = groupEditText.EditText.Text.TrimEnd();
            string phone = phoneEditText.EditText.Text.TrimEnd().Length == 0 ? null : phoneEditText.EditText.Text.TrimEnd();

            if (NewStudent)
            {
                var student = new Student() { Id = id, Name = name, Birthdate = birthdate, University = uni, GroupName = group, Phone = phone };
                studentRepository.AddNewStudent(student);
            }
            else
            {
                studentRepository.ChangeStudentById(StudentId, name, birthdate, group, uni, phone);
            }

            this.Activity.OnBackPressed();
        }

        private void DisplayHomeUp(bool trigger)
        {
            ((AppCompatActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(trigger && Activity.SupportFragmentManager.BackStackEntryCount > 0);
        }

        private void Reset()
        {
            nameEditText.EditText.Text = string.Empty;
            groupEditText.EditText.Text = string.Empty;
            birthdateEditText.EditText.Text = string.Empty;
            universityEditText.EditText.Text = string.Empty;
            phoneEditText.EditText.Text = string.Empty;
        }

        private bool Validate(params TextInputLayout[] inputLayout)
        {
            bool validation = false;

            for (int i = 0; i < inputLayout.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(inputLayout[i].EditText.Text) || string.IsNullOrEmpty(inputLayout[i].EditText.Text))
                {
                    inputLayout[i].Error = " ";
                    validation = true;
                }
                else
                {
                    inputLayout[i].Error = string.Empty;
                }
            }

            return validation;
        }
    }
}
