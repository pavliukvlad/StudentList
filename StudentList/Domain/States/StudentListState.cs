using System.Collections.Generic;
using StudentList.Models;

namespace StudentList.Domain.States
{
    public class StudentListState
    {
        public StudentListState(IEnumerable<StudentImmutable> students)
        {
            this.Students = students;
        }

        public IEnumerable<StudentImmutable> Students { get; }
    }
}
