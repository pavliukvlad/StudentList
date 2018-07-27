namespace StudentList.Domain.States
{
    public class ApplicationState
    {
        public ApplicationState(
            FilterStudentsState filterStudentsState,
            StudentListState studentListState,
            StudentProfileState studentProfileState)
        {
            this.FilterStudentState = filterStudentsState;
            this.StudentListState = studentListState;
            this.StudentProfileState = studentProfileState;
        }

        public FilterStudentsState FilterStudentState { get; }

        public StudentListState StudentListState { get; }

        public StudentProfileState StudentProfileState { get; }
    }
}
