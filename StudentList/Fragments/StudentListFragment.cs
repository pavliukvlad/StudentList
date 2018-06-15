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
using StudentList.Model;
using StudentList.Providers.Interfaces;

namespace StudentList.Fragments
{
    public class StudentListFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView recyclerView;
        private RecyclerView.LayoutManager layoutManager;
        private StudentAdapter studentAdapter;
   
        private IList<Student> students;

        private Button addNewStudentButton;
        private Button filterStudentsButton;

        public StudentListFragment()
        {
            students = new StudentsRepository().Students;
        }
        public StudentListFragment(IList<Student> students)
        {
            this.students = students;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            addNewStudentButton = view.FindViewById<Button>(Resource.Id.add_new_student_btn);
            filterStudentsButton = view.FindViewById<Button>(Resource.Id.filter_students_btn);

            layoutManager = new LinearLayoutManager(Activity);
            studentAdapter = new StudentAdapter(students);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(studentAdapter);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.recycle_holder, container, false);
        }

        public override void OnStart()
        {
            base.OnStart();
            addNewStudentButton.Click += AddNewStudentButton_Click;
            studentAdapter.ItemClick += StudentAdapter_ItemClick;
            filterStudentsButton.Click += FilterStudentsButton_Click;
        }

        private void FilterStudentsButton_Click(object sender, EventArgs e)
        {
            FilterStudentsFragment filterStudents = new FilterStudentsFragment();
            FragmentManager.BeginTransaction().Replace(Resource.Id.main_container, filterStudents).AddToBackStack(null).Commit();
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
            FragmentManager.BeginTransaction().Replace(Resource.Id.main_container, studentDetails).AddToBackStack(null).Commit();
        }
    }
}