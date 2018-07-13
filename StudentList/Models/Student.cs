using System;

namespace StudentList.Models
{
    public class Student
    {
        public string Id { get; set; }

        public DateTime Birthdate { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }

        public string University { get; set; }

        public string Phone { get; set; }

        public Uri ProfilePhoto { get; set; }
    }
}
