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
using StudentList.Activities;

namespace StudentList.Adapters
{
    class StudentAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;

        StudentsProvider studentsProvider;

        public StudentAdapter(StudentsProvider studentsProvider)
        {
            this.studentsProvider = studentsProvider;
        }

        public override int ItemCount => studentsProvider.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            StudentViewHolder vh = holder as StudentViewHolder;
            vh.Info.Text = string.Format("Name: {0} Age: {1} \nUniversity: {2}\nGroup: {3}",
                studentsProvider[position].Name, studentsProvider[position].Age,
                studentsProvider[position].University,
                studentsProvider[position].GroupName);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.student_list, parent, false);
            StudentViewHolder viewHolder = new StudentViewHolder(itemView, OnClick);
            return viewHolder;
        }

        private void OnClick(int position)
        {
            EventHandler<int> handler = ItemClick;

            if (handler != null)
                handler(this, position);
        }
    }
}