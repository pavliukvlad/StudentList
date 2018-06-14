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
using StudentList.Providers.Interfaces;

namespace StudentList.Adapters
{
    class StudentAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;

        private IStudentRepository studentsProvider;
        private Context parentContext;

        public StudentAdapter(IStudentRepository studentsProvider)
        {
            this.studentsProvider = studentsProvider;
        }

        public override int ItemCount => studentsProvider.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            StudentViewHolder vh = holder as StudentViewHolder;
            vh.Info.Text = string.Format(parentContext.GetString(Resource.String.student_info_pattern),
                studentsProvider[position].Name, 
                studentsProvider[position].Age,
                studentsProvider[position].University,
                studentsProvider[position].GroupName);
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