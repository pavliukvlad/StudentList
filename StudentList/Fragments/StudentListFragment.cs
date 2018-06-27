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
        private IStudentRepository repository;

        private TextView filteringResultTextView;
        private ProgressBar loadingProgressBar;
        private TextView studentsCountTextView;
        private ImageView phoneImageView;

        private int studentsCount;
        private bool matchesFound;

        public StudentListFragment() { }

        public StudentListFragment(StudentFilter studentFilter)
        {
            this.studentFilter = studentFilter;
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            this.layoutManager = new LinearLayoutManager(Activity);
            this.studentAdapter = new StudentAdapter(recyclerView);
            this.repository = new StudentsRepository();

            this.students = await this.repository.GetStudentsAsync(studentFilter);
            this.studentFilter = null;

            this.loadingProgressBar.Visibility = ViewStates.Invisible;

            this.studentAdapter.SetItems(this.students);

            this.recyclerView.SetLayoutManager(this.layoutManager);
            this.recyclerView.SetAdapter(this.studentAdapter);

            this.matchesFound = students.Count > 0 ? true : false;
            this.filteringResultTextView.Visibility = !matchesFound ? ViewStates.Visible : ViewStates.Invisible;
            this.studentsCountTextView.Text = string.Format(GetString(Resource.String.student_count_pattern), students.Count);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.recycle_holder, container, false);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            filteringResultTextView = view.FindViewById<TextView>(Resource.Id.filter_result_textview);
            loadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_progress_bar);
            studentsCountTextView = view.FindViewById<TextView>(Resource.Id.students_count_textview);
            //phoneImageView = view.FindViewById<ImageView>(Resource.Id.phone_image);
            HasOptionsMenu = true;

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_reset:
                    Reset();
                    return true;
                case Resource.Id.menu_add_student:
                    ShowStudentInfo(string.Empty, true);
                    return true;
                case Resource.Id.menu_search:
                    FilterStudents();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnStart()
        {
            base.OnStart();
            studentAdapter.ItemClick += StudentAdapterItemClick;
        }

        public override void OnStop()
        {
            base.OnStop();
            studentAdapter.ItemClick -= StudentAdapterItemClick;
        }

        public async void Reset()
        {
            loadingProgressBar.Visibility = ViewStates.Visible;
            filteringResultTextView.Visibility = ViewStates.Invisible;
            students = await repository.GetStudentsAsync(null);
            studentsCountTextView.Text = string.Format(GetString(Resource.String.student_count_pattern), students.Count);
            loadingProgressBar.Visibility = ViewStates.Invisible;
            studentAdapter.SetItems(students);
        }

        private void FilterStudents()
        {
            FilterStudentsFragment filterStudents = new FilterStudentsFragment();
            FragmentManager.BeginTransaction().Replace(Resource.Id.main_container, filterStudents).AddToBackStack(null).Commit();
        }

        private void ShowStudentInfo(string studentId, bool newStudent = false)
        {
            var studentDetails = StudentProfileFragment.NewInstance(studentId, newStudent);
            FragmentManager.BeginTransaction().Replace(Resource.Id.main_container, studentDetails).AddToBackStack(null).Commit();
        }

        private void StudentAdapterItemClick(object sender, string e)
        {
            ShowStudentInfo(e);
        }
    }
}
