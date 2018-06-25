using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace StudentList.Activities
{
    public class StudentViewHolder : RecyclerView.ViewHolder
    {
        public TextView Info { get; set; }
        public Button EditButton { get; set; }

        public string Id { get; set; }

        public StudentViewHolder(View itemView, Action<string> listener) : base(itemView)
        {
            Info = itemView.FindViewById<TextView>(Resource.Id.textView);
            EditButton = itemView.FindViewById<Button>(Resource.Id.edit_student_btn);

            EditButton.Click += (sender, e) => listener(Id);
        }
    }
}
