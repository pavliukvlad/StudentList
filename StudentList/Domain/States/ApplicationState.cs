using System.Collections.Generic;
using StudentList.Models;

namespace StudentList.Domain.States
{
    public class ApplicationState
    {
        public ApplicationState()
            : this(
                filterStudentsState: new FilterStudentsState(StudentFilter.Default),
                studentList: null,
                studentProfileState: new StudentProfileState(new Student()))
        {
        }

        public ApplicationState(
            FilterStudentsState filterStudentsState,
            IEnumerable<Student> studentList,
            StudentProfileState studentProfileState)
        {
            this.FilterStudentState = filterStudentsState;
            this.StudentList = studentList;
            this.StudentProfileState = studentProfileState;
        }

        public FilterStudentsState FilterStudentState { get; }

        public IEnumerable<Student> StudentList { get; }

        public StudentProfileState StudentProfileState { get; }
    }
}
