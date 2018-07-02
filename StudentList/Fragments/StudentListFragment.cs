using System;
using System.Collections.Generic;
using System.Globalization;
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
using Android.Support.V7.App;
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

        private bool matchesFound;

        public StudentListFragment(StudentFilter studentFilter)
        {
            this.studentFilter = studentFilter;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.repository = new StudentsRepository();

            this.HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.recycle_holder, container, false);

            this.recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            this.filteringResultTextView = view.FindViewById<TextView>(Resource.Id.filter_result_textview);
            this.loadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_progress_bar);
            this.studentsCountTextView = view.FindViewById<TextView>(Resource.Id.students_count_textview);

            ((AppCompatActivity)this.Activity).SupportActionBar.Title = this.GetString(Resource.String.app_name);

            return view;
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            this.layoutManager = new LinearLayoutManager(this.Activity);
            this.studentAdapter = new StudentAdapter(this.recyclerView);

            if (this.students == null)
            {
                this.students = await this.repository.GetStudentsAsync(this.studentFilter);
            }

            this.loadingProgressBar.Visibility = ViewStates.Invisible;

            this.studentAdapter.SetItems(this.students);

            this.recyclerView.SetLayoutManager(this.layoutManager);
            this.recyclerView.SetAdapter(this.studentAdapter);

            this.matchesFound = this.students.Count > 0 ? true : false;
            this.filteringResultTextView.Visibility = !this.matchesFound ? ViewStates.Visible
                : ViewStates.Invisible;

            this.studentsCountTextView.Text = string.Format(
                CultureInfo.InvariantCulture, this.GetString(
                    Resource.String.student_count_pattern), this.students.Count);
        }

        public override void OnStart()
        {
            base.OnStart();

            this.studentAdapter.ItemClick += this.StudentAdapterItemClick;
        }

        public override void OnStop()
        {
            base.OnStop();

            this.studentAdapter.ItemClick -= this.StudentAdapterItemClick;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.top_menu, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_reset:
                    this.Reset();
                    return true;
                case Resource.Id.menu_add_student:
                    this.ShowStudentInfo(string.Empty, true);
                    return true;
                case Resource.Id.menu_search:
                    this.FilterStudents();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void StudentAdapterItemClick(object sender, string e)
        {
            this.ShowStudentInfo(e);
        }

        private async void Reset()
        {
            this.loadingProgressBar.Visibility = ViewStates.Visible;
            this.filteringResultTextView.Visibility = ViewStates.Invisible;

            this.students = await this.repository.GetStudentsAsync(null);
            this.studentsCountTextView.Text = string.Format(
                CultureInfo.InvariantCulture, this.GetString(Resource.String.student_count_pattern), this.students.Count);

            this.loadingProgressBar.Visibility = ViewStates.Invisible;
            this.studentAdapter.SetItems(this.students);
        }

        private void FilterStudents()
        {
            var filterStudents = new FilterStudentsFragment(this.studentFilter);
            this.FragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.main_container, filterStudents)
                .AddToBackStack(null)
                .Commit();
        }

        private void ShowStudentInfo(string studentId, bool newStudent = false)
        {
            var studentDetails = StudentProfileFragment.NewInstance(studentId, newStudent);
            this.FragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.main_container, studentDetails)
                .AddToBackStack(null)
                .Commit();
        }
    }
}
