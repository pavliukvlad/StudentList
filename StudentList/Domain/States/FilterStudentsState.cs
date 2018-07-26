using StudentList.Models;

namespace StudentList.Domain.States
{
    public class FilterStudentsState
    {
        public FilterStudentsState(StudentFilter studentFilter)
        {
            this.StudentFilter = studentFilter;
        }

        public StudentFilter StudentFilter { get; }
    }
}
