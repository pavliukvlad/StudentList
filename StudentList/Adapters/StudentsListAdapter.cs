using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StudentList.Fragments
{
    class StudentsListAdapter : BaseAdapter<Student>
    {
        Student[] students;
        Activity activity;

        public StudentsListAdapter(Activity activity, Student[] students)
        {
            this.activity = activity;
            this.students = students;
        }

        public override Student this[int position] => students[position];

        public override int Count => students.Count();

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = activity.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = string.Format("Name: {0} Group name: {1}", students[position].Name, students[position].GroupName);
            return view;
        }
    }
}