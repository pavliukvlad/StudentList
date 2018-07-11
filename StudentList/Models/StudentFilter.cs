using System;

namespace StudentList.Models
{
    public class StudentFilter
    {
        public static StudentFilter Default => new StudentFilter()
        {
            Name = null,
            Group = null,
            Birthdate = DateTime.MinValue
        };

        public string Name { get; set; }

        public string Group { get; set; }

        public DateTime Birthdate { get; set; }
    }
}
