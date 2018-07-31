using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Refractored.Controls;
using StudentList.Common.Dialogs;
using StudentList.Common.Intents;
using StudentList.Constants;
using StudentList.Domain;
using StudentList.Domain.Actions;
using StudentList.Domain.States;
using StudentList.Extensions;
using StudentList.Models;
using StudentList.Providers;
using StudentList.Providers.Interfaces;
using StudentList.Services;

namespace StudentList.Fragments
{
    public class StudentProfileFragment : Android.Support.V4.App.Fragment
    {
        private IStudentRepository repository;
        private Dictionary<string, TextInputLayout> layouts;
        private Student selectedStudent;
        private Bitmap profilePhoto;
        private Guid profilePhotoGuid;
        private IStore<ApplicationState> store;

        private Button confirmButton;
        private LoadingDialog loadingDialog;
        private CircleImageView profilePhotoImageView;
        private TextInputLayout nameLayout;
        private TextInputLayout birthdateLayout;
        private TextInputLayout universityLayout;
        private TextInputLayout groupLayout;
        private TextInputLayout phoneLayout;

        private string StudentId => this.Arguments.GetString(IntentConstants.StudentId, string.Empty);

        public static StudentProfileFragment NewInstance()
        {
            var studentProfileFragment = new StudentProfileFragment();
            return studentProfileFragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.store = MainApplication.Store;
            this.selectedStudent = this.store.GetState().StudentProfileState.SelectedStudent;

            this.repository = new StudentsRepository(
                new LoadingDelays { AddStudentDelay = 300, ChangeStudentDelay = 300, GetStudentDelay = 500, GetStudentsDelay = 1000 },
                new StringProvider(this.Context));

            this.layouts = new Dictionary<string, TextInputLayout>();
            this.loadingDialog = new LoadingDialog(this.Context);

            this.HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.student_profile_fragment, null);

            this.confirmButton = view.FindViewById<Button>(Resource.Id.confirm_btn);
            this.nameLayout = view.FindViewById<TextInputLayout>(Resource.Id.name_layout);
            this.birthdateLayout = view.FindViewById<TextInputLayout>(Resource.Id.birthdate_layout);
            this.universityLayout = view.FindViewById<TextInputLayout>(Resource.Id.uni_layout);
            this.groupLayout = view.FindViewById<TextInputLayout>(Resource.Id.group_layout);
            this.phoneLayout = view.FindViewById<TextInputLayout>(Resource.Id.phone_layout);
            this.profilePhotoImageView = view.FindViewById<CircleImageView>(Resource.Id.profile_image);

            this.layouts.Add(StudentProfileKeys.Name, this.nameLayout);
            this.layouts.Add(StudentProfileKeys.Birthdate, this.birthdateLayout);
            this.layouts.Add(StudentProfileKeys.Group, this.groupLayout);
            this.layouts.Add(StudentProfileKeys.University, this.universityLayout);
            this.layouts.Add(StudentProfileKeys.Phone, this.phoneLayout);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            ((AppCompatActivity)this.Activity).SupportActionBar.Title = this.selectedStudent == null ? this.GetString(Resource.String.create_student_title)
               : string.Format(CultureInfo.InvariantCulture, PatternConstants.AppBarTitle, this.GetString(Resource.String.edit_student_title), this.selectedStudent.Name);

            if (this.selectedStudent != null && this.selectedStudent.ProfilePhoto != null)
            {
                this.profilePhotoImageView.SetImageBitmap(BitmapFactory
                    .DecodeFile(this.selectedStudent.ProfilePhoto.AbsolutePath));
            }

