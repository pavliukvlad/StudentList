using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using StudentList.Activities;
using StudentList.Model;

namespace StudentList.Adapters
{
    public class StudentAdapter : RecyclerView.Adapter
    {
        private IList<Student> students;

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
                this.students[position].Birthdate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                this.students[position].University,
                this.students[position].GroupName);

            vh.PhoneImage.Visibility = this.students[position].Phone == null ? ViewStates.Invisible : ViewStates.Visible;

            if (this.students[position].ProfilePhoto != null)
            {
                var profilePhoto = BitmapFactory.DecodeFile(this.students[position].ProfilePhoto.AbsolutePath);
                vh.ProfilePhotoImage.SetImageBitmap(profilePhoto);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.student_cart, parent, false);
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
