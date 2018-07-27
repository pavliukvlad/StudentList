using System.Collections.Generic;
using StudentList.Models;

namespace StudentList.Domain.States
{
    public class StudentListState
    {
        public StudentListState(ICollection<Student> students)
        {
            this.Students = students;
        }

        public ICollection<Student> Students { get; }
    }
}
