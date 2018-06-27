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
        public ImageView phoneImage { get; set; }

        public string Id { get; set; }
        public string Phone { get; set; }

        public StudentViewHolder(View itemView, Action<string> listener)
            : base(itemView)
        {
            Info = itemView.FindViewById<TextView>(Resource.Id.textView);
            phoneImage = itemView.FindViewById<ImageView>(Resource.Id.phone_image);

            itemView.Click += (sender, e) => { listener(Id); };
        }

        public void SetPhoneIconVisible(string phone)
        {
            phoneImage.Visibility = phone == null ? ViewStates.Invisible : ViewStates.Visible;
        }
    }
}
