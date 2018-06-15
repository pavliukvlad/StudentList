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

namespace StudentList.Fragments
{
    public class FilterStudentsFragment : Android.Support.V4.App.Fragment
    {
        private EditText nameEditText;
        private Spinner groupSpinner;
        private Button selectBirthdateButton;
        private TextView selectedDateTextView;
        private Button confirmButton;

        private DateTime birthdate;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.filter_students, container, false);

            nameEditText = view.FindViewById<EditText>(Resource.Id.name_edittext);
            groupSpinner = view.FindViewById<Spinner>(Resource.Id.group_spinner);
            selectBirthdateButton = view.FindViewById<Button>(Resource.Id.select_birthdate_btn);
            selectedDateTextView = view.FindViewById<TextView>(Resource.Id.selected_date_textview);
            confirmButton = view.FindViewById<Button>(Resource.Id.confirm_filter_btn);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var adapter = ArrayAdapter.CreateFromResource(Activity, Resource.Array.group_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            groupSpinner.Adapter = adapter;

            selectedDateTextView.Text = DateTime.Now.ToShortDateString();
        }

        public override void OnStart()
        {
            base.OnStart();
            selectBirthdateButton.Click += ChooseBirthdayButton_Click;
            confirmButton.Click += ConfirmButton_Click;
        }

        public override void OnStop()
        {
            base.OnStop();
            selectBirthdateButton.Click -= ChooseBirthdayButton_Click;
            confirmButton.Click -= ConfirmButton_Click;
        }

        private void ChooseBirthdayButton_Click(object sender, EventArgs e)
        {
            DatePickerFragment datePickerFrag = DatePickerFragment.NewInstance( date => 
            {
                selectedDateTextView.Text = date.ToShortDateString();
                birthdate = date;
            });
            datePickerFrag.Show(FragmentManager, datePickerFrag.Tag);
        }
        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            string name = nameEditText.Text;
            string group = groupSpinner.SelectedItem.ToString();

            var filteringStudents = new StudentsRepository().GetFilteringStudents(name, group, birthdate);

            FragmentManager.BeginTransaction().Replace(Resource.Id.main_container, new StudentListFragment(filteringStudents)).Commit();
        }
    }
}