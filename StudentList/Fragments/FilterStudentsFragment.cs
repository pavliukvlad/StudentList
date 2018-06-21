using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
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
        private Button selectBirthdateButton;
        private TextView showDateTextView;
        private Button confirmButton;

        private DateTime birthdate;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.filter_students, container, false);

            nameEditText = view.FindViewById<EditText>(Resource.Id.name_edittext);
            groupSpinner = view.FindViewById<Spinner>(Resource.Id.group_spinner);
            selectBirthdateButton = view.FindViewById<Button>(Resource.Id.select_birthdate_btn);
            showDateTextView = view.FindViewById<TextView>(Resource.Id.show_date_textview);
            confirmButton = view.FindViewById<Button>(Resource.Id.confirm_filter_btn);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var adapter = ArrayAdapter.CreateFromResource(Activity, Resource.Array.group_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            groupSpinner.Adapter = adapter;

            showDateTextView.Text = view.Context.GetString(Resource.String.show_date_textview);
        }

        public override void OnStart()
        {
            base.OnStart();
            selectBirthdateButton.Click += DateOfBirthEditTextClick;
            confirmButton.Click += ConfirmButtonClick;
        }

        public override void OnStop()
        {
            base.OnStop();
            selectBirthdateButton.Click -= DateOfBirthEditTextClick;
            confirmButton.Click -= ConfirmButtonClick;
        }

        private void DateOfBirthEditTextClick(object sender, EventArgs e)
        {
            var datePickerDialog = new DatePickerDialog(Context, DateOfBirthDatePickerDialogDateSet, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
            datePickerDialog.Show();
        }

        private void DateOfBirthDatePickerDialogDateSet(object sender, DatePickerDialog.DateSetEventArgs args)
        {
            birthdate = args.Date;
            showDateTextView.Text = args.Date.ToShortDateString();
        }

        private void ConfirmButtonClick(object sender, EventArgs e)
        {
            StudentFilter studentFilter = new StudentFilter() { Name = nameEditText.Text, Group = groupSpinner.SelectedItem.ToString(), Birthdate = birthdate };
            FragmentManager.BeginTransaction().Replace(Resource.Id.main_container, new StudentListFragment(studentFilter)).Commit();
        }
    }
}