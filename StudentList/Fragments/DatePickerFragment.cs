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
    public class DatePickerFragment : Android.Support.V4.App.DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();
        private Action<DateTime> dateSelectedHandler;

        public static DatePickerFragment NewInstance(Action<DateTime> action)
        {
            DatePickerFragment datePicker = new DatePickerFragment();
            datePicker.dateSelectedHandler = action;
            return datePicker;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            DatePickerDialog dateDialog = new DatePickerDialog(Activity, this, currently.Year, currently.Month, currently.Day);
            return dateDialog;
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            DateTime selectedDate = new DateTime(year, month + 1, dayOfMonth);
            dateSelectedHandler(selectedDate);
        }
    }
}