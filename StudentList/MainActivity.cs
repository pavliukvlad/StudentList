using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Support.V7.App;

namespace StudentList
{
    [Activity(Label = "@string/app_name", Theme = "@android:style/Theme.Material.Light.DarkActionBar", MainLauncher = true)]
    public class MainActivity : Activity
    {
        RecyclerView recyclerView;
        RecyclerView.LayoutManager layoutManager;
        StudentAdapter studentAdapter;
        StudentsProvider provider;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            layoutManager = new LinearLayoutManager(this);
            provider = new StudentsProvider();
            studentAdapter = new StudentAdapter(provider);

            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(studentAdapter);

            studentAdapter.ItemClick += (sender, e) => { ShowStudentInfo(e); };

            var addNewStudentButton = FindViewById<Button>(Resource.Id.add_new_student_btn);
            addNewStudentButton.Click += AddNewStudentButton_Click;
        }

        private void AddNewStudentButton_Click(object sender, EventArgs e)
        {
            ShowStudentInfo(-1);
        }

        private void ShowStudentInfo(int position)
        {
            var intent = new Intent(this, typeof(StudentProfileActivity));
            intent.PutExtra("student_id", position);
            StartActivity(intent);
        }
    }

    class StudentViewHolder : RecyclerView.ViewHolder
    {
        public TextView Info { get; set; }

        public StudentViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Info = itemView.FindViewById<TextView>(Resource.Id.textView);
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }

    }

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
            vh.Info.Text = string.Format("Name: {0} Age: {1} \nUniversity: {2}\nGroup: {3}",studentsProvider[position].Name, studentsProvider[position].Age, studentsProvider[position].University,
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


