using System.Collections.Generic;

namespace StudentList.Models
{
    public class StudentListModel
    {
        public IEnumerable<Student> Students { get; set; }

        public StudentFilter StudentFilter { get; set; }
    }
}
