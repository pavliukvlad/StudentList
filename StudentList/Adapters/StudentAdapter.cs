using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using StudentList.Activities;
using StudentList.Model;

namespace StudentList.Adapters
{
    public class StudentAdapter : RecyclerView.Adapter
    {
        public event EventHandler<string> ItemClick;

        private IList<Student> students;

        private Context parentContext;
        private readonly RecyclerView recyclerView;

        public override int ItemCount => this.students.Count;

        public StudentAdapter(RecyclerView recyclerView)
        {
            this.recyclerView = recyclerView;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            StudentViewHolder vh = holder as StudentViewHolder;

            var cultureSettings = new CultureInfo("en-Us");
            vh.Info.Text = string.Format(
                cultureSettings,
                this.parentContext.GetString(Resource.String.student_info_pattern),
               this.students[position].Name,
                this.students[position].Birthdate.ToShortDateString(),
                this.students[position].University,
                this.students[position].GroupName);

            vh.Id = this.students[position].Id;
            vh.SetPhoneIconVisible(this.students[position].Phone);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            this.parentContext = parent.Context;
            var itemView = LayoutInflater.From(this.parentContext).Inflate(Resource.Layout.student_cart, parent, false);
            var viewHolder = new StudentViewHolder(itemView, this.OnClick);
            return viewHolder;
        }

        public void SetItems(IList<Student> items)
        {
            this.students = items;
            this.NotifyDataSetChanged();
        }

        private void OnClick(string id)
        {
            this.ItemClick?.Invoke(this, id);
        }
    }
}
