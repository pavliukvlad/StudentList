using System.Collections.Generic;
using StudentList.Models;

namespace StudentList.Domain.States
{
    public class StudentListState
    {
        public StudentListState(IList<Student> students)
        {
            this.Students = students;
        }

        public IList<Student> Students { get; }
    }
}
