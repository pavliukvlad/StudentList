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

        public StudentImmutable ToStudentImmutable()
        {
            return new StudentImmutable(
                this.Id, this.Birthdate, this.Name, this.GroupName, this.University, this.Phone, this.ProfilePhoto);
        }
    }
}
