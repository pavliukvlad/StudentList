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
using StudentList.Model;
using StudentList.Providers.Interfaces;

namespace StudentList.Adapters
{
    class StudentAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;

        private IList<Student> students;
        private Context parentContext;

        public StudentAdapter(IList<Student> students)
        {
            this.students = students;
        }
        
        public override int ItemCount => students.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            StudentViewHolder vh = holder as StudentViewHolder;

            vh.Info.Text = string.Format(parentContext.GetString(Resource.String.student_info_pattern),
                students[position].Name, 
                students[position].Birthdate.ToShortDateString(),
                students[position].University,
                students[position].GroupName);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            parentContext = parent.Context;
            View itemView = LayoutInflater.From(parentContext).Inflate(Resource.Layout.student_list, parent, false);
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