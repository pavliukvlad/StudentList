using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace StudentList.Activities
{
    public class StudentViewHolder : RecyclerView.ViewHolder
    {
        public StudentViewHolder(View itemView, Action<int> listener)
            : base(itemView)
        {
            this.Info = itemView.FindViewById<TextView>(Resource.Id.textView);
            this.PhoneImage = itemView.FindViewById<ImageView>(Resource.Id.phone_image);
            this.ProfilePhotoImage = itemView.FindViewById<ImageView>(Resource.Id.profile_photo);

            itemView.Click += (sender, e) => { listener(this.AdapterPosition); };
        }

        public TextView Info { get; set; }

        public ImageView PhoneImage { get; set; }

        public ImageView ProfilePhotoImage { get; set; }
    }
}