            this.nameLayout.EditText.Text = this.selectedStudent == null ? string.Empty : this.selectedStudent.Name;
            this.birthdateLayout.EditText.Text = this.selectedStudent == null ? string.Empty
                : this.selectedStudent.Birthdate.ToString(FormatConstants.DateTimeFormat, CultureInfo.InvariantCulture);
            this.universityLayout.EditText.Text = this.selectedStudent == null ? string.Empty : this.selectedStudent.University;
            this.groupLayout.EditText.Text = this.selectedStudent == null ? string.Empty : this.selectedStudent.GroupName;
            this.phoneLayout.EditText.Text = this.selectedStudent == null ? string.Empty : this.selectedStudent.Phone;
            this.confirmButton.Text = this.selectedStudent == null ? this.GetString(Resource.String.add_new_student_btn)
                : this.GetString(Resource.String.save_changes_btn);
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
                    this.Activity.RunMethodWithLoaderAsync(this.ConfirmAsync());
                    return true;
                case Resource.Id.action_reset:
                    this.Reset();
                    return true;
                case Android.Resource.Id.Home:
                    this.Activity.OnBackPressed();
                    return true;
                default:
                    return false;
            }
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == 1000 && resultCode == (int)Result.Ok && data != null)
            {
                if (data.Data == null)
                {
                    this.profilePhoto = (Bitmap)data.GetParcelableExtra("data");
                }
                else
                {
                    this.profilePhoto = MediaStore.Images.Media.GetBitmap(this.Activity.ContentResolver, data.Data);
                }

                this.profilePhotoGuid = Guid.NewGuid();
                this.profilePhotoImageView.SetImageBitmap(this.profilePhoto);
            }
        }

        public override void OnStart()
        {
            base.OnStart();

            this.confirmButton.Click += this.ConfirmButtonClickAsync;
            this.birthdateLayout.EditText.Touch += this.OnBirthdateEditTextTouch;
            this.birthdateLayout.EditText.FocusChange += this.OnBirthdateEditTextFocus;
            this.profilePhotoImageView.Touch += this.OnProfilePhotoTouch;

            this.DisplayHomeUp(true);
        }

        public override void OnStop()
        {
            base.OnStop();

            this.confirmButton.Click -= this.ConfirmButtonClickAsync;
            this.birthdateLayout.Touch -= this.OnBirthdateEditTextTouch;
            this.birthdateLayout.EditText.FocusChange -= this.OnBirthdateEditTextFocus;
            this.profilePhotoImageView.Touch -= this.OnProfilePhotoTouch;

            this.DisplayHomeUp(false);
        }

        private async void OnProfilePhotoTouch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                var requestedCameraPermission = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                var chooserIntent = await PhotoIntent.CreateImageChooserIntentAsync(this.Activity);
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

        private async void ConfirmButtonClickAsync(object sender, EventArgs e)
        {
            await this.Activity.RunMethodWithLoaderAsync(this.ConfirmAsync());
        }

        private void DataSetPickerDialog(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            this.birthdateLayout.EditText.Text = e.Date.ToString(FormatConstants.DateTimeFormat, CultureInfo.InvariantCulture);
        }

        private async Task ConfirmAsync()
        {
            var name = this.nameLayout.EditText.Text.Trim();
            var uni = this.universityLayout.EditText.Text.Trim();
            var group = this.groupLayout.EditText.Text.Trim();
            var phone = string.IsNullOrWhiteSpace(this.phoneLayout.EditText.Text) ? null
                : this.phoneLayout.EditText.Text.Trim();

            if (!DateTime.TryParse(this.birthdateLayout.EditText.Text, out DateTime birthdate))
            {
                birthdate = DateTime.MinValue;
            }

            var savingPhotoResult = await PhotoService.SavePhotoAsync(
               this.profilePhoto,
               string.Format(CultureInfo.InvariantCulture, PatternConstants.PhotoName, this.profilePhotoGuid.ToString()),
               this.Activity);

            if (!savingPhotoResult.IsError || savingPhotoResult == null)
            {
                if (this.selectedStudent == null)
                {
                    await this.AddStudent(savingPhotoResult.ProfilePhotoUri, name, birthdate, uni, group, phone);
                }
                else
                {
                    await this.ChangeStudentById(this.selectedStudent.Id, savingPhotoResult.ProfilePhotoUri, name, birthdate, group, uni, phone);
                }
            }
            else
            {
                Toast.MakeText(this.Activity, Resource.String.photo_toast_error, ToastLength.Short).Show();
            }
        }

        private async Task AddStudent(Uri photoUri, string name, DateTime birthdate, string uni, string group, string phone)
        {
            var validationResult = await this.repository.AddNewStudentAsync(name, photoUri, birthdate, group, uni, phone);

            if (validationResult.IsValid)
            {
                this.ShowStudentList();
            }
            else
            {
                this.SetErrors(validationResult);
            }
        }

        private async Task ChangeStudentById(string studentId, Uri photoUri, string name, DateTime birthdate, string group, string uni, string phone)
        {
            var validationResult = await this.repository.ChangeStudentById(
                studentId, photoUri, name, birthdate, group, uni, phone);

            if (validationResult.IsValid)
            {
                this.ShowStudentList();
            }
            else
            {
                this.SetErrors(validationResult);
            }
        }

        private void SetErrors(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                foreach (var ctr in this.layouts)
                {
                    ctr.Value.Error = validationResult.GetErrorMessages(ctr.Key);
                }
            }
        }

        private void ShowStudentList()
        {
            this.store.Dispatch(new StudentListUpdated());
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
            this.profilePhotoImageView.SetImageResource(Resource.Drawable.camera);
            this.nameLayout.EditText.Text = string.Empty;
            this.groupLayout.EditText.Text = string.Empty;
            this.birthdateLayout.EditText.Text = string.Empty;
            this.universityLayout.EditText.Text = string.Empty;
            this.phoneLayout.EditText.Text = string.Empty;
        }
    }
}
