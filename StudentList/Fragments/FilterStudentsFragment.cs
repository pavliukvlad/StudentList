using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using StudentList.Domain;
using StudentList.Domain.Actions;
using StudentList.Domain.States;
using StudentList.Models;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace StudentList.Fragments
{
    public class FilterStudentsFragment : Android.Support.V4.App.Fragment
    {
        private readonly StudentFilter studentFilter;
        private ArrayAdapter adapter;
        private IStore<ApplicationState> store;

        private TextInputLayout nameLayout;
        private TextInputLayout birthdateLayout;
        private TextInputLayout groupLayout;
        private FloatingActionButton confirmButton;

        private AlertDialog.Builder groupDialog;
        private DatePickerDialog birthdatePickerDialog;

        private DateTime birthdate;

        public FilterStudentsFragment(StudentFilter studentFilter)
        {
            this.studentFilter = studentFilter;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.store = MainApplication.Store;
            this.adapter = ArrayAdapter.CreateFromResource(
                this.Context, Resource.Array.group_array, Android.Resource.Layout.SimpleListItem1);

            this.HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.filter_students_fragment, container, false);

            this.nameLayout = view.FindViewById<TextInputLayout>(Resource.Id.name_layout);
            this.groupLayout = view.FindViewById<TextInputLayout>(Resource.Id.group_layout);
            this.birthdateLayout = view.FindViewById<TextInputLayout>(Resource.Id.birthdate_layout);
            this.confirmButton = view.FindViewById<FloatingActionButton>(Resource.Id.confirm_fab);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            this.nameLayout.EditText.Text = this.studentFilter.Name;
            this.groupLayout.EditText.Text = this.studentFilter.Group ?? this.GetString(Resource.String.filter_group_txt);
            this.birthdateLayout.EditText.Text = this.studentFilter.Birthdate == default(DateTime) ? string.Empty
                : this.studentFilter.Birthdate.ToShortDateString();

            ((AppCompatActivity)this.Activity).SupportActionBar.Title = this.GetString(Resource.String.filter_title);

            this.groupDialog = new AlertDialog.Builder(this.Context);
            this.groupDialog.SetAdapter(this.adapter, this.OnItemClick);

            this.birthdatePickerDialog = new DatePickerDialog(
                this.Context, this.DateOfBirthDatePickerDialogDateSet, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
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
                default:
                    return false;
            }
        }

        public override void OnStart()
        {
            base.OnStart();

            this.birthdateLayout.EditText.Touch += this.BirthdateEditTextTouch;
            this.birthdateLayout.EditText.FocusChange += this.BirthdateEditTextFocusChange;
            this.confirmButton.Click += this.ConfirmButtonClick;
            this.groupLayout.EditText.Touch += this.GroupEditTextTouch;
            this.groupLayout.EditText.FocusChange += this.GroupEditTextFocusChange;

            this.DisplayHomeUp(true);
        }

        public override void OnStop()
        {
            base.OnStop();

            this.birthdateLayout.EditText.Touch -= this.BirthdateEditTextTouch;
            this.birthdateLayout.EditText.FocusChange -= this.BirthdateEditTextFocusChange;
            this.confirmButton.Click -= this.ConfirmButtonClick;
            this.groupLayout.EditText.Touch -= this.GroupEditTextTouch;
            this.groupLayout.EditText.FocusChange -= this.GroupEditTextFocusChange;

            this.DisplayHomeUp(false);
        }

        private void BirthdateEditTextTouch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                this.birthdatePickerDialog.Show();
            }
        }

        private void BirthdateEditTextFocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                this.birthdatePickerDialog.Show();
            }
        }

        private void GroupEditTextFocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                this.groupDialog.Show();
            }
        }

        private void GroupEditTextTouch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                this.groupDialog.Show();
            }
        }

        private void ConfirmButtonClick(object sender, EventArgs e)
        {
            this.Confirm();
        }

        private void OnItemClick(object sender, DialogClickEventArgs e)
        {
            this.groupLayout.EditText.Text = this.Resources.GetStringArray(Resource.Array.group_array)[e.Which];
        }

        private void DateOfBirthDatePickerDialogDateSet(object sender, DatePickerDialog.DateSetEventArgs args)
        {
            this.birthdate = args.Date;
            this.birthdateLayout.EditText.Text = args.Date.ToShortDateString();
        }

        private void Confirm()
        {
            StudentFilter studentFilter = new StudentFilter(
                this.nameLayout.EditText.Text, this.groupLayout.EditText.Text, this.birthdate);

            this.store.Dispatch(new FiltersApplied() { Filters = studentFilter });
            this.Activity.OnBackPressed();
        }

        private void DisplayHomeUp(bool trigger)
        {
            ((AppCompatActivity)this.Activity).
                SupportActionBar.SetDisplayHomeAsUpEnabled(trigger && this.Activity.SupportFragmentManager.BackStackEntryCount > 0);
        }

        private void Reset()
        {
            this.nameLayout.EditText.Text = string.Empty;
            this.groupLayout.EditText.Text = this.GetString(Resource.String.filter_group_txt);
            this.birthdateLayout.EditText.Text = string.Empty;
        }
    }
}
