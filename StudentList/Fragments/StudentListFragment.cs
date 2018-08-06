using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using StudentList.Adapters;
using StudentList.Domain;
using StudentList.Domain.Actions;
using StudentList.Domain.Actions.ThunkActions;
using StudentList.Domain.States;
using StudentList.Extensions;
using StudentList.Models;
using StudentList.Providers;
using StudentList.Providers.Interfaces;

namespace StudentList.Fragments
{
    public class StudentListFragment : Android.Support.V4.App.Fragment
    {
        private readonly StudentFilter studentFilter;
        private IStore<ApplicationState> store;

        private RecyclerView recyclerView;
        private StudentAdapter studentAdapter;

        private IStudentRepository repository;
        private IDisposable storeSubscription;

        private TextView filteringResultTextView;
        private TextView studentsCountTextView;

        public StudentListFragment(StudentFilter studentFilter)
        {
            this.studentFilter = studentFilter;


        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.store = MainApplication.Store;
            this.studentAdapter = new StudentAdapter();
            this.repository = new StudentsRepository(
                new LoadingDelays { AddStudentDelay = 300, ChangeStudentDelay = 300, GetStudentDelay = 300, GetStudentsDelay = 3000 },
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

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            if (!this.store.GetState().StudentList.Any())
            {
                var studentsReceived = new StudentsReceivedAction(this.Activity, this.repository.GetStudentsAsync());
                this.store.Dispatch(studentsReceived.Action);
            }

            var layoutManager = new LinearLayoutManager(this.Activity);
            this.recyclerView.SetLayoutManager(layoutManager);

            this.recyclerView.SetAdapter(this.studentAdapter);
        }

        public override void OnStart()
        {
            base.OnStart();

            this.studentAdapter.ItemClick += this.StudentAdapterItemClick;
            this.storeSubscription = this.store.Select(
                state => new StudentListModel() { Students = state.StudentList.Select(s => s.ToStudent()), StudentFilter = state.FilterStudentState.StudentFilter })
                .Distinct()
                .Subscribe(this.HandleNewStudents);
        }

        public override void OnStop()
        {
            base.OnStop();

            this.studentAdapter.ItemClick -= this.StudentAdapterItemClick;
            this.storeSubscription.Dispose();
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
                    var addNewStudentAction = new AddNewStudent()
                    {
                        Student = new Student()
                    };
                    this.store.Dispatch(addNewStudentAction);
                    this.ShowStudentProfile();
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
            var editStudentAction = new StudentEditRequestedAction(e.Id);
            this.store.Dispatch(editStudentAction.Action);
            this.ShowStudentProfile();
        }

        private void Reset()
        {
            this.store.Dispatch(new ResetAppliedFilters());
        }

        private void HandleNewStudents(StudentListModel model)
        {
            if (model.Students.Any())
            {
                var filters = model.StudentFilter;
                var students = model.Students;
                students = this.FilterStudents(filters, students);

                this.studentAdapter.SetItems(students.ToList());

                this.filteringResultTextView.Visibility = this.studentAdapter.ItemCount > 0 ? ViewStates.Invisible
                   : ViewStates.Visible;
                this.studentsCountTextView.Text = string.Format(
                CultureInfo.InvariantCulture, this.GetString(Resource.String.student_count_pattern), this.studentAdapter.ItemCount);
            }
        }

        private IEnumerable<Student> FilterStudents(StudentFilter filters, IEnumerable<Student> students)
        {
            if (!filters.IsDefault())
            {
                if (!string.IsNullOrWhiteSpace(filters.Name))
                {
                    students = students.Where(s => s.Name.ToUpperInvariant() == filters.Name.ToUpperInvariant()
                    || s.Name.ToUpperInvariant().Contains(filters.Name.ToUpperInvariant()));
                }

                if (!string.IsNullOrWhiteSpace(filters.Group))
                {
                    students = students.Where(s => s.GroupName.ToUpperInvariant() == filters.Group.ToUpperInvariant()
                    || filters.Group == this.Activity.GetString(Resource.String.filter_group_txt));
                }

                if (filters.Birthdate != default(DateTime))
                {
                    students = students.Where(s => s.Birthdate == filters.Birthdate);
                }
            }

            return students;
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

        private void ShowStudentProfile()
        {
            var studentDetails = StudentProfileFragment.NewInstance();
            this.FragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.main_container, studentDetails)
                .AddToBackStack(null)
                .Commit();
        }
    }
}
