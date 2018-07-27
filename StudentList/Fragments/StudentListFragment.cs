using System.Globalization;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using StudentList.Adapters;
using StudentList.Common.Dialogs;
using StudentList.Extensions;
using StudentList.Models;
using StudentList.Providers;
using StudentList.Providers.Interfaces;

namespace StudentList.Fragments
{
    public class StudentListFragment : Android.Support.V4.App.Fragment
    {
        private readonly StudentFilter studentFilter;

        private RecyclerView recyclerView;
        private StudentAdapter studentAdapter;
        private IStudentRepository repository;

        private TextView filteringResultTextView;
        private TextView studentsCountTextView;

        public StudentListFragment(StudentFilter studentFilter)
        {
            this.studentFilter = studentFilter;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.studentAdapter = new StudentAdapter();
            this.repository = new StudentsRepository(
                new LoadingDelays { AddStudentDelay = 300, ChangeStudentDelay = 300, GetStudentDelay = 300, GetStudentsDelay = 1000 },
                new StringProvider(this.Context));

            this.HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.student_list_fragment, container, false);

            this.recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            this.filteringResultTextView = view.FindViewById<TextView>(Resource.Id.filter_result_textview);
            this.studentsCountTextView = view.FindViewById<TextView>(Resource.Id.students_count_textview);

            ((AppCompatActivity)this.Activity).SupportActionBar.Title = this.GetString(Resource.String.app_name);

            return view;
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            if (!this.studentAdapter.IsAnyStudents)
            {
                var students = await this.Activity.RunMethodWithLoaderAsync(
                    this.repository.GetStudentsAsync(this.studentFilter));
                this.studentAdapter.SetItems(students);

                this.filteringResultTextView.Visibility = students.Count > 0 ? ViewStates.Invisible
                : ViewStates.Visible;
            }

            this.studentsCountTextView.Text = string.Format(
               CultureInfo.InvariantCulture, this.GetString(Resource.String.student_count_pattern), this.studentAdapter.ItemCount);

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
            var students = await this.Activity.RunMethodWithLoaderAsync(this.repository.GetStudentsAsync(StudentFilter.Default));
            this.studentAdapter.SetItems(students);

            this.filteringResultTextView.Visibility = students.Count > 0 ? ViewStates.Invisible
            : ViewStates.Visible;
            this.studentsCountTextView.Text = string.Format(
               CultureInfo.InvariantCulture, this.GetString(Resource.String.student_count_pattern), this.studentAdapter.ItemCount);
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
