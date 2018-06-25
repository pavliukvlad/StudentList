using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using StudentList.Model;

namespace StudentList.Fragments
{
    public class FilterStudentsFragment : Android.Support.V4.App.Fragment
    {
        private EditText nameEditText;
        private Spinner groupSpinner;
        private TextView chooseBirthdate;
        private FloatingActionButton confirmButton;

        private DateTime birthdate;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.filter_students, container, false);

            nameEditText = view.FindViewById<EditText>(Resource.Id.name_edittext);
            groupSpinner = view.FindViewById<Spinner>(Resource.Id.group_spinner);
            chooseBirthdate = view.FindViewById<TextInputEditText>(Resource.Id.choose_birthdate);
            confirmButton = view.FindViewById<FloatingActionButton>(Resource.Id.confirm_fab);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var adapter = ArrayAdapter.CreateFromResource(Activity, Resource.Array.group_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            groupSpinner.Adapter = adapter;
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
            chooseBirthdate.Touch += ChooseBirthdateClick;
            confirmButton.Click += ConfirmButtonClick;
            DisplayHomeUp(true);
        }

        public override void OnStop()
        {
            base.OnStop();
            DisplayHomeUp(false);
            confirmButton.Click -= ConfirmButtonClick;
        }

        private void DateOfBirthDatePickerDialogDateSet(object sender, DatePickerDialog.DateSetEventArgs args)
        {
            birthdate = args.Date;
            chooseBirthdate.Text = args.Date.ToShortDateString();
        }

        private void ConfirmButtonClick(object sender, EventArgs e)
        {
            Confirm();
        }

        private void ChooseBirthdateClick(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                var datePickerDialog = new DatePickerDialog(Context, DateOfBirthDatePickerDialogDateSet, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
                datePickerDialog.Show();
            }
        }

        private void Confirm()
        {
            StudentFilter studentFilter = new StudentFilter() { Name = nameEditText.Text, Group = groupSpinner.SelectedItem.ToString(), Birthdate = birthdate };
            FragmentManager.BeginTransaction().Replace(Resource.Id.main_container, new StudentListFragment(studentFilter)).Commit();
        }

        private void DisplayHomeUp(bool trigger)
        {
            if (trigger)
            {
                bool canback = Activity.SupportFragmentManager.BackStackEntryCount > 0;
                ((AppCompatActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(canback);
            }
            else
            {
                ((AppCompatActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(trigger);
            }
        }

        private void Reset()
        {
            nameEditText.Text = string.Empty;
            chooseBirthdate.Text = string.Empty;
        }
    }
}
