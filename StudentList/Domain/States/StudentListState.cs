using System.Collections.Generic;
using StudentList.Models;

namespace StudentList.Domain.States
{
    public class StudentListState
    {
        public StudentListState(IEnumerable<Student> students)
        {
            this.Students = students;
        }
      
        public IEnumerable<Student> Students { get; }
    }
}
