using System.Collections.Generic;
using System.Globalization;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using StudentList.Adapters;
using StudentList.Model;
using StudentList.Providers.Interfaces;

namespace StudentList.Fragments
{
    public class StudentListFragment : Android.Support.V4.App.Fragment
    {
        private readonly StudentFilter studentFilter;
        private readonly StudentAdapter studentAdapter;

        private RecyclerView recyclerView;
        private IStudentRepository repository;

        private TextView filteringResultTextView;
        private ProgressBar loadingProgressBar;
        private TextView studentsCountTextView;

        public StudentListFragment(StudentFilter studentFilter)
        {
            this.studentFilter = studentFilter;
            this.studentAdapter = new StudentAdapter();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.repository = new StudentsRepository();

            this.HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.student_list_fragment, container, false);

            this.recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            this.filteringResultTextView = view.FindViewById<TextView>(Resource.Id.filter_result_textview);
            this.loadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_progress_bar);
            this.studentsCountTextView = view.FindViewById<TextView>(Resource.Id.students_count_textview);

            ((AppCompatActivity)this.Activity).SupportActionBar.Title = this.GetString(Resource.String.app_name);

            return view;
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            if (!this.studentAdapter.IsAnyStudents)
            {
                var students = await this.repository.GetStudentsAsync(this.studentFilter);
                this.studentAdapter.SetItems(students);

                this.filteringResultTextView.Visibility = students.Count > 0 ? ViewStates.Invisible
                : ViewStates.Visible;
                this.studentsCountTextView.Text = string.Format(
                CultureInfo.InvariantCulture, this.GetString(Resource.String.student_count_pattern), students.Count);
            }

            this.loadingProgressBar.Visibility = ViewStates.Invisible;

            var layoutManager = new LinearLayoutManager(this.Activity);
            this.recyclerView.SetLayoutManager(layoutManager);
            this.recyclerView.SetAdapter(this.studentAdapter);
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
                    this.ShowStudentInfo(string.Empty);
                    return true;
                case Resource.Id.menu_search:
                    this.FilterStudents();
                    return true;
                default:
                    return false;
            }
        }

        private void StudentAdapterItemClick(object sender, Student e)
        {
            this.ShowStudentInfo(e.Id);
        }

        private async void Reset()
        {
            this.loadingProgressBar.Visibility = ViewStates.Visible;
            this.filteringResultTextView.Visibility = ViewStates.Invisible;

            var students = await this.repository.GetStudentsAsync(StudentFilter.Default);
            this.studentsCountTextView.Text = string.Format(
                CultureInfo.InvariantCulture, this.GetString(Resource.String.student_count_pattern), students.Count);

            this.loadingProgressBar.Visibility = ViewStates.Invisible;
            this.studentAdapter.SetItems(students);
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

        private void ShowStudentInfo(string studentId)
        {
            var studentDetails = StudentProfileFragment.NewInstance(studentId);
            this.FragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.main_container, studentDetails)
                .AddToBackStack(null)
                .Commit();
        }
    }
}
