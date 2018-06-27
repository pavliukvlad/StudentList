using System;
using System.Collections.Generic;
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

        public IList<Student> students { get; set; }

        private Context parentContext;
        private RecyclerView recyclerView;

        public override int ItemCount => students.Count;

        public StudentAdapter(RecyclerView recyclerView)
        {
            this.recyclerView = recyclerView;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            StudentViewHolder vh = holder as StudentViewHolder;

            vh.Info.Text = string.Format(parentContext.GetString(Resource.String.student_info_pattern),
               students[position].Name,
                students[position].Birthdate.ToShortDateString(),
                students[position].University,
                students[position].GroupName);
            vh.Id = students[position].Id;
            vh.SetPhoneIconVisible(students[position].Phone);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            parentContext = parent.Context;
            View itemView = LayoutInflater.From(parentContext).Inflate(Resource.Layout.student_cart, parent, false);
            StudentViewHolder viewHolder = new StudentViewHolder(itemView, OnClick);
            return viewHolder;
        }

        public void SetItems(IList<Student> items)
        {
            students = items;
            NotifyDataSetChanged();
        }

        private void OnClick(string id)
        {
            ItemClick?.Invoke(this, id);
        }
    }
}
