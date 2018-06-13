using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using StudentList.Adapters;

namespace StudentList.Fragments
{
    public class StudentListFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView recyclerView;
        private RecyclerView.LayoutManager layoutManager;
        private StudentAdapter studentAdapter;
        private StudentsProvider provider;
        private Button addNewStudentButton;

        public static StudentListFragment NewInstance()
        {
            return new StudentListFragment();
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            addNewStudentButton = view.FindViewById<Button>(Resource.Id.add_new_student_btn);

            layoutManager = new LinearLayoutManager(Activity);
            provider = StudentsProvider.NewInstance();
            studentAdapter = new StudentAdapter(provider);

            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(studentAdapter);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.recycle_holder, container, false);
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            addNewStudentButton.Click += AddNewStudentButton_Click;
            studentAdapter.ItemClick += StudentAdapter_ItemClick;
        }
        public override void OnStop()
        {
            base.OnStop();
            addNewStudentButton.Click -= AddNewStudentButton_Click;
            studentAdapter.ItemClick -= StudentAdapter_ItemClick;
        }

        private void StudentAdapter_ItemClick(object sender, int e)
        {
            ShowStudentInfo(e);
        }
        private void AddNewStudentButton_Click(object sender, EventArgs e)
        {
            ShowStudentInfo(0, true);
        }

        private void ShowStudentInfo(int studentId, bool newStudent = false)
        {
            var studentDetails = StudentProfileFragment.NewInstance(studentId, newStudent);
            FragmentManager.BeginTransaction().Replace(Resource.Id.main_container, studentDetails).Commit();

        }
    }
}