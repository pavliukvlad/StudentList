using System.Collections.Generic;
using StudentList.Models;

namespace StudentList.Domain.Actions
{
    public class StudentsReceived
    {
        public IEnumerable<Student> StudentList { get; set; }
    }
}
