using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Support.V7.Widget;
using Android.Views;
using StudentList.Activities;
using StudentList.Constants;
using StudentList.Models;
using StudentList.Providers;
using StudentList.Providers.Interfaces;

namespace StudentList.Adapters
{
    public class StudentAdapter : RecyclerView.Adapter
    {
        private IList<Student> students;
        private IUserPhotoProvider photoProvider;

        public StudentAdapter()
        {
            this.photoProvider = new PhotoProvider();
            this.students = new List<Student>();
        }

        public event EventHandler<Student> ItemClick;

        public override int ItemCount => this.students.Count;

        public bool IsAnyStudents => this.students != null;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            StudentViewHolder vh = holder as StudentViewHolder;

            vh.Info.Text = string.Format(
                CultureInfo.InvariantCulture,
                vh.ItemView.Context.GetString(Resource.String.student_info_pattern),
                this.students[position].Name,
                this.students[position].Birthdate.ToString(FormatConstants.DateTimeFormat, CultureInfo.InvariantCulture),
                this.students[position].University,
                this.students[position].GroupName);

            vh.PhoneImage.Visibility = this.students[position].Phone == null ? ViewStates.Invisible : ViewStates.Visible;

            var image = this.photoProvider.GetUserPhoto(this.students[position]);

            if (image != null)
            {
                vh.ProfilePhotoImage.SetImageBitmap(image);
            }
            else
            {
                vh.ProfilePhotoImage.SetImageResource(Resource.Drawable.person_photo);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.student_card, parent, false);
            var viewHolder = new StudentViewHolder(itemView, this.OnItemClick);

            return viewHolder;
        }

        public void SetItems(IList<Student> items)
        {
            this.students = items;
            this.NotifyDataSetChanged();
        }

        private void OnItemClick(int position)
        {
            this.ItemClick?.Invoke(this, this.students[position]);
        }
    }
}
