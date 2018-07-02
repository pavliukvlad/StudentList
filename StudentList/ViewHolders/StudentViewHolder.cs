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
        public StudentViewHolder(View itemView, Action<string> listener)
            : base(itemView)
        {
            this.Info = itemView.FindViewById<TextView>(Resource.Id.textView);
            this.PhoneImage = itemView.FindViewById<ImageView>(Resource.Id.phone_image);

            itemView.Click += (sender, e) => { listener(this.Id); };
        }

        public void SetPhoneIconVisible(string phone)
        {
            this.PhoneImage.Visibility = phone == null ? ViewStates.Invisible : ViewStates.Visible;
        }

        public TextView Info { get; set; }

        public ImageView PhoneImage { get; set; }

        public string Id { get; set; }

        public string Phone { get; set; }
    }
}
