using StudentList.Models;

namespace StudentList.Domain.States
{
    public class StudentProfileState
    {
        public StudentProfileState(StudentImmutable selectedStudent)
        {
            this.SelectedStudent = selectedStudent;
        }

        public StudentImmutable SelectedStudent { get; }
    }
}
