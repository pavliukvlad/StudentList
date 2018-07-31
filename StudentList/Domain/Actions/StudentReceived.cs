using System.Collections.Generic;
using StudentList.Models;

namespace StudentList.Domain.Actions
{
    public class StudentReceived
    {
        public IEnumerable<Student> StudentList { get; set; }
    }
}
