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
using StudentList.Models;
using StudentList.Providers.Interfaces;
using System.Drawing;
using Android.Media;
using Android.Graphics;
using Android.Provider;
using System.IO;
using Java.IO;
using Refractored.Controls;
using StudentList.Services;

namespace StudentList.Fragments
{
    public class StudentProfileFragment : Android.Support.V4.App.Fragment
    {
        private IStudentRepository studentRepository;
        private Dictionary<string, TextInputLayout> layouts;
        private Student selectedStudent;
        private Uri profilePhotoPath;
        private Bitmap image;

        private Button saveButton;
        private ProgressBar loadingProgressBar;
        private CircleImageView profilePhoto;
        private TextInputLayout nameLayout;
        private TextInputLayout photoLayout;
        private TextInputLayout birthdateLayout;
        private TextInputLayout universityLayout;
        private TextInputLayout groupLayout;
        private TextInputLayout phoneLayout;

        private string StudentId => this.Arguments.GetString(IntentConstant.StudentId, string.Empty);

        private bool NewStudent => this.Arguments.GetBoolean(IntentConstant.NewStudent, false);

        public static StudentProfileFragment NewInstance(string studentId, bool newStudent)
        {
            var bundle = new Bundle();
            bundle.PutString(IntentConstant.StudentId, studentId);
            bundle.PutBoolean(IntentConstant.NewStudent, newStudent);
            var obj = new StudentProfileFragment() { Arguments = bundle };
            return obj;
        }

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.studentRepository = new StudentsRepository();
            this.layouts = new Dictionary<string, TextInputLayout>();

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
            this.loadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.student_profile_progressbar);
            this.profilePhoto = view.FindViewById<CircleImageView>(Resource.Id.profile_image);

            this.layouts.Add("name", this.nameLayout);
            this.layouts.Add("birthdate", this.birthdateLayout);
            this.layouts.Add("group", this.groupLayout);
            this.layouts.Add("uni", this.universityLayout);
            this.layouts.Add("phone", this.phoneLayout);

            return view;
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            this.selectedStudent = await this.studentRepository.GetStudentById(this.StudentId);

            this.loadingProgressBar.Visibility = ViewStates.Invisible;

            this.saveButton.Text = this.NewStudent ? this.GetString(Resource.String.add_new_student_text)
               : this.GetString(Resource.String.save_changes_text);

            ((AppCompatActivity)this.Activity).SupportActionBar.Title = this.NewStudent ? this.GetString(Resource.String.create_student_title)
                : this.GetString(Resource.String.edit_student_title) + " " + this.selectedStudent.Name;

            this.nameLayout.EditText.Text = this.NewStudent ? string.Empty : this.selectedStudent.Name;
            this.birthdateLayout.EditText.Text = this.NewStudent ? string.Empty : this.selectedStudent.Birthdate.ToShortDateString();
            this.universityLayout.EditText.Text = this.NewStudent ? string.Empty : this.selectedStudent.University;
            this.groupLayout.EditText.Text = this.NewStudent ? string.Empty : this.selectedStudent.GroupName;
            this.phoneLayout.EditText.Text = this.NewStudent ? string.Empty : this.selectedStudent.Phone;
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
                    this.ConfirmAsync();
                    return true;
                case Resource.Id.action_reset:
                    this.Reset();
                    return true;
                case Android.Resource.Id.Home:
                    this.Activity.OnBackPressed();
                    return true;
                default:
                    return true;
            }
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == 1000 && resultCode == (int)Result.Ok && data != null)
            {
                if (data.Data == null)
                {
                    this.image = (Bitmap)data.GetParcelableExtra("data");
                    this.profilePhoto.SetImageBitmap(this.image);
                }
                else
                {
                    this.image = MediaStore.Images.Media.GetBitmap(this.Activity.ContentResolver, data.Data);
                }
            }
        }

        public override void OnStart()
        {
            base.OnStart();

            this.saveButton.Click += this.SaveButtonClickAsync;
            this.birthdateLayout.EditText.Touch += this.OnBirthdateEditTextTouch;
            this.birthdateLayout.EditText.FocusChange += this.OnBirthdateEditTextFocus;
            this.profilePhoto.Touch += this.OnProfilePhotoTouch;

            this.DisplayHomeUp(true);
        }

        public override void OnStop()
        {
            base.OnStop();

            this.saveButton.Click -= this.SaveButtonClickAsync;
            this.birthdateLayout.Touch -= this.OnBirthdateEditTextTouch;
            this.birthdateLayout.EditText.FocusChange -= this.OnBirthdateEditTextFocus;
            this.profilePhoto.Touch -= this.OnProfilePhotoTouch;

            this.DisplayHomeUp(false);
        }

        private void OnProfilePhotoTouch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                var chooserIntent = Common.Intents.CommonIntents.CreateImageChooserIntent(this.Activity);
                this.StartActivityForResult(chooserIntent, 1000);
            }
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

        private async void SaveButtonClickAsync(object sender, EventArgs e)
        {
            await this.ConfirmAsync();
        }

        private void DataSetPickerDialog(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            this.birthdateLayout.EditText.Text = e.Date.ToShortDateString();
        }


        private async Task ConfirmAsync()
        {
            var name = this.nameLayout.EditText.Text.TrimEnd();
            var birthdate = this.birthdateLayout.EditText.Text.TrimEnd();
            var uni = this.universityLayout.EditText.Text.TrimEnd();
            var group = this.groupLayout.EditText.Text.TrimEnd();
            var phone = this.phoneLayout.EditText.Text.TrimEnd().Length == 0 ? null
                : this.phoneLayout.EditText.Text.TrimEnd();

            if (this.NewStudent)
            {
                await this.AddStudent(name, birthdate, uni, group, phone);
            }
            else
            {
                await this.ChangeStudentById(this.StudentId, this.profilePhotoPath, name, birthdate, group, uni, phone);
            }
        }

        private async Task AddStudent(string name, string birthdate, string uni, string group, string phone)
        {
            var profilePhotoUri = await PhotoService.SavePhotoAsync(
               image, string.Format(
                   CultureInfo.InvariantCulture, "{0}{1}_profile_photo.png", name.ToLowerInvariant(), GetHashCode()), Activity);

            var validationResult = await this.studentRepository.AddNewStudentAsync(name, profilePhotoUri, birthdate, group, uni, phone);

            if (!validationResult.IsValid)
            {
                this.SetErrors(validationResult);
            }
            else
            {
                this.ShowStudentList();
            }
        }


        private async Task ChangeStudentById(string studentId, Uri profilePhotoPath, string name, string birthdate, string group, string uni, string phone)
        {
            var profilePhotoUri = await PhotoService.SavePhotoAsync(
                image, string.Format(
                    CultureInfo.InvariantCulture, "{0}{1}_profile_photo.png", name.ToLowerInvariant(), GetHashCode()), Activity);

            var validationResult = await this.studentRepository.ChangeStudentById(
                this.StudentId, profilePhotoUri, name, birthdate, group, uni, phone);

            if (!validationResult.IsValid)
            {
                this.SetErrors(validationResult);
            }
            else
            {
                this.ShowStudentList();
            }
        }

        private void SetErrors(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                foreach (var ctr in this.layouts)
                {
                    if (error.Key == ctr.Key)
                    {
                        foreach (var message in error.Value)
                        {
                            ctr.Value.Error = message;
                        }
                    }
                }
            }
        }

        private void ShowStudentList()
        {
            var studentList = new StudentListFragment(null);
            this.FragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.main_container, studentList)
                .Commit();
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
    }
}
