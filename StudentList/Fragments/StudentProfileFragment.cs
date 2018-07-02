using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace StudentList.Fragments
{
    public class StudentProfileFragment : Android.Support.V4.App.Fragment
    {
        private string StudentId => this.Arguments.GetString(IntentConstant.StudentId, string.Empty);

        private bool NewStudent => this.Arguments.GetBoolean(IntentConstant.NewStudent, false);

        private IStudentRepository studentRepository;

        private Button saveButton;
        private TextInputLayout nameLayout;
        private TextInputLayout birthdateLayout;
        private TextInputLayout universityLayout;
        private TextInputLayout groupLayout;
        private TextInputLayout phoneLayout;

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

            this.studentRepository = new StudentsRepository();
            this.HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.student_profile, null);

            this.saveButton = view.FindViewById<Button>(Resource.Id.save_changes_btn);
            this.nameLayout = view.FindViewById<TextInputLayout>(Resource.Id.name_layout);
            this.birthdateLayout = view.FindViewById<TextInputLayout>(Resource.Id.birthdate_layout);
            this.universityLayout = view.FindViewById<TextInputLayout>(Resource.Id.uni_layout);
            this.groupLayout = view.FindViewById<TextInputLayout>(Resource.Id.group_layout);
            this.phoneLayout = view.FindViewById<TextInputLayout>(Resource.Id.phone_layout);

            return view;
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var selectedStudent = await this.studentRepository.GetStudentById(this.StudentId);

            ((AppCompatActivity)this.Activity).SupportActionBar.Title = this.NewStudent ? this.GetString(Resource.String.create_student_title)
                : this.GetString(Resource.String.edit_student_title) + " " + selectedStudent.Name;

            this.saveButton.Text = this.NewStudent ? this.GetString(Resource.String.add_new_student_text)
                : this.GetString(Resource.String.save_changes_text);
            this.nameLayout.EditText.Text = this.NewStudent ? string.Empty : selectedStudent.Name;
            this.birthdateLayout.EditText.Text = this.NewStudent ? string.Empty : selectedStudent.Birthdate.ToShortDateString();
            this.universityLayout.EditText.Text = this.NewStudent ? string.Empty : selectedStudent.University;
            this.groupLayout.EditText.Text = this.NewStudent ? string.Empty : selectedStudent.GroupName;
            this.phoneLayout.EditText.Text = this.NewStudent ? string.Empty : selectedStudent.Phone;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();
            inflater.Inflate(Resource.Menu.confirm_menu, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_confirm:
                    this.Confirm();
                    return true;
                case Resource.Id.action_reset:
                    this.Reset();
                    return true;
                case Android.Resource.Id.Home:
                    this.Activity.OnBackPressed();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnStart()
        {
            base.OnStart();

            this.saveButton.Click += this.SaveButton_Click;
            this.birthdateLayout.EditText.Touch += this.OnBirthdateEditTextTouch;
            this.birthdateLayout.EditText.FocusChange += this.OnBirthdateEditTextFocus;
            this.DisplayHomeUp(true);
        }

        public override void OnStop()
        {
            base.OnStop();

            this.saveButton.Click -= this.SaveButton_Click;
            this.birthdateLayout.Touch -= this.OnBirthdateEditTextTouch;
            this.birthdateLayout.EditText.FocusChange -= this.OnBirthdateEditTextFocus;
            this.DisplayHomeUp(false);
        }

        private void OnBirthdateEditTextTouch(object sender, View.TouchEventArgs e)
        {
            var datePicker = new DatePickerDialog(
                this.Context, this.DataSetPickerDialog, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);

            if (e.Event.Action == MotionEventActions.Down)
            {
                datePicker.Show();
            }
        }

        private void OnBirthdateEditTextFocus(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                var datePicker = new DatePickerDialog(
                    this.Context, this.DataSetPickerDialog, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
                datePicker.Show();
            }
        }

        private void DataSetPickerDialog(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            this.birthdateLayout.EditText.Text = e.Date.ToShortDateString();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.Confirm();
        }

        private void Confirm()
        {
            string name = this.nameLayout.EditText.Text.TrimEnd();
            string birthdate = this.birthdateLayout.EditText.Text;
            string uni = this.universityLayout.EditText.Text.TrimEnd();
            string group = this.groupLayout.EditText.Text.TrimEnd();
            string phone = this.phoneLayout.EditText.Text.TrimEnd().Length == 0 ? null
                : this.phoneLayout.EditText.Text.TrimEnd();

            if (this.Validate(name, birthdate, group, uni))
            {
                string id = Guid.NewGuid().ToString();

                if (this.NewStudent)
                {
                    var student = new Student()
                    {
                        Id = id,
                        Name = name,
                        Birthdate = Convert.ToDateTime(birthdate, CultureInfo.InvariantCulture),
                        University = uni,
                        GroupName = group,
                        Phone = phone
                    };

                    this.studentRepository.AddNewStudent(student);
                }
                else
                {
                    this.studentRepository.ChangeStudentById(
                        this.StudentId, name, Convert.ToDateTime(birthdate, CultureInfo.InvariantCulture), group, uni, phone);
                }
            }

            this.Activity.OnBackPressed();
        }

        private void DisplayHomeUp(bool trigger)
        {
            ((AppCompatActivity)this.Activity)
                .SupportActionBar
                .SetDisplayHomeAsUpEnabled(trigger && this.Activity.SupportFragmentManager.BackStackEntryCount > 0);
        }

        private void Reset()
        {
            this.nameLayout.EditText.Text = string.Empty;
            this.groupLayout.EditText.Text = string.Empty;
            this.birthdateLayout.EditText.Text = string.Empty;
            this.universityLayout.EditText.Text = string.Empty;
            this.phoneLayout.EditText.Text = string.Empty;
        }

        private bool Validate(string name, string birthdate, string group, string uni)
        {
            bool validation = false;

            this.IsEmptyOrWhiteSpace(name, this.nameLayout, ref validation);
            this.IsEmptyOrWhiteSpace(birthdate, this.birthdateLayout, ref validation);
            this.IsEmptyOrWhiteSpace(group, this.groupLayout, ref validation);
            this.IsEmptyOrWhiteSpace(uni, this.universityLayout,  ref validation);

            return validation;
        }

        private void IsEmptyOrWhiteSpace(string name, TextInputLayout targetLayout, ref bool validation)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                targetLayout.Error = " ";
                validation = true;
            }
            else
            {
                targetLayout.Error = string.Empty;
            }
        }

        //private bool Validate(params TextInputLayout[] inputLayout)
        //{
        //    bool validation = false;

        //    for (int i = 0; i < inputLayout.Length; i++)
        //    {
        //        if (string.IsNullOrWhiteSpace(inputLayout[i].EditText.Text) || string.IsNullOrEmpty(inputLayout[i].EditText.Text))
        //        {
        //            inputLayout[i].Error = " ";
        //            validation = true;
        //        }
        //        else
        //        {
        //            inputLayout[i].Error = string.Empty;
        //        }
        //    }

        //    return validation;
        //}
    }
}
