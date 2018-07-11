using System;

namespace StudentList.Model
{
    public class StudentFilter
    {
        public string Name { get; set; }

        public string Group { get; set; }

        public DateTime Birthdate { get; set; }

        public static StudentFilter Default => default(StudentFilter);
    }
}
