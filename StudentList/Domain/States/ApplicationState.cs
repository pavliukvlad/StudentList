using System.Collections.Generic;
using StudentList.Models;

namespace StudentList.Domain.States
{
    public class ApplicationState
    {
        public ApplicationState()
            : this(
                filterStudentsState: new FilterStudentsState(StudentFilter.Default),
                studentList: new List<StudentImmutable>(),
                studentProfileState: new StudentProfileState(new StudentImmutable()))
        {
        }

        public ApplicationState(
            FilterStudentsState filterStudentsState,
            IEnumerable<StudentImmutable> studentList,
            StudentProfileState studentProfileState)
        {
            this.FilterStudentState = filterStudentsState;
            this.StudentList = studentList;
            this.StudentProfileState = studentProfileState;
        }

        public FilterStudentsState FilterStudentState { get; }

        public IEnumerable<StudentImmutable> StudentList { get; }

        public StudentProfileState StudentProfileState { get; }
    }
}
