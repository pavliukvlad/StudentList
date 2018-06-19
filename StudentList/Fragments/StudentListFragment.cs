using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
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
        private StudentFilter studentFilter;

        private Button addNewStudentButton;
        private Button filterStudentsButton;
        private TextView filteringResultTextView;
        private ProgressBar loadingProgressBar;
        private Button resetButton;

        private bool matchesFound;

        public StudentListFragment() { }

        public StudentListFragment(StudentFilter studentFilter)
        {
            this.studentFilter = studentFilter;
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            layoutManager = new LinearLayoutManager(Activity);
            studentAdapter = new StudentAdapter(recyclerView);

            students = await GetStudentsAsync(studentFilter);
            loadingProgressBar.Visibility = ViewStates.Invisible;

            studentAdapter.SetItems(students);
         
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(studentAdapter);

            matchesFound = students.Count > 0 ? true : false;
            filteringResultTextView.Visibility = !matchesFound ? ViewStates.Visible : ViewStates.Invisible;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.recycle_holder, container, false);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            addNewStudentButton = view.FindViewById<Button>(Resource.Id.add_new_student_btn);
            filterStudentsButton = view.FindViewById<Button>(Resource.Id.filter_students_btn);
            filteringResultTextView = view.FindViewById<TextView>(Resource.Id.filter_result_textview);
            loadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_progress_bar);
            resetButton = view.FindViewById<Button>(Resource.Id.reset_btn);
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            addNewStudentButton.Click += AddNewStudentButtonClick;
            studentAdapter.ItemClick += StudentAdapterItemClick;
            filterStudentsButton.Click += FilterStudentsButton_Click;
            resetButton.Click += ResetButtonClick;
        }

        public override void OnStop()
        {
            base.OnStop();
            addNewStudentButton.Click -= AddNewStudentButtonClick;
            studentAdapter.ItemClick -= StudentAdapterItemClick;
        }

        private async void ResetButtonClick(object sender, EventArgs e)
        {
            studentFilter = null;
            loadingProgressBar.Visibility = ViewStates.Visible;
            students = await GetStudentsAsync(studentFilter);
            loadingProgressBar.Visibility = ViewStates.Invisible;
            studentAdapter.SetItems(students);
        }

        private void FilterStudentsButton_Click(object sender, EventArgs e)
        {
            FilterStudentsFragment filterStudents = new FilterStudentsFragment();
            FragmentManager.BeginTransaction().Replace(Resource.Id.main_container, filterStudents).AddToBackStack(null).Commit();
        }      

        private void StudentAdapterItemClick(object sender, int e)
        {
            ShowStudentInfo(e);
        }

        private void AddNewStudentButtonClick(object sender, EventArgs e)
        {
            ShowStudentInfo(0, true);
        }

        private async Task<IList<Student>> GetStudentsAsync(StudentFilter studentFilter)
        {
            var repository = new StudentsRepository();

            if (studentFilter == null)
                return await repository.GetStudentsAsync();
            else
                return await repository.GetStudentsAsync(studentFilter.Name, studentFilter.Group, studentFilter.Birthdate);
        }

        private void ShowStudentInfo(int studentId, bool newStudent = false)
        {
            var studentDetails = StudentProfileFragment.NewInstance(studentId, newStudent);
            FragmentManager.BeginTransaction().Replace(Resource.Id.main_container, studentDetails).AddToBackStack(null).Commit();
        }
    }
}