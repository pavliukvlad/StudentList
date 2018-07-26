using StudentList.Models;

namespace StudentList.Domain.States
{
    public class StudentProfileState
    {
        public StudentProfileState(Student selectedStudent)
        {
            this.SelectedStudent = selectedStudent;
        }

        public Student SelectedStudent { get; }
    }
}
